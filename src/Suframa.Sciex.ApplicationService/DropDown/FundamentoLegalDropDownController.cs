using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class FundamentoLegalDropDownController : ApiController
	{
		private readonly IFundamentoLegalBll _fundamentoLegalBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="fundamentoLegal"></param>
		public FundamentoLegalDropDownController(IFundamentoLegalBll fundamentoLegalBll)
		{
			_fundamentoLegalBll = fundamentoLegalBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]FundamentoLegalVM fundamentoLegalVM)
		{
			return _fundamentoLegalBll.ListarChave(fundamentoLegalVM);
		}
	}
}