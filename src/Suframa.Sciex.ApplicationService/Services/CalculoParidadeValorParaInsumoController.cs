using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class CalculoParidadeValorParaInsumoController : ApiController
	{
		private readonly IAnaliseInsumoImportadoBll _analiseInsumoImportadoBll;

		public CalculoParidadeValorParaInsumoController(IAnaliseInsumoImportadoBll analiseInsumoImportadoBll)
		{
			_analiseInsumoImportadoBll = analiseInsumoImportadoBll;
		}

		public decimal? Get()
		{
			var calcularParidade = _analiseInsumoImportadoBll.CalcularParidadeValorPara();
			return calcularParidade;
		}

	}
}