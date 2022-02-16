using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{

	public class IncluirEditarDueController : ApiController
	{
		private readonly IPlanoExportacaoBll _bll;

		public IncluirEditarDueController(IPlanoExportacaoBll planoExportacaoBll)
		{
			_bll = planoExportacaoBll;
		}


		public DuePorProdutoVM Post([FromBody] DuePorProdutoVM view)
		{
			return _bll.SalvarDocumentosComprobatorios(view);
		}

		public DuePorProdutoVM Put([FromBody]DuePorProdutoVM view)
		{
			return _bll.EditarDocumentosCombprobatorios(view);
		}

	}
}