using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class HistoricoProcessoSuframaController : ApiController
	{
		IHistoricoProcessoSuframaBll _historicoProcessoSuframaBll;

		public HistoricoProcessoSuframaController(IHistoricoProcessoSuframaBll historicoProcessoSuframaBll)
		{
			_historicoProcessoSuframaBll = historicoProcessoSuframaBll;
		}

		public PagedItems<PRCSolicHistoricoVM> Get([FromUri] PRCStatusVM objeto) =>
			_historicoProcessoSuframaBll.ListarPaginado(objeto);

	}
}