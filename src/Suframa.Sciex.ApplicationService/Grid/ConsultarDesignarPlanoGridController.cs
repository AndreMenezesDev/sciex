using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ConsultarDesignarPlanoGridController : ApiController
    {
		private readonly IDesignarPliBll _designarPliBll;

		public ConsultarDesignarPlanoGridController(IDesignarPliBll designarPliBll)
		{
			_designarPliBll = designarPliBll;
		}

		public PagedItems<PlanoExportacaoVM> Get([FromUri]PlanoExportacaoVM pagedFilter)
		{
			return _designarPliBll.ListarPaginadoPlanosSql(pagedFilter);
		}
	}
}
