using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class IncotermsDropDownController : ApiController
	{
		private readonly IIncotermsBll _incotermsBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="Incoterms"></param>
		public IncotermsDropDownController(IIncotermsBll incotermsBll)
		{
			_incotermsBll = incotermsBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]IncotermsVM incotermsVM)
		{
			return _incotermsBll.ListarChave(incotermsVM);
		}
	}
}