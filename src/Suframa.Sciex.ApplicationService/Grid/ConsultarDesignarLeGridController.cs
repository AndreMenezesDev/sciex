using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ConsultarDesignarLeGridController : ApiController
    {
		private readonly IDesignarPliBll _designarPliBll;

		public ConsultarDesignarLeGridController(IDesignarPliBll designarPliBll)
		{
			_designarPliBll = designarPliBll;
		}

		public PagedItems<LEProdutoVM> Get([FromUri]LEProdutoVM pagedFilter)
		{
			return _designarPliBll.ListarPaginadoLesSql(pagedFilter);
		}
	}
}
