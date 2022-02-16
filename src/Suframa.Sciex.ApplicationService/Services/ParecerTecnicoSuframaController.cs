using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ParecerTecnicoSuframaController : ApiController
	{
		private readonly IParecerTecnicoSuframaBll _bll;

		public ParecerTecnicoSuframaController(IParecerTecnicoSuframaBll bll)
		{
			_bll = bll;
		}

		public ParecerTecnicoVM Get(int id)
		{
			return _bll.Selecionar(id);
		}

		public PagedItems<ParecerTecnicoVM> Get([FromUri] ParecerTecnicoVM vm)
		{
			return _bll.ListarPaginado(vm);
		}

	}
}