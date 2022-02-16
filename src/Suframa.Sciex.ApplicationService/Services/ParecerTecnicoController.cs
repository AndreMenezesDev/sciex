using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ParecerTecnicoController : ApiController
	{
		private readonly IParecerTecnicoBll _parecerTecnicoBll;

		public ParecerTecnicoController(IParecerTecnicoBll parecerTecnicoBll)
		{
			_parecerTecnicoBll = parecerTecnicoBll;
		}

		public ParecerTecnicoVM Get(int id)
		{
			return _parecerTecnicoBll.Selecionar(id);
		}

		public PagedItems<ParecerTecnicoVM> Get([FromUri] ParecerTecnicoVM vm)
		{
			return _parecerTecnicoBll.ListarPaginado(vm);
		}

	}
}