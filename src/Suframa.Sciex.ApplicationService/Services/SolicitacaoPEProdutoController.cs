using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class SolicitacaoPEProdutoController : ApiController
    {
		private readonly ISolicitacaoPEBll _solicitacaoPEBll;

		public SolicitacaoPEProdutoController(ISolicitacaoPEBll solicitacaoPEBll)
		{
			_solicitacaoPEBll = solicitacaoPEBll;
		}

		public EstruturaPropriaPEVM Get([FromUri] EstruturaPropriaPEVM pagedFilter)
		{
			return _solicitacaoPEBll.SelecionarEstruturaPropriaPE(pagedFilter);
		}
	}
}
