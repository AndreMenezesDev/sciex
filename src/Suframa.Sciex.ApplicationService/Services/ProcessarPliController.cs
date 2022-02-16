using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service paridade cambial</summary>
	public class ProcessarPliController : ApiController
	{
		private readonly IPliBll _pliBll;

		/// <summary>Paridade Cambial injetar regras de negócio</summary>
		/// <param name="paridadeCambialBll"></param>
		public ProcessarPliController(IPliBll pliBll)
		{
			_pliBll = pliBll;

		}

		/// <summary>Baixar/Copiar Paridade Cambial</summary>
		/// <param name="paridadeCambialGenerator">Objeto natureza jurídica a ser persistido</param>
		/// <returns></returns>		
		[AllowAnonymous]
		[HttpGet]
		public JsonResult<string> ProcessarPli(string hash)
		{
			try
			{
				if (!string.IsNullOrEmpty(hash) && hash.ToUpper().Trim() == "A71E3FE602EA53827A3AB490709C94F4E13F84E90F6D07100F38052D22A99DF8F63E8BE20E2E753AC40F03C3B73B194555A79E8E717888525E5288298F53B7F1")
				{
					_pliBll.ProcessarPLI();
					return Json("Sucesso");
				}
				else
				{
					return Json("Você não tem autorização para executar este serviço");
				}
			}
			catch (System.Exception ex)
			{
				return Json("Erro ao executar o serviço");
			}
			
		}
	}
}