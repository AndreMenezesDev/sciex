using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class EntregarPlanoExportacaoController : ApiController
	{
		private readonly IPlanoExportacaoBll _planoExportacaoBll;

		public EntregarPlanoExportacaoController(IPlanoExportacaoBll planoExportacaoBll)
		{
			_planoExportacaoBll = planoExportacaoBll;
		}

		public ResultadoProcessamentoVM Post([FromBody] PlanoExportacaoVM vm)
		{
			return _planoExportacaoBll.EntregarPlano(vm);
		}
		
	}
}