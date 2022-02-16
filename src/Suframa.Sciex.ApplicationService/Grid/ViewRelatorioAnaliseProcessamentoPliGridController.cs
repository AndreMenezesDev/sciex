using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ViewRelatorioAnaliseProcessamentoPliGridController : ApiController
    {
		private readonly IViewRelatorioAnaliseProcessamentoPli _viewRelatorioAnaliseProcessamentoPliGrid;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="viewRelatorioAnaliseProcessamentoPliGrid"></param>
		public ViewRelatorioAnaliseProcessamentoPliGridController(IViewRelatorioAnaliseProcessamentoPli viewRelatorioAnaliseProcessamentoPliGrid)
		{
			_viewRelatorioAnaliseProcessamentoPliGrid = viewRelatorioAnaliseProcessamentoPliGrid;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<ViewRelatorioAnaliseProcessamentoPliVM> Get([FromUri]ViewRelatorioAnaliseProcessamentoPliVM pagedFilter)
		{
			return _viewRelatorioAnaliseProcessamentoPliGrid.ListarPaginado(pagedFilter);
		}
	}
}
