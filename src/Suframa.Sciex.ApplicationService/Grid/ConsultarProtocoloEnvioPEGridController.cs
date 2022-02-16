using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ConsultarProtocoloEnvioPEGridController : ApiController
    {
		private readonly ISolicitacaoPEBll _solicitacaoPEBll;

		public ConsultarProtocoloEnvioPEGridController(ISolicitacaoPEBll solicitacaoPEBll)
		{
			_solicitacaoPEBll = solicitacaoPEBll;
		}

		public PagedItems<SolicitacaoPEProdutoVM> Get([FromUri] EstruturaPropriaPEVM pagedFilter)
		{
			return _solicitacaoPEBll.ListarPaginadoSolicitacaoPE(pagedFilter);
		}
	}
}
