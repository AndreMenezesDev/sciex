using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class ViewMunicipioDropDownController : ApiController
	{
		private readonly IViewMunicipioBll _viewMunicipioBll;

		/// <summary>Município injetar regras de negócio</summary>
		/// <param name="viewMunicipioBll"></param>
		public ViewMunicipioDropDownController(IViewMunicipioBll viewMunicipioBll)
		{
			_viewMunicipioBll = viewMunicipioBll;
		}

		/// <summary>Obter ID e Descrição dos municípios filtrados</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get()
		{
			return _viewMunicipioBll.ListarViewMunicipio();
		}
		


	}
}