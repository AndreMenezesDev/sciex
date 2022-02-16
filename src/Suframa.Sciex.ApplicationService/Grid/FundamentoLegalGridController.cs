using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	/// <summary>Grid de Regime Tributário</summary>
	public class FundamentoLegalGridController : ApiController
	{
		private readonly IFundamentoLegalBll _fundamentoLegalBll;

		/// <summary>Regime Tributário injetar regras de negócio</summary>
		/// <param name="fundamentoLegalBll"></param>
		public FundamentoLegalGridController(IFundamentoLegalBll fundamentoLegalBll)
		{
			_fundamentoLegalBll = fundamentoLegalBll;
		}

		/// <summary>Obter dados para o grid de fundameto Legal paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de fundameto Legal</param>
		/// <returns></returns>
		public PagedItems<FundamentoLegalVM> Get([FromUri]FundamentoLegalPagedFilterVM pagedFilter)
		{
			var paged = _fundamentoLegalBll.ListarPaginado(pagedFilter);
			return paged;
		}
	}
}