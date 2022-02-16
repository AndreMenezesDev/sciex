using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;
using System.Web.Mvc;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class SelecionarProdutoEmAnalisePorIdProcessoController : ApiController
    {

		private IProcessoProdutoBll _bll;
		public SelecionarProdutoEmAnalisePorIdProcessoController(IProcessoProdutoBll bll)
		{
			_bll = bll;
		}

        public PRCProdutoVM Get(int id)
        {
            return _bll.SelecionarProdutoEmAnalisePorIdProcesso(id);
        }
		
    }
}
