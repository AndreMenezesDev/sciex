using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class FornecedorDropDownController : ApiController
	{
		private readonly IFornecedorBll _fornecedorBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="fornecedor"></param>
		public FornecedorDropDownController(IFornecedorBll fornecedorBll)
		{
			_fornecedorBll = fornecedorBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]FornecedorVM fornecedorVM)
		{
			var lista = _fornecedorBll.ListarChave(fornecedorVM);
			return lista;
		}
	}
}