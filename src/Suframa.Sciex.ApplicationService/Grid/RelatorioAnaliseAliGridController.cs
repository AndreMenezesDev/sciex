using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class RelatorioAnaliseAliGridController : ApiController
    {
		private readonly IAliBll _aliBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="aladiBll"></param>
		public RelatorioAnaliseAliGridController(IAliBll aliBll)
		{
			_aliBll = aliBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<AliVM> Get([FromUri]AliVM pagedFilter)
		{
			return _aliBll.ListarPaginadoRelatorioAli(pagedFilter);
		}
	}
}
