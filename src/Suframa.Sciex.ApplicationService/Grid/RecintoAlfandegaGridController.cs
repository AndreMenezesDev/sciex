using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class RecintoAlfandegaGridController : ApiController
	{
		IRecintoAlfandegaBll _recintoAlfandega;

		public RecintoAlfandegaGridController(IRecintoAlfandegaBll recintoAlfandega)
		{
			_recintoAlfandega = recintoAlfandega;
		}

		public PagedItems<RecintoAlfandegaVM> Get([FromUri] RecintoAlfandegaVM pagedFilter)
		{
			return _recintoAlfandega.ListarPaginado(pagedFilter);
		}

	}
}