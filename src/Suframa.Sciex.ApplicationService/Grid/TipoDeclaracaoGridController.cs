using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class TipoDeclaracaoGridController : ApiController
	{
		private readonly ITipoDeclaracaoBll _tipoDeclaracaoBll;

		public TipoDeclaracaoGridController(ITipoDeclaracaoBll tipoDeclaracaoBll)
		{
			_tipoDeclaracaoBll = tipoDeclaracaoBll;
		}

		public PagedItems<TipoDeclaracaoVM> Get([FromUri]TipoDeclaracaoVM pagedFilter)
		{
			return _tipoDeclaracaoBll.ListarPaginado(pagedFilter);
		}
	}
}