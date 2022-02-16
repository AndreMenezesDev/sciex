using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class SolicitacaoPliGridController : ApiController
    {
		private readonly ISolicitacaoPliBll _solicitacaoPliBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="consultarPliBll"></param>
		public SolicitacaoPliGridController(ISolicitacaoPliBll solicitacaoPliBll)
		{
			_solicitacaoPliBll = solicitacaoPliBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<SolicitacaoPliVM> Get([FromUri]SolicitacaoPliVM pagedFilter)
		{
			return _solicitacaoPliBll.ListarPaginado(pagedFilter);
		}
	}
}
