using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class PliTipoFixoDropDownController : ApiController
	{
		private readonly IPliTipoBll _pliTipoBll;

		/// <summary>Classe Atividade injetar regras de negócio dropdown tipo pli</summary>
		/// <param name="pliTipoFixo"></param>
		public PliTipoFixoDropDownController(IPliTipoBll pliTipoBll)
		{
			_pliTipoBll = pliTipoBll;
		}

		public IEnumerable<object> Get([FromUri]PliTipoVM pliTipoVM)
		{
			return _pliTipoBll.ListarTipoPliFixo();
		}
	}
}