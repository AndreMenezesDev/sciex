using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ConsultarDesignarSolicitacaoGridController : ApiController
    {
		private readonly IDesignarPliBll _designarPliBll;

		public ConsultarDesignarSolicitacaoGridController(IDesignarPliBll designarPliBll)
		{
			_designarPliBll = designarPliBll;
		}

		public PagedItems<PRCSolicitacaoAlteracaoVM> Get([FromUri] DesignarSolicitacaoVM pagedFilter)
		{
			return _designarPliBll.ListarPaginadoSolicitacao(pagedFilter);
		}
	}
}
