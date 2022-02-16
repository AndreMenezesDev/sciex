using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class PliAplicacaoFixoDropDownController : ApiController
	{
		private readonly IPliAplicacaoBll _pliAplicacaoBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="pliAplicacao"></param>
		public PliAplicacaoFixoDropDownController(IPliAplicacaoBll pliAplicacaoBll)
		{
			_pliAplicacaoBll = pliAplicacaoBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]PliAplicacaoVM pliAplicacaoVM)
		{
			return _pliAplicacaoBll.ListarSemTodos();
		}
	}
}