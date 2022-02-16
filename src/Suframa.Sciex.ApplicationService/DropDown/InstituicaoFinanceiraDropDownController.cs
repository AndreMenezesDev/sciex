using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class InstituicaoFinanceiraDropDownController : ApiController
	{
		private readonly IInstituicaoFinanceiraBll _instituicaoFinanceiraBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="instituicaoFinanceiraBll"></param>
		public InstituicaoFinanceiraDropDownController(IInstituicaoFinanceiraBll instituicaoFinanceiraBll)
		{
			_instituicaoFinanceiraBll = instituicaoFinanceiraBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]InstituicaoFinanceiraVM instituicaoFinanceiraVM)
		{
			return _instituicaoFinanceiraBll.ListarChave(instituicaoFinanceiraVM);
		}
	}
}