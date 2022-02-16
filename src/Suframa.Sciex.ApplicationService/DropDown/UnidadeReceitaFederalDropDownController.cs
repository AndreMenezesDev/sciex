using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class UnidadeReceitaFederalDropDownController : ApiController
	{
		private readonly IUnidadeReceitaFederalBll _unidadeReceitaFederal;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="unidadeReceitaFederal"></param>
		public UnidadeReceitaFederalDropDownController(IUnidadeReceitaFederalBll unidadeReceitaFederal)
		{
			_unidadeReceitaFederal = unidadeReceitaFederal;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]UnidadeReceitaFederalVM unidadeReceitaFederalVM)
		{
			return _unidadeReceitaFederal.ListarChave(unidadeReceitaFederalVM);
		}
	}
}