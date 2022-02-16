using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	/// <summary>
	/// Serviço GRID - PLI Processo Anuente
	/// </summary>
    public class PliProcessoAnuenteGridController : ApiController
    {
		private readonly IPliProcessoAnuenteBll _pliProcessoAnuenteBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="pliProcessoAnuenteBll"></param>
		public PliProcessoAnuenteGridController(IPliProcessoAnuenteBll pliProcessoAnuenteBll)
		{
			_pliProcessoAnuenteBll = pliProcessoAnuenteBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<PliProcessoAnuenteVM> Get([FromUri]PliProcessoAnuenteVM pagedFilter)
		{
			return _pliProcessoAnuenteBll.ListarPaginado(pagedFilter);
		}
	}
}
