using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{

	public class InativarDUEController : ApiController
	{
		private readonly IPlanoExportacaoBll _bll;

		public InativarDUEController(IPlanoExportacaoBll planoExportacaoBll)
		{
			_bll = planoExportacaoBll;
		}

		public int Put([FromBody]DuePorProdutoVM view)
		{
			return _bll.InativarDocumentosComprobatorios(view);
		}

	}
}