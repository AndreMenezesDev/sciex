using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{

	public class CorrigirDUEController : ApiController
	{
		private readonly IPlanoExportacaoBll _bll;

		public CorrigirDUEController(IPlanoExportacaoBll planoExportacaoBll)
		{
			_bll = planoExportacaoBll;
		}

		public DuePorProdutoVM Put([FromBody]DuePorProdutoVM view)
		{
			return _bll.CorrigirDocumentosComprobatorios(view);
		}

	}
}