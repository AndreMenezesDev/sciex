using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PRCHistoricoInsumoSuframaController : ApiController
	{
		private readonly IProcessoInsumoBll _bll;

		public PRCHistoricoInsumoSuframaController(IProcessoInsumoBll bll)
		{
			_bll = bll;
		}

		public List<PRCHistoricoInsumoVM> Get([FromUri] PRCHistoricoInsumoVM parametros)
		{
			return _bll.SelecionarRelatorio(parametros);
		}

	}
}