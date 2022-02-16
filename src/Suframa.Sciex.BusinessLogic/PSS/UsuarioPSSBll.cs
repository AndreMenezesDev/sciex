using DotNetCasClient;
using FluentValidation;
using Newtonsoft.Json;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Security;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.DataAccess.RestService;
using Suframa.Sciex.DataAccess.RestService.ApiDto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Suframa.Sciex.BusinessLogic
{
	public class UsuarioPssBll : IUsuarioPssBll
	{

		private readonly IIntegracaoPssApi _integracaoPssApi;
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IViewImportadorBll _importadorBll;
		

		public UsuarioPssBll(IIntegracaoPssApi integracaoPssApi, IUnitOfWorkSciex uowSciex, IViewImportadorBll iViewImportadorBll)
		{
			_integracaoPssApi = integracaoPssApi;
			_uowSciex = uowSciex;
			_importadorBll = iViewImportadorBll;
		}

		public UsuarioPssVM obterUsuarioInternoPorLogin(string loginUsuario)
		{
			if (PrivateSettings.DEVELOPMENT_MODE)
			{
				if (loginUsuario.Equals("11111111111"))
				{
					return null;
				}
				return obterUsuarioInternoPorLoginMock(loginUsuario);
			}
			UsuarioPSSResDto json = _integracaoPssApi.obterUsuarioInternoPorLogin(loginUsuario);

			return json != null ? preencherDadosBasicoUsuario(loginUsuario, json) : null;
		}

		private UsuarioPssVM obterUsuarioInternoPorLoginMock(string loginUsuario)
		{
			var usuario = new UsuarioPssVM
			{
				usuarioLogadoNome = "UsuarioMock Nome",
				usuarioLogadoCpfCnpj = loginUsuario.CnpjCpfFormat(),
				usuCpfCnpjEmpresaOuLogado = loginUsuario.CnpjCpfFormat(),
				usuNomeRepresentanteLogado = "RepresentanteMock Nome",
				usuSetor = "COVIS",
				usuUnidadeAdministrativa = "MANAUS",
				usuNomeUsuario = "SUFRAMA",
				usuCodmunicipio = 1302603,
				Perfis = new List<EnumPerfil>()
			};
			return usuario;
		}


		public IEnumerable<RepresentacaoVM> ObterRepresentacoesUsuarioLogado()
		{
			var cnpjUsuarioLogado = this.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.Replace(".", "").Replace("-", "").Replace("/", ""); ;

			if (PrivateSettings.DEVELOPMENT_MODE)
			{
				return ObterRepresentacoesUsuarioLogadoMock(cnpjUsuarioLogado.CnpjCpfUnformat());

			}
			return _integracaoPssApi.ObterRepresentacoesUsuarioLogado(cnpjUsuarioLogado);
		}

		public IEnumerable<RepresentacaoVM> ObterRepresentacoesUsuarioLogadoMock(String loginUsuario)
		{
			var listaRepresentacoes = _uowSciex.QueryStackSciex.Representacao.Listar(s => s.CPF == loginUsuario);

			return listaRepresentacoes.Select(rep => new RepresentacaoVM
			{
				Cnpj = rep.CNPJ.CnpjCpfFormat(),
				Nome = rep.RazaoSocial,
				IsUsuarioLogado = false
			});
		}

		public IEnumerable<RepresentacaoVM> ListaEmpresaRepresentadas()
		{
			return ObterRepresentacoesUsuarioLogado();
		}

		public string obterMenuUsuarioLogado()
		{
			var cnpjUsuarioLogado = this.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.Replace(".", "").Replace("-", "").Replace("/", ""); ;
			//string cpf = CasAuthentication.CurrentPrincipal.Identity.Name.CnpjCpfUnformat();
			return _integracaoPssApi.obterMenuUsuarioLogado(cnpjUsuarioLogado);
		}

		private UsuarioPssVM preencherDadosBasicoUsuario(String loginUsuario, UsuarioPSSResDto json)
		{
			UsuarioPssVM usuario = new UsuarioPssVM();
			usuario.usuCpfCnpjEmpresaOuLogado = loginUsuario;
			usuario.usuarioLogadoCpfCnpj = loginUsuario;
			usuario.usuarioLogadoNome = json.nome;
			usuario.usuSetor = json.setorNome;
			usuario.usuUnidadeAdministrativa = json.unidadeAdministrativaNome;
			usuario.usuNomeUsuario = json.nome;
			if (json.municipioId != null)
				usuario.usuCodmunicipio = Int32.Parse(json.municipioId);
			return usuario;
		}

		public UsuarioPssVM PossuiUsuarioLogado()
		{
			var token = JwtManager.GetTokenFromHeader();
			if (token != null)
			{
				var usuarioJWT = JwtManager.GetPrincipal(token);
				if (usuarioJWT == null)
				{
					throw new ValidationException("Usuario não reconhecido");
				}
				return usuarioJWT;
			}
			return null;
		}

		public UsuarioPssVM ObterUsuarioLogado()
		{
			var token = JwtManager.GetTokenFromHeader();
			var usuarioJWT = JwtManager.GetPrincipal(token);
			if (usuarioJWT == null)
			{
				throw new ValidationException("Usuario não reconhecido");
			}
			return usuarioJWT;
		}

		public UsuarioPssVM configurarUsuario(String cnpjUsuarioLogado)
		{
			var url = new PublicSettings().URL_BASE_PSS + "usuario-sistema/usuario-logado-suframa";
			var ticketUrl = "?ticket=" + CasAuthentication.GetProxyTicketIdFor(url);
			HttpClient client = new HttpClient();
			var ususarioPss = client.GetStringAsync(url + ticketUrl).Result;
			UsuarioPSSResDto json = JsonConvert.DeserializeObject<UsuarioPSSResDto>(ususarioPss);

			UsuarioPssVM usuario = new UsuarioPssVM();
			usuario.usuCpfCnpjEmpresaOuLogado = cnpjUsuarioLogado;
			usuario.usuSetor = json.setorNome;
			usuario.usuUnidadeAdministrativa = json.unidadeAdministrativaNome;
			usuario.usuNomeUsuario = json.nome;

			usuario.usuarioLogadoCpfCnpj = cnpjUsuarioLogado;
			usuario.usuarioLogadoNome = json.nome;

			if (json.municipioId != null)
				usuario.usuCodmunicipio = Int32.Parse(json.municipioId);

			usuario.Perfis = RecuperarPerfis(json);

			if (usuario.Perfis.Contains(EnumPerfil.Importador))
			{
				usuario.usuInscricaoCadastral = recuperarInscricaoCadastral(usuario.usuCpfCnpjEmpresaOuLogado.CnpjUnformat());
			}

			return usuario;
		}

		public UsuarioPssVM configurarRepresentacao(RepresentacaoVM representacao)
		{
			if (PrivateSettings.DEVELOPMENT_MODE)
			{
				return configurarRepresentacaoMock(representacao);
			}

			String cnpjUsuarioLogado = this.ObterUsuarioLogado().usuCnpjRepresentanteLogado == null ?
				this.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado : this.ObterUsuarioLogado().usuCnpjRepresentanteLogado;
			String cnpjUsuario = representacao.IsUsuarioLogado == true ? cnpjUsuarioLogado : representacao.Cnpj;
			var url = new PublicSettings().URL_BASE_PSS + "usuario-sistema/usuario-interno-suframa?loginUsuario=" + cnpjUsuario;

			var ticketUrl = "&ticket=" + CasAuthentication.GetProxyTicketIdFor(url);
			HttpClient client = new HttpClient();
			var ususarioPss = client.GetStringAsync(url + ticketUrl).Result;
			UsuarioPSSResDto json = JsonConvert.DeserializeObject<UsuarioPSSResDto>(ususarioPss);

			UsuarioPssVM usuario = new UsuarioPssVM();
			usuario.usuCpfCnpjEmpresaOuLogado = cnpjUsuario;
			usuario.usuSetor = json.setorNome;
			usuario.usuUnidadeAdministrativa = json.unidadeAdministrativaNome;
			usuario.usuNomeUsuario = json.nome;
			usuario.usuNomeRepresentanteLogado = json.nomeRepresentante;
			usuario.usuCnpjRepresentanteLogado = json.cnpjRepresentante;

			usuario.usuarioLogadoCpfCnpj = this.ObterUsuarioLogado().usuarioLogadoCpfCnpj;
			usuario.usuarioLogadoNome = this.ObterUsuarioLogado().usuarioLogadoNome;
			usuario.empresaRepresentadaCnpj = cnpjUsuario.CnpjCpfFormat();
			usuario.empresaRepresentadaRazaoSocial = json.nome;

			usuario.usuNomeEmpresaOuLogado = json.nome != null ? json.nome : usuario.usuarioLogadoNome;

			if (json.municipioId != null)
				usuario.usuCodmunicipio = Int32.Parse(json.municipioId);

			usuario.Perfis = RecuperarPerfis(json);

			if (usuario.Perfis.Contains(EnumPerfil.Importador))
			{
				usuario.usuInscricaoCadastral = recuperarInscricaoCadastral(usuario.usuCpfCnpjEmpresaOuLogado.CnpjUnformat());
			}

			return usuario;
		}

		public int recuperarInscricaoCadastral(String cnpjUsuarioLogado)
		{
			var viewImportador = _importadorBll.Selecionar(cnpjUsuarioLogado);
			return viewImportador.InscricaoCadastral;
		}

		public List<EnumPerfil> RecuperarPerfis(UsuarioPSSResDto json)
		{
			List<EnumPerfil> perfis = new List<EnumPerfil>();
			foreach (var perfilVO in json.perfis)
			{
				if (perfilVO.nome.Contains(EnumPerfil.Importador.GetDescription()))
				{
					perfis.Add(EnumPerfil.Importador);
				}
				else if (perfilVO.nome.Contains(EnumPerfil.Coordenador.GetDescription()))
				{
					perfis.Add(EnumPerfil.Coordenador);
				}
				else if (perfilVO.nome.Contains(EnumPerfil.Analista.GetDescription()))
				{
					perfis.Add(EnumPerfil.Analista);
				}
			}

			return perfis;
		}

		public UsuarioPssVM configurarRepresentacaoMock(RepresentacaoVM representacao)
		{
			String cnpjUsuarioLogado = this.ObterUsuarioLogado().usuCnpjRepresentanteLogado == null ?
				this.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado : this.ObterUsuarioLogado().usuCnpjRepresentanteLogado;
			String cnpjUsuario = representacao.IsUsuarioLogado == true ? cnpjUsuarioLogado : representacao.Cnpj;

			UsuarioPssVM usuario = new UsuarioPssVM();
			usuario.usuCpfCnpjEmpresaOuLogado = cnpjUsuario;
			usuario.usuNomeUsuario = representacao.Nome;

			usuario.usuarioLogadoCpfCnpj = this.ObterUsuarioLogado().usuarioLogadoCpfCnpj;
			usuario.usuarioLogadoNome = this.ObterUsuarioLogado().usuarioLogadoNome;
			usuario.empresaRepresentadaCnpj = cnpjUsuario;
			usuario.empresaRepresentadaRazaoSocial = representacao.Nome;
			usuario.Perfis = new List<EnumPerfil>();
			configurarPerfis(usuario);

			usuario.usuNomeEmpresaOuLogado = representacao.Nome != null ? representacao.Nome : usuario.usuarioLogadoNome;

			if (usuario.Perfis.Contains(EnumPerfil.Importador))
			{
				usuario.usuInscricaoCadastral = recuperarInscricaoCadastral(usuario.usuCpfCnpjEmpresaOuLogado.CnpjUnformat());
			}

			return usuario;
		}

		public UsuarioPssVM configurarUsuarioMock(String cnpjUsuarioLogado)
		{
			var usuario = new UsuarioPssVM
			{
				usuarioLogadoNome = "UsuarioMock Nome",
				usuarioLogadoCpfCnpj = cnpjUsuarioLogado.CnpjCpfFormat(),
				usuCpfCnpjEmpresaOuLogado = cnpjUsuarioLogado.CnpjCpfFormat(),
				usuNomeRepresentanteLogado = "RepresentanteMock Nome",
				usuSetor = "CODOC",
				usuUnidadeAdministrativa = "MANAUS",
				usuNomeEmpresaOuLogado = "KAWASAKI COMPONENTES DA AMAZONIA LTDA",
				usuNomeUsuario = "Usuario Mockado",
				usuCodmunicipio = 1302603,
				Perfis = new List<EnumPerfil>()
			};
			configurarPerfis(usuario);

			if (usuario.Perfis.Contains(EnumPerfil.Importador))
			{
				usuario.usuInscricaoCadastral = recuperarInscricaoCadastral(usuario.usuCpfCnpjEmpresaOuLogado.CnpjUnformat());
			}

			return usuario;
		}

		public IEnumerable<UsuarioPssVM> ListarUsuariosInternos()
		{
			return Enumerable.Empty<UsuarioPssVM>();
		}

		public IEnumerable<UsuarioPssVM> ListarUsuariosInternosPorUC()
		{
			return Enumerable.Empty<UsuarioPssVM>();
		}

		public void configurarPerfis(UsuarioPssVM usuario)
		{
			var cnpjUsuarioLogado = usuario.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();
			var listaDestinatariosAmazonas = new List<String>()
			{
				"04337168000148","04817052000106","05994459000171",
				"09154836000115","04280196000176","04337168000148",
				"04817052000106","05994459000171","63602940000170",
				"63602940000170","09154836000115","04280196000176",
				"04337168000148","04817052000106","05994459000171",
				"63602940000170","09154836000115","04280196000176",
				"13699433000129","09398303000189","14808074000163",
				"16482714000113","04854120000107","34582973000106",
				"13699433000129","09137895000185","04337168000148",
				"04817052000106","05994459000171","04817052000106",
				"08992137000181","08992137000181","08992137000181",
				"08992137000181","24227491000176","01166372000821",
				"14386045000150"
			};
			var listaRepresentantes = new List<String>() {
				"15375951215","19600402272","59288434291","59456912086","66335442272"
			};

			if (listaDestinatariosAmazonas.Contains(cnpjUsuarioLogado.CnpjCpfUnformat()) || cnpjUsuarioLogado.Equals("000000")
						 || cnpjUsuarioLogado.Equals("04337168000148"))
			{
				usuario.Perfis.Add(EnumPerfil.Importador);
			}
			else if (listaRepresentantes.Contains(cnpjUsuarioLogado.CnpjCpfUnformat()))
			{
				usuario.Perfis.Add(EnumPerfil.Preposto);
			}
			else if ("89084748204".Equals(cnpjUsuarioLogado.CnpjCpfUnformat()) || cnpjUsuarioLogado.Equals("00092385060"))
			{
				usuario.Perfis.Add(EnumPerfil.Coordenador);
			}
			else if ("89084748204".Equals(cnpjUsuarioLogado.CnpjCpfUnformat()) || cnpjUsuarioLogado.Equals("00379427273"))
			{
				usuario.Perfis.Add(EnumPerfil.Analista);
			}
		}

		public IEnumerable<UsuarioPssVM> ObterListaUsuariosPerfilAnalista()
		{
			List<UsuarioPssVM> usuarios = new List<UsuarioPssVM>();

			string pssUrl = "https://pss.hom.suframa.gov.br/backend-pss-autorizacao/rest/usuario-sistema/annon/381/perfil";
			try
			{
				HttpClientHandler clientHandler = new HttpClientHandler();
				// Pass the handler to httpclient(from you are calling api)				
				using (var httpClient = new HttpClient(clientHandler))
				{
					// Define Headers
					httpClient.DefaultRequestHeaders.Accept.Clear();
					// Add an Accept header for JSON format.
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					var usuarioPss = httpClient.GetStringAsync(pssUrl).Result;
					List<UsuarioPSSResDto> jsonLista = JsonConvert.DeserializeObject<List<UsuarioPSSResDto>>(usuarioPss);

					foreach (var json in jsonLista)
					{
						UsuarioPssVM usuario = new UsuarioPssVM();
						usuario.usuNomeUsuario = json.nome;
						usuario.usuarioLogadoCpfCnpj = json.codUsuarioExterno;
						usuarios.Add(usuario);
					}
				}
			}
			catch (Exception e)
			{
				//logger.Error("Excao ao logar usuario: " + e.Message);
				return null;
			}

			return usuarios.OrderBy(o => o.usuNomeUsuario);
		}
	}
}

