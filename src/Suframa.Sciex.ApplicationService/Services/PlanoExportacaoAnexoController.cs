using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PlanoExportacaoAnexoController : ApiController
	{
		private readonly IPlanoExportacaoBll _bll;

		public PlanoExportacaoAnexoController(IPlanoExportacaoBll bll)
		{
			_bll = bll;
		}

		public PlanoExportacaoVM Put([FromBody]PlanoExportacaoVM vm)
		{
			vm = _bll.SalvarAnexo(vm);
			return vm;
		}
	}
}