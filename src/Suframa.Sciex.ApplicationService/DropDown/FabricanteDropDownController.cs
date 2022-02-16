using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class FabricanteDropDownController : ApiController
	{
		private readonly IFabricanteBll _fabricanteBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="fabricante"></param>
		public FabricanteDropDownController(IFabricanteBll fabricanteBll)
		{
			_fabricanteBll = fabricanteBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]FabricanteVM fabricanteVM)
		{
			return _fabricanteBll.ListarChave(fabricanteVM);
		}
	}
}