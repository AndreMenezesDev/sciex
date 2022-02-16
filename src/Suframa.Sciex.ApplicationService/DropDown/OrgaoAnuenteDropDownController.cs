using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class OrgaoAnuenteDropDownController : ApiController
	{
		private readonly IOrgaoAnuenteBll _orgaoAnuenteBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="orgaoAnuenteBll"></param>
		public OrgaoAnuenteDropDownController(IOrgaoAnuenteBll orgaoAnuenteBll)
		{
			_orgaoAnuenteBll = orgaoAnuenteBll;			
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]OrgaoAnuenteVM orgaoAnuenteVM)
		{
			return _orgaoAnuenteBll.ListarChave(orgaoAnuenteVM);
		}

	}
}