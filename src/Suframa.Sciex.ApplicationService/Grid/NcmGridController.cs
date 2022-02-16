using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class NcmGridController : ApiController
	{
		private readonly INcmBll _ncmBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="ncmAmazoniaOcidentalBll"></param>
		public NcmGridController(INcmBll ncmBll)
		{
			_ncmBll = ncmBll;
		}

		/// <summary>Obter dados para o grid de ncmAmazoniaOcidental paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de ncmAmazoniaOcidental</param>
		/// <returns></returns>
		public PagedItems<NcmVM> Get([FromUri]NcmVM pagedFilter)
		{
			return _ncmBll.ListarPaginado(pagedFilter);
		}
	}
}