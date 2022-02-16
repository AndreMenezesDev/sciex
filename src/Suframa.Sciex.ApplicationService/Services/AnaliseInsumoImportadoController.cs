using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class AnaliseInsumoImportadoController : ApiController
	{
		private readonly IAnaliseInsumoImportadoBll _analiseInsumoImportadoBll;

		public AnaliseInsumoImportadoController(IAnaliseInsumoImportadoBll analiseInsumoImportadoBll)
		{
			_analiseInsumoImportadoBll = analiseInsumoImportadoBll;
		}

		public List<PRCInsumoVM> Get([FromUri] BuscarValoresInsumoVM objeto) =>

			_analiseInsumoImportadoBll.ListarInsumosImportados(objeto);

	}
}