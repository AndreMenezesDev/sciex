using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class AnalistaAtivoDropDownController : ApiController
	{
		private readonly IParametrizarAnalistaBll _parametrizarAnalistaBll;
		/// <summary>Classe Atividade injetar regras de negócio dropdown tipo pli</summary>
		/// <param name="LiStatus"></param>
		public AnalistaAtivoDropDownController(IParametrizarAnalistaBll parametrizarAnalistaBll)
		{
			_parametrizarAnalistaBll = parametrizarAnalistaBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]AnalistaVM vm)
		{
			return _parametrizarAnalistaBll.ListarAnalistaPliDropDown();
		}
	}
}