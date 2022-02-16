using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class CodigoUtilizacaoGridController : ApiController
	{
		private readonly ICodigoUtilizacaoBll _codigoUtilizacaoBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="codigoUtilizacaoBll"></param>
		public CodigoUtilizacaoGridController(ICodigoUtilizacaoBll codigoUtilizacaoBll)
		{
			_codigoUtilizacaoBll = codigoUtilizacaoBll;
		}

		/// <summary>Obter dados para o grid de codigoUtilizacao paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de codigoUtilizacao</param>
		/// <returns></returns>
		public PagedItems<CodigoUtilizacaoVM> Get([FromUri]CodigoUtilizacaoVM pagedFilter)
		{
			return _codigoUtilizacaoBll.ListarPaginado(pagedFilter);
		}
	}
}