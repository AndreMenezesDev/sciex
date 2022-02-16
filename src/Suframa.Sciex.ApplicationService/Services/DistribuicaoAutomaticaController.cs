using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service paridade cambial</summary>
	public class DistribuicaoAutomaticaController : ApiController
	{
		private readonly IDistribuicaoAutomaticaBll _distribuicaoBll;

		/// <summary>Paridade Cambial injetar regras de negócio</summary>
		/// <param name="paridadeCambialBll"></param>
		public DistribuicaoAutomaticaController(IDistribuicaoAutomaticaBll distribuicaoBll)
		{
			_distribuicaoBll = distribuicaoBll;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hash"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		public JsonResult<string> Distribuir(string hash)
		{
			try
			{
				if (!string.IsNullOrEmpty(hash) && hash.ToUpper().Trim() == "B601EC97F637E8B5EB5C38CB7C5D66CC6CA82AAB5873BDD82584682C96C71AF5FE7B91D48A163EFF91760857409EA34BEE5B37428B09628AD337FD09EA9EAE07")
				{

					var a = _distribuicaoBll.DistribuirPlisAutomaticamente();
					return Json(a.Mensagem);
				}
				else
				{
					return Json("Você não tem autorização para executar este serviço");
				}
			}
			catch (System.Exception ex)
			{
				return Json("Erro ao executar o serviço: "+ex.Message.ToString());
			}



		}
	}
}