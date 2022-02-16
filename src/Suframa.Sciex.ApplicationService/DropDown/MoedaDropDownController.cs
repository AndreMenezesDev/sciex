using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class MoedaDropDownController : ApiController
	{
		private readonly IMoedaBll _moedaBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="moedaBll"></param>
		public MoedaDropDownController(IMoedaBll moedaBll)
		{
			_moedaBll = moedaBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]MoedaVM moedaVM)
		{
			return _moedaBll.ListarChave(moedaVM);
		}
	}
}