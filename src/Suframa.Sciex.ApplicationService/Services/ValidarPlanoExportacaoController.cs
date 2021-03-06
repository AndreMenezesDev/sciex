using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ValidarPlanoExportacaoController : ApiController
	{
		private readonly IPlanoExportacaoBll _planoExportacaoBll;

		public ValidarPlanoExportacaoController(IPlanoExportacaoBll planoExportacaoBll)
		{
			_planoExportacaoBll = planoExportacaoBll;
		}
		
		public ResultadoProcessamentoVM Post([FromBody] PlanoExportacaoVM vm)
		{
			var resultado = new ResultadoProcessamentoVM()
			{
				Resultado = true
			};

			return _planoExportacaoBll.ValidarPlano(vm.IdPlanoExportacao, resultado);
		}
	}
}