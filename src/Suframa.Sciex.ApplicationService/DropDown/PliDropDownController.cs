using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class PliDropDownController : ApiController
	{
		private readonly IPliBll _pliBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="pli"></param>
		public PliDropDownController(IPliBll pliBll)
		{
			_pliBll = pliBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]PliVM pliVM)
		{
			return _pliBll.Listar();
		}
	}
}