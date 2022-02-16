using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ConsultarDesignarPliGridController : ApiController
    {
		private readonly IDesignarPliBll _designarPliBll;

		public ConsultarDesignarPliGridController(IDesignarPliBll designarPliBll)
		{
			_designarPliBll = designarPliBll;
		}

		public PagedItems<PliVM> Get([FromUri]PliVM pagedFilter)
		{
			return _designarPliBll.ListarPaginado(pagedFilter);
		}
	}
}
