using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
    public class DeclaracaoUnicaExportacaoController : ApiController
    {
		private IPlanoExportacaoBll _bll;

		public DeclaracaoUnicaExportacaoController(IPlanoExportacaoBll bll)
		{
			_bll = bll;
		}

		// GET: api/DeclaracaoUnicaExportacao/5
		public PagedItems<PlanoExportacaoDUEComplementoVM> Get([FromUri] PEProdutoVM vm)
        {
            return _bll.ListarDUEPaginado(vm);
        }

        // DELETE: api/DeclaracaoUnicaExportacao/5
        public bool Delete(int id)
        {
			return _bll.DeletarDUE(id);
        }
    }
}
