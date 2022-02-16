using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NLog;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Exceptions;
using Suframa.Sciex.CrossCutting.Resources;
using Suframa.Sciex.CrossCutting.Security;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.DataAccess.LdapService;
using Suframa.Sciex.DataAccess.RestService;

namespace Suframa.Sciex.BusinessLogic
{
	public class UsuarioBll : IUsuarioBll
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private readonly IAutenticacaoApi _autenticacaoApi;
		private readonly IAutenticacaoLdap _autenticacaoLdap;
		private readonly IIntegracaoLegadoApi _integracaoLegadoApi;
		private readonly IUnitOfWork _uow;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IUsuarioLogado _usuarioLogado;

		public UsuarioBll(
			IUnitOfWork uow,
			IAutenticacaoApi autenticacaoApi,
			IAutenticacaoLdap autenticacaoLdap,
			IIntegracaoLegadoApi integracaoLegadoApi,
			IUsuarioInformacoesBll usuarioInformacoesBll,
			IUsuarioLogado usuarioLogado)
		{
			_uow = uow;
			_autenticacaoApi = autenticacaoApi;
			_autenticacaoLdap = autenticacaoLdap;
			_usuarioLogado = usuarioLogado;
			_usuarioInformacoesBll = usuarioInformacoesBll;
			_integracaoLegadoApi = integracaoLegadoApi;
		}

		public void AssociarDesassociarProtocolo(UsuarioInternoAssociacaoProtocoloVM usuarioInternoAssociacaoProtocoloVM)
		{
			foreach (var idProtocolo in usuarioInternoAssociacaoProtocoloVM.Protocolos ?? Enumerable.Empty<int>())
			{
				var protocoloEntity = _uow.QueryStack.Protocolo.Selecionar(x => x.IdProtocolo == idProtocolo);
				protocoloEntity.IdUsuarioInterno = usuarioInternoAssociacaoProtocoloVM.IdUsuarioInterno;

				if (protocoloEntity.IdStatusProtocolo == (int)EnumStatusProtocolo.ProntoParaAnalise ||
					protocoloEntity.IdStatusProtocolo == (int)EnumStatusProtocolo.PagamentoRecebido)
				{
					protocoloEntity.IdStatusProtocolo = (int)EnumStatusProtocolo.AguardandoAnalise;
				}

				_uow.CommandStack.Protocolo.Salvar(protocoloEntity);

				if (usuarioInternoAssociacaoProtocoloVM.IdUsuarioInterno.HasValue)
				{
					_uow.CommandStack.WorkflowProtocolo.Salvar(new WorkflowProtocoloEntity
					{
						IdProtocolo = protocoloEntity.IdProtocolo,
						Data = DateTime.Now,
						IdStatusProtocolo = protocoloEntity.IdStatusProtocolo,
						IdUsuarioInterno = protocoloEntity.IdUsuarioInterno,
						Justificativa = "Protocolo distribuído manualmente por: " + _usuarioLogado.Usuario.NomeUsuario
					});
				}
			}

			_uow.CommandStack.Save();
		}

		public void AutenticarUsuarioSenha(string usuario, string senha)
		{
			//Autenticar no SSA (Usuário externo)
			var isAutenticadoSsa = false;

			try
			{
				// O serviço espera que seja enviado um hash MD5 da senha em minúsculo
				isAutenticadoSsa = _autenticacaoApi.Autenticar(usuario, Hash.GerarMD5(senha.ToLower()));
				logger.Info("Autenticado com SSA? " + isAutenticadoSsa);
			}
			catch (Exception ex)
			{
				logger.Warn(ex, string.Format("Tentativa de login SSA {0}", usuario));
			}

			if (isAutenticadoSsa)
			{
				var pessoaFisica = _uow.QueryStack.PessoaFisica.Selecionar(x => x.Cpf.Equals(usuario));
				var pessoaJuridica = _uow.QueryStack.PessoaJuridica.Selecionar(x => x.Cnpj.Equals(usuario));

				// Verificar se a pessoa física ou pessoa jurídica existe
				if (pessoaFisica == null && pessoaJuridica == null)
				{
					throw new RedirectValidationException(Resources.PESSOA_INEXISTENTE, "/menu-externo/opcao");
				}

				var perfis = _usuarioInformacoesBll.ListarPerfis(pessoaFisica?.IdPessoaFisica, pessoaJuridica?.IdPessoaJuridica);

				if (perfis == null || !perfis.Any())
				{
					throw new RedirectValidationException(Resources.PESSOA_INEXISTENTE, "/menu-externo/opcao");
				}

				return;
			}

			//Autenticar no LDAP (Usuário interno)
			var isAutenticadoLdap = false;

			try
			{
				isAutenticadoLdap = _autenticacaoLdap.Autenticar(usuario, senha);
				logger.Info("Autenticado com LDAP? " + isAutenticadoLdap);
			}
			catch (Exception ex)
			{
				logger.Warn(ex, string.Format("Tentativa de login LDAP {0}", usuario));
			}

			if (isAutenticadoLdap)
			{
				var usuarioInterno = _uow.QueryStack.UsuarioInterno.Selecionar(x => x.Cpf.Equals(usuario));

				// Verificar se o usuário interno existe
				if (usuarioInterno == null)
				{
					throw new ValidationException(Resources.USUARIO_INTERNO_INEXISTENTE);
				}

				var papeis = _usuarioInformacoesBll.ListarPapeis(usuarioInterno.IdUsuarioInterno);

				if (papeis == null || !papeis.Any())
				{
					throw new ValidationException(Resources.SEU_USUARIO_NAO_POSSUI_PERFIL_ATRIBUIDO_AO_SISTEMA_CADSUF_COMUNIQUE_O_COORDENADOR_DE_SUA_AREA);
				}

				return;
			}

			throw new ValidationException(Resources.USUARIO_SENHA_INVALIDOS);
		}

		public bool ConsultarSituacaoUsuario(string cpfCnpj)
		{
			var ret = _integracaoLegadoApi.ConsultarSituacaoUsuario(cpfCnpj);

			if (ret.status == 0)
				return true; // desbloqueado
			else
				return false; // bloqueado
		}

		public bool ConsultarSituacaoUsuarioComValidacao(string cpfCnpj)
		{
			try
			{
				return ConsultarSituacaoUsuario(cpfCnpj);
			}
			catch
			{
				throw new ValidationException(Resources.LEGADO_ERRO_COMUNICACAO);
			}
		}

		public IEnumerable<UsuarioInternoVM> Listar(UsuarioInternoVM usuarioInternoVM)
		{
			var unidadeCadastradora = new PublicSettings().ID_MUNICIPIO_UNIDADE_CADASTRADORA;

			var query = _uow.QueryStack.UsuarioInterno
				.ListarGrafo<UsuarioInternoVM>(s => new UsuarioInternoVM
				{
					Cpf = s.Cpf,
					Nome = s.Nome,
					Setor = s.Setor,
					IdUsuarioInterno = s.IdUsuarioInterno,
					IdUnidadeCadastradora = s.IdUnidadeCadastradora,
					IsUnidadeCadastradoraManaus = s.ParametroAnalista.Any(x => x.UnidadeCadastradora.Municipio.IdMunicipio == unidadeCadastradora),
					IsCoordenadorDescentralizada = s.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.CoordenadorDescentralizada),
					IsCoordenadorGeralCOCAD = s.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.CoordenadorGeralCocad),
					IsCoordenadorOutrasAreas = s.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.CoordenadorOutrasAreas),
					IsSuperintendenteAdjunto = s.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.SuperintendenteAdjunto),
					IsTecnico = s.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.Tecnico)
				}, w =>
				(!usuarioInternoVM.IdUsuarioInterno.HasValue || w.IdUsuarioInterno == usuarioInternoVM.IdUsuarioInterno) &&
				(!usuarioInternoVM.IdUnidadeCadastradora.HasValue || w.IdUnidadeCadastradora == usuarioInternoVM.IdUnidadeCadastradora)
			);

			return query;
		}

		public IEnumerable<UsuarioInternoVM> ListarAgendamento(UsuarioInternoVM usuarioInternoVM)
		{
			var unidadeCadastradora = new PublicSettings().ID_MUNICIPIO_UNIDADE_CADASTRADORA;

			var unidadeCadastradoraEntity = _uow.QueryStack.UnidadeCadastradora.Selecionar(x => x.IdUnidadeCadastradora == usuarioInternoVM.IdUnidadeCadastradora);

			var lista = Listar(usuarioInternoVM);

			if (unidadeCadastradoraEntity.IdMunicipio == unidadeCadastradora)
			{
				// Filtrar COCAD
				return lista.Where(x => x.Setor == "COCAD");
			}

			return lista;
		}

		public IEnumerable<UsuarioInternoVM> ListarParaParametroAnalista(UsuarioInternoVM usuarioInternoVM)
		{
			return _uow.QueryStack.UsuarioInterno.Listar<UsuarioInternoVM>(x =>
				(x.ParametroAnalista.Any(y => y.IsStatusAtivoProtocolo == 1)) &&
				(!usuarioInternoVM.IdUsuarioInterno.HasValue || x.IdUsuarioInterno == usuarioInternoVM.IdUsuarioInterno) &&
				(!usuarioInternoVM.IdUnidadeCadastradora.HasValue || x.ParametroAnalista.Any(y => y.IdUnidadeCadastradora == usuarioInternoVM.IdUnidadeCadastradora))
			);
		}

		public IEnumerable<UsuarioInternoVM> ListarParaUsuarioInterno()
		{
			var unidadeCadastradora = new PublicSettings().ID_MUNICIPIO_UNIDADE_CADASTRADORA;
			var idUsuarioInterno = _usuarioLogado.Usuario.IdUsuarioInterno;

			var usuarioInterno = _uow.QueryStack.UsuarioInterno.Selecionar<UsuarioInternoVM>(x => x.IdUsuarioInterno == idUsuarioInterno);

			if (usuarioInterno != null)
			{
				var lista = _uow.QueryStack.UsuarioInterno.ListarGrafo<UsuarioInternoVM>(s => new UsuarioInternoVM()
				{
					Cpf = s.Cpf,
					Nome = s.Nome,
					Setor = s.Setor,
					IdUsuarioInterno = s.IdUsuarioInterno,
					IdUnidadeCadastradora = s.IdUnidadeCadastradora,
					IsUnidadeCadastradoraManaus = s.ParametroAnalista.Any(x => x.UnidadeCadastradora.Municipio.IdMunicipio == unidadeCadastradora),
					IsCoordenadorDescentralizada = s.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.CoordenadorDescentralizada),
					IsCoordenadorGeralCOCAD = s.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.CoordenadorGeralCocad),
					IsCoordenadorOutrasAreas = s.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.CoordenadorOutrasAreas),
					IsSuperintendenteAdjunto = s.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.SuperintendenteAdjunto),
					IsTecnico = s.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.Tecnico)
				}, w => w.IdUnidadeCadastradora == usuarioInterno.IdUnidadeCadastradora);

				return lista;
			}

			return Enumerable.Empty<UsuarioInternoVM>();
		}

		public UsuarioInternoVM Selecionar(UsuarioInternoVM usuarioInternoVM)
		{
			return Listar(usuarioInternoVM).SingleOrDefault();
		}

		public int SelecionarQuantidadeProtocolos(UsuarioInternoVM usuarioInternoVM)
		{
			if (usuarioInternoVM == null || !usuarioInternoVM.IdUsuarioInterno.HasValue) { return 0; }

			return _uow.QueryStack.Protocolo.Contar(x =>
				x.IdUsuarioInterno == usuarioInternoVM.IdUsuarioInterno &&
				StatusProtocoloGrupoDto.StatusAnaliseUsuario.Contains(x.IdStatusProtocolo)
			);
		}

		public UsuarioInternoVM SelecionarUsuarioLogado()
		{
			if (_usuarioLogado == null || !_usuarioLogado.Usuario.IdUsuarioInterno.HasValue) { return null; }

			return Listar(new UsuarioInternoVM { IdUsuarioInterno = _usuarioLogado.Usuario.IdUsuarioInterno }).SingleOrDefault();
		}

		public void Salvar(UsuarioInternoVM usuarioInternoVM)
		{
			if (_usuarioLogado == null || !_usuarioLogado.Usuario.IdUsuarioInterno.HasValue) { return ; }

			_usuarioLogado.Usuario.CpfCnpj = usuarioInternoVM.Cnpj;
			
			_usuarioLogado.Load();

		}
	}
}