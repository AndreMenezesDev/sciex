using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class MunicipioDropDownController : ApiController
	{
		private readonly IMunicipioBll _municipioBll;

		public MunicipioDropDownController(IMunicipioBll municipioBll)
		{
			_municipioBll = municipioBll;
		}

		public IEnumerable<object> Get([FromUri]ViewMunicipioVM municipioVM)
		{
			return _municipioBll.ListarChave(municipioVM);
		}
	}
}