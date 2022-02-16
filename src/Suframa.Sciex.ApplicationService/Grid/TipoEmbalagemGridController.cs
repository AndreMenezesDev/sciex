using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class TipoEmbalagemGridController : ApiController
	{
		ITipoEmbalagemBll _tipoEmbalagem;
		public TipoEmbalagemGridController(ITipoEmbalagemBll tipoEmbalagem)
		{
			_tipoEmbalagem = tipoEmbalagem;
		}

		public PagedItems<TipoEmbalagemVM> Get([FromUri] TipoEmbalagemVM pagedFilter)
		{
			return _tipoEmbalagem.ListarPaginado(pagedFilter);
		}

	}
}