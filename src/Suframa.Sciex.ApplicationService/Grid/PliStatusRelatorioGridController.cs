using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class PliStatusRelatoriosGridController : ApiController
    {
		private readonly IPliStatusRelatorioBll _pliStatusRelatorioGridBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="pliStatusBll"></param>
		public PliStatusRelatoriosGridController(IPliStatusRelatorioBll pliStatusRelatorioGridBll)
		{
			_pliStatusRelatorioGridBll = pliStatusRelatorioGridBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<PliMercadoriaVM> Get([FromUri]PliMercadoriaVM pagedFilter)
		{
			return _pliStatusRelatorioGridBll.ListarPaginado(pagedFilter);
		}
	}
}
