using System;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.DataAccess;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Security;
using Suframa.Sciex.CrossCutting.DataTransferObject;

namespace Suframa.Sciex.BusinessLogic
{
	public class UsuarioInformacoesBll : IUsuarioInformacoesBll
	{
		private readonly IUnitOfWork _uow;
		private readonly IUnitOfWorkSciex _uowSciex;
	

		public UsuarioInformacoesBll(
			IUnitOfWork uow, IUnitOfWorkSciex uowSciex
			)
		{
			_uow = uow;
			_uowSciex = uowSciex;
		
		}
		
		public string ObterCNPJ()
		{
			var token = JwtManager.GetTokenFromHeader();
			if (string.IsNullOrEmpty(token))
				return "";

			var Usuario = JwtManager.GetPrincipal(token);

			/*return Usuario.CpfCnpj.Length == 11 ?
				(Usuario.ListaEmpresaRepresentadas.Count() > 0 ?
					Usuario.CNPJRepresentacao : Usuario.CpfCnpj) 
				: Usuario.CpfCnpj;*/

			return Usuario.usuCpfCnpjEmpresaOuLogado;
		}

		public IEnumerable<EnumPapel> ListarPapeis(int idUsuarioInterno)
		{
			var papeis = _uow.QueryStack.UsuarioPapel
				.ListarGrafo<PapelDto>(s => new PapelDto() { IdPapel = s.IdPapel, IdUsuarioInterno = s.IdUsuarioInterno },
				w => w.IdUsuarioInterno == idUsuarioInterno)
				.Select(s => s.IdPapel)
				.Distinct()
				.Cast<EnumPapel>();

			return papeis;
		}

		public IEnumerable<EnumPerfil> ListarPerfis(int? idPessoaFisica, int? idPessoaJuridica)
		{
			var perfis = _uow.QueryStack.InscricaoCadastralCredenciamento
				.ListarGrafo<PerfilDto>(s => new PerfilDto()
				{
					IdPessoaFisica = s.IdPessoaFisica,
					IdPessoaJuridica = s.IdPessoaJuridica,
					IdTipoUsuario = s.IdTipoUsuario
				},
					w => (idPessoaFisica.HasValue && w.IdPessoaFisica == idPessoaFisica)
							|| (idPessoaJuridica.HasValue && w.IdPessoaJuridica == idPessoaJuridica)
							)
				.Select(s => s.IdTipoUsuario.Value)
				.Distinct()
				.Cast<EnumPerfil>();

			return perfis;
		}

		public int? ObterIdPessoaFisica(string cpf)
		{
			var member = _uow.QueryStack.PessoaFisica.Selecionar(w => w.Cpf == cpf);
			return member != null ? (int?)member.IdPessoaFisica : null;
		}

		public int? ObterIdPessoaJuridica(string cnpj)
		{
			var member = _uow.QueryStack.PessoaJuridica.Selecionar(w => w.Cnpj == cnpj);
			return member != null ? (int?)member.IdPessoaJuridica : null;
		}

		public string ObterSetorUsuarioInterno(string cpf)
		{
			var member = _uow.QueryStack.UsuarioInterno.Selecionar(w => w.Cpf == cpf);
			return member?.Setor;
		}

		public UsuarioInternoEntity ObterUsuarioInterno(string cpf)
		{
			return _uow.QueryStack.UsuarioInterno.Selecionar(w => w.Cpf == cpf);
		}

		public string ObterDadosImportador(string cnpj)
		{
			return _uowSciex.QueryStackSciex.ViewImportador.Selecionar(w => w.Cnpj == cnpj).RazaoSocial;
		}

		public string ObterDadosPreposto(string cpf)
		{
			return _uow.QueryStack.PessoaFisica.Selecionar(w => w.Cpf == cpf).Nome.ToUpper();
		}

		/*
		public IEnumerable<RepresentacaoVM> ListaEmpresaRepresentadas()
		{
			var token = JwtManager.GetTokenFromHeader();
			if (string.IsNullOrEmpty(token))
				return null;

			var Usuario = JwtManager.GetPrincipal(token);

			return Usuario.ListaEmpresaRepresentadas;
		}

		public TokenDto ObterDadosUsuario()
		{
			var token = JwtManager.GetTokenFromHeader();
			if (string.IsNullOrEmpty(token))
				return null;

			return JwtManager.GetPrincipal(token);
		}*/
	}
}