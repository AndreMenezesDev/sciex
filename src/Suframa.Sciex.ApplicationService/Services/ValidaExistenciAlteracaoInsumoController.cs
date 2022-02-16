using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class ValidaExistenciAlteracaoInsumoController : ApiController
	{
		IProcessoInsumoBll _processoInsumoBll;
		public ValidaExistenciAlteracaoInsumoController(IProcessoInsumoBll processoInsumoBll)
		{
			_processoInsumoBll = processoInsumoBll;
		}

		public string Get([FromUri]ListarProcessoInsumosNacionalImportadosVM objeto)
		{
			return _processoInsumoBll.ValidaSeUsuarioAlterouInsumos(objeto);
		}
	}
}