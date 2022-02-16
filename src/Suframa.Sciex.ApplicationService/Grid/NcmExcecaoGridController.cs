using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class NcmExcecaoGridController : ApiController
	{
		private readonly INcmExcecaoBll _ncmExcecaoBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="ncmExcecaoBll"></param>
		public NcmExcecaoGridController(INcmExcecaoBll ncmExcecaoBll)
		{
			_ncmExcecaoBll = ncmExcecaoBll;
		}

		/// <summary>Obter dados para o grid de ncmAmazoniaOcidental paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de ncmAmazoniaOcidental</param>
		/// <returns></returns>
		public PagedItems<NcmExcecaoVM> Get([FromUri]NcmExcecaoVM pagedFilter)
		{
			return _ncmExcecaoBll.ListarPaginado(pagedFilter);
		}
	}
}