using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class AnalistaAtivoLeDropDownController : ApiController
	{
		private readonly IParametrizarAnalistaBll _parametrizarAnalistaBll;

		public AnalistaAtivoLeDropDownController(IParametrizarAnalistaBll parametrizarAnalistaBll)
		{
			_parametrizarAnalistaBll = parametrizarAnalistaBll;
		}

		public IEnumerable<object> Get([FromUri]AnalistaVM vm)
		{
			return _parametrizarAnalistaBll.ListarAnalistaLeDropDown();
		}
	}
}