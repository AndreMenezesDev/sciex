using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class MotivoDropDownController : ApiController
	{
		private readonly IMotivoBll _motivoBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="motivo"></param>
		public MotivoDropDownController(IMotivoBll motivoBll)
		{
			_motivoBll = motivoBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]MotivoVM motivoVM)
		{
			return _motivoBll.ListarChave(motivoVM);
		}
	}
}