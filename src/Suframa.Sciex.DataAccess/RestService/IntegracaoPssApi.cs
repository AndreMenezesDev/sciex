using DotNetCasClient;
using Newtonsoft.Json;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.RestService.ApiDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Suframa.Sciex.DataAccess.RestService
{
	public class IntegracaoPssApi : IIntegracaoPssApi
	{
		public UsuarioPSSResDto obterUsuarioInternoPorLogin(string loginUsuario)
		{
			try
			{
				var url = new PublicSettings().URL_BASE_PSS + "usuario-sistema/usuario-interno-suframa?loginUsuario=" + loginUsuario;
				var ticketUrl = "&ticket=" + CasAuthentication.GetProxyTicketIdFor(url);
				HttpClient client = new HttpClient();
				var usuarioPss = client.GetStringAsync(url + ticketUrl).Result;
				UsuarioPSSResDto json = JsonConvert.DeserializeObject<UsuarioPSSResDto>(usuarioPss);

				return json;
			}
			catch (Exception e)
			{
				return null;
			}

		}

		public string obterMenuUsuarioLogado(string loginUsuario)
		{
			var url = new PublicSettings().URL_BASE_PSS + "contexto-funcao/shell2?sistemaSigla=SCIEX&usuario=" + loginUsuario;
			var ticketUrl = "&ticket=" + CasAuthentication.GetProxyTicketIdFor(url);
			HttpClient client = new HttpClient();

			var json = client.GetStringAsync(url + ticketUrl).Result;
			var teste = Regex.Unescape(json);
			return teste;
		}

		public IEnumerable<RepresentacaoVM> ObterRepresentacoesUsuarioLogado(string loginUsuario)
		{
			var url = new PublicSettings().URL_BASE_PSS + "usuario-representante/representacao-usuario";

			var ticketUrl = "?ticket=" + CasAuthentication.GetProxyTicketIdFor(url);
			HttpClient client = new HttpClient();
			var representacoes = client.GetStringAsync(url + ticketUrl).Result;
			List<RepresentanteLegalApiDto> json = JsonConvert.DeserializeObject<List<RepresentanteLegalApiDto>>(representacoes);

			return json.Select(rep => new RepresentacaoVM
			{
				Cnpj = rep.CodUsuarioExterno,
				Nome = rep.Nome,
				IsUsuarioLogado = false
			});
		}
		
		//TODO WILLY REMOVER
		/*private IEnumerable<RepresentacaoVM> ObterRepresentacoesUsuarioLogadoMock(string loginUsuario)
		{
			var representacao = new RepresentacaoVM()
			{
				Cnpj = "02334158000123",
				Nome = "castelobr"
			};

			List<RepresentacaoVM> list = new List<RepresentacaoVM>();
			list.Add(representacao);

			return list;
		}*/
	}
}
