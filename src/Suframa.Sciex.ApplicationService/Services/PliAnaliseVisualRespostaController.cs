using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PliAnaliseVisualRespostaController : ApiController
	{
		private readonly IPliAnaliseVisualBll _pliAnaliseVisualBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="pliBll"></param>
		public PliAnaliseVisualRespostaController(IPliAnaliseVisualBll pliAnaliseVisualBll)
		{
			_pliAnaliseVisualBll = pliAnaliseVisualBll;
		}

		/// <summary>Valida a regra de cadastro do PLI</summary>
		/// <param name="pliVM"></param>
		/// <returns></returns>
		public PliVM Put([FromBody]PliVM pliVM)
		{
			pliVM.LocalPastaEstruturaArquivo = HttpContext.Current.Server.MapPath("ArquivoPliResposta");
			pliVM = _pliAnaliseVisualBll.SalvarResposta(pliVM);
			return pliVM;
		}

	}
}