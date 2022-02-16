
using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	//
	public class ViaTransporteGridController: ApiController
	{
		private readonly IViaTransporteBll _viaTransporteBll;

		//
		public ViaTransporteGridController(IViaTransporteBll viaTransporteBll)
		{
			_viaTransporteBll = viaTransporteBll;
		}

		//
		public PagedItems<ViaTransporteVM> Get([FromUri] ViaTransporteVM pagedFilter)
		{
			return _viaTransporteBll.ListarPaginado(pagedFilter);
		}
	}
}