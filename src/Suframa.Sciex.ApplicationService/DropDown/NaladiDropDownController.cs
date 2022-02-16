using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class NaladiDropDownController : ApiController
	{
		private readonly INaladiBll _naladiBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="aladi"></param>
		public NaladiDropDownController(INaladiBll naladiBll)
		{
			_naladiBll = naladiBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]NaladiVM naladiVM)
		{
			return _naladiBll.ListarChave(naladiVM);
		}
	}
}