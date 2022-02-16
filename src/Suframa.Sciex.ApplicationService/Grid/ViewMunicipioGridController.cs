using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class ViewMunicipioGridController : ApiController
	{
		private readonly IViewMunicipioBll _viewMunicipioBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="viewNcmBll"></param>
		public ViewMunicipioGridController(IViewMunicipioBll viewMunicipioBll)
		{
			_viewMunicipioBll = viewMunicipioBll;
		}

		public IEnumerable<ViewMunicipioVM> Get([FromUri]ViewMunicipioVM viewMunicipioVM)
		{
			return _viewMunicipioBll.Listar(viewMunicipioVM);
		}
	}
}