using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class PliTipoDropDownController : ApiController
	{
		private readonly IPliTipoBll _pliTipoBll;

		/// <summary>Classe Atividade injetar regras de negócio dropdown tipo pli</summary>
		/// <param name="pliTipo"></param>
		public PliTipoDropDownController(IPliTipoBll pliTipoBll)
		{
			_pliTipoBll = pliTipoBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]PliTipoVM pliTipoVM)
		{
			return _pliTipoBll.ListarTipoPli();
		}

		public IEnumerable<object> ListarPli([FromUri]PliTipoVM pliTipoVM)
		{
			return _pliTipoBll.ListarTipoPliFixo();
		}
	}
}