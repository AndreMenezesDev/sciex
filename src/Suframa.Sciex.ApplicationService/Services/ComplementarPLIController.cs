using Newtonsoft.Json;
using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ComplementarPLIController : ApiController
	{
		private readonly IComplementarPLIBll _complementarPLIBll;
		private readonly IParidadeCambialBll _paridadeCambialBll;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="complementarPLIBll"></param>
		/// <param name="paridadeCambialBll"></param>
		public ComplementarPLIController(IComplementarPLIBll complementarPLIBll, IParidadeCambialBll paridadeCambialBll)
		{
			_complementarPLIBll = complementarPLIBll;
			_paridadeCambialBll = paridadeCambialBll;

		}

		/// <summary>Acessa serviço</summary>
		/// <param name="hash"></param>
		[AllowAnonymous]
		[HttpGet]
		public JsonResult<string> Put(string hash)
		{
			if (!string.IsNullOrEmpty(hash) &&
				hash.ToUpper().Trim() == "A71E3FE602EA53827A3AB490709C94F4E13F84E90F6D07100F38052D22A99DF8F63E8BE20E2E753AC40F03C3B73B194555A79E8E717888525E5288298F53B7F1")
			{
				var r = _complementarPLIBll.ComplementarPLI("");
				if (string.IsNullOrEmpty(r))
				{				
					return Json("Serviço executado sem PLIs para complementar.");
				}
				else
				{
					//ParidadeCambialGenerator paridadeCambialGenerator = new ParidadeCambialGenerator();
					//paridadeCambialGenerator.DataParidade = DateTime.Now.AddDays(1);
					//paridadeCambialGenerator.TipoGeracao = 3;

					//_paridadeCambialBll.BaixarArquivoParidadeEmail(paridadeCambialGenerator);
					return Json(r);
				}
			}
			else
			{
				var dado = "Você não tem autorização para executar este serviço";
				return Json(dado);				
			}


		}
	}
}