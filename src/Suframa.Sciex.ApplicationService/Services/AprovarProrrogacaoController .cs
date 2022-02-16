using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class AprovarProrrogacaoController : ApiController
	{
		private readonly IConsultarProcessoExportacaoBll _consultarProcessoBll;

		public AprovarProrrogacaoController(IConsultarProcessoExportacaoBll consultarProcessoBll)
		{
			_consultarProcessoBll = consultarProcessoBll;
		}

		public PRCStatusVM Get([FromUri] PRCStatusVM filtro)
		{
			return _consultarProcessoBll.AprovarProrogacao(filtro);
		}
	}
}