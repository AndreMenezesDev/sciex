using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class SetorArmazenamentoGridController : ApiController
	{
		private readonly ISetorArmazenamentoBll _setorArmazenamentoBll;

		public SetorArmazenamentoGridController(ISetorArmazenamentoBll setorArmazenamentoBll)
		{
			_setorArmazenamentoBll = setorArmazenamentoBll;
		}

		public PagedItems<SetorArmazenamentoVM> Get([FromUri]SetorArmazenamentoVM pagedFilter)
		{
			return _setorArmazenamentoBll.ListarPaginado(pagedFilter);
		}
	}
}