using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ParecerTecnicoRelatorioController : ApiController
	{
		private readonly IParecerTecnicoBll _parecerTecnicoBll;

		public ParecerTecnicoRelatorioController(IParecerTecnicoBll parecerTecnicoBll)
		{
			_parecerTecnicoBll = parecerTecnicoBll;
		}

		public RelatorioParecerTecnicoVM Get(int id)
		{
			return _parecerTecnicoBll.SelecionarRelatorio(id);
		}

	}
}