using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class HistoricoProcessoController : ApiController
	{
		IHistoricoProcessoBll _historicoProcessoBll;

		public HistoricoProcessoController(IHistoricoProcessoBll historicoProcessoBll)
		{
			_historicoProcessoBll = historicoProcessoBll;
		}

		public PagedItems<PRCSolicHistoricoVM> Get([FromUri] PRCStatusVM objeto) =>
			_historicoProcessoBll.ListarPaginado(objeto);

	}
}