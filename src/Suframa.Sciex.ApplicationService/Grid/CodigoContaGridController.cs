using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class CodigoContaGridController : ApiController
	{
		private readonly ICodigoContaBll _codigoContaBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="codigoContaBll"></param>
		public CodigoContaGridController(ICodigoContaBll codigoContaBll)
		{
			_codigoContaBll = codigoContaBll;
		}

		/// <summary>Obter dados para o grid de codigoConta paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de codigoConta</param>
		/// <returns></returns>
		public PagedItems<CodigoContaVM> Get([FromUri]CodigoContaVM pagedFilter)
		{
			return _codigoContaBll.ListarPaginado(pagedFilter);
		}
	}
}