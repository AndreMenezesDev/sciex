using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ParecerTecnicoRelatorioSuframaController : ApiController
	{
		private readonly IParecerTecnicoSuframaBll _bll;

		public ParecerTecnicoRelatorioSuframaController(IParecerTecnicoSuframaBll bll)
		{
			_bll = bll;
		}

		public RelatorioParecerTecnicoVM Get(int id)
		{
			return _bll.SelecionarRelatorio(id);
		}

	}
}