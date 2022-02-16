using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.Compressor;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Mensagens;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service EstruturaPropriaPLI</summary>
	public class ValidarEstruturaPropriaLEController : ApiController
	{
		private readonly ISolicitacaoLEInsumoBll _solicitacaoLEInsumoBll;

		public ValidarEstruturaPropriaLEController(ISolicitacaoLEInsumoBll solicitacaoLEInsumoBll)
		{
			_solicitacaoLEInsumoBll = solicitacaoLEInsumoBll;
		}

		/// <summary>Enviar arquivo</summary>
		/// <param name="hash">Codigo para manter segurança de acesso</param>
		/// <returns></returns>		
		[AllowAnonymous]
		[HttpGet]
		public JsonResult<string> EnviarArquivo(string hash)
		{
			//try
			//{
				if (!string.IsNullOrEmpty(hash) && hash.ToUpper().Trim() == "FAB614FB7753E0BA24AA7314FAC5AD74D37AE38A6D58B5F20FEC7690B7C308C5AA562B40EA0892A743E639C97F675CA04BF9A4EF98FB85B782E85F7FE14A96CF")
				{
					//try
					//{
						string msg = _solicitacaoLEInsumoBll.LeituraArquivoInserirDados();
						return Json(msg);
					//}
					//catch (Exception ex)
					//{
					//	return Json("Problema na validação: " + ex.Message.ToString());
					//}

				}
				else
				{
					return Json("Erro de executação ou você não tem autorização para executar este serviço.");
				}
			//}
			//catch (System.Exception ex)
			//{
			//	return Json("Erro ao executar o serviço : " + ex.Message.ToString());
			//}

		}
	}
}