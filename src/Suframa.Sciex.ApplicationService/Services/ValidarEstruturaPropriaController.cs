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
	public class ValidarEstruturaPropriaController : ApiController
	{
		private readonly ISolicitacaoPliBll _solicitacaoPliBll;

		/// <summary>Construtor</summary>
		/// <param name="solicitacaoPliBll"></param>
		public ValidarEstruturaPropriaController(
			ISolicitacaoPliBll solicitacaoPliBll)
		{
			_solicitacaoPliBll = solicitacaoPliBll;
		}

		/// <summary>Enviar arquivo</summary>
		/// <param name="hash">Codigo para manter segurança de acesso</param>
		/// <returns></returns>		
		[AllowAnonymous]
		[HttpGet]
		public JsonResult<string> EnviarArquivo(string hash)
		{
			try
			{
				if (!string.IsNullOrEmpty(hash) && hash.ToUpper().Trim() == "A71E3FE602EA53827A3AB490709C94F4E13F84E90F6D07100F38052D22A99DF8F63E8BE20E2E753AC40F03C3B73B194555A79E8E717888525E5288298F53B7F1")
				{
					try
					{
						string msg = _solicitacaoPliBll.LeituraArquivoInserirDados();
						return Json(msg);
					}
					catch (Exception ex)
					{
						return Json("Problema na validação: " + ex.Message.ToString());
					}

				}
				else
				{
					return Json("Erro de executação ou você não tem autorização para executar este serviço.");
				}
			}
			catch (System.Exception ex)
			{
				return Json("Erro ao executar o serviço : " + ex.Message.ToString());
			}

		}
	}
}