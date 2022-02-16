using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class TipoCoberturaCambialDropDownController : ApiController
	{
		private readonly ITipoCoberturaCambialBll _tipoCoberturaCambialBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="tipoCoberturaCambial"></param>
		public TipoCoberturaCambialDropDownController(ITipoCoberturaCambialBll tipoCoberturaCambialBll)
		{
			_tipoCoberturaCambialBll = tipoCoberturaCambialBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get()
		{
			return _tipoCoberturaCambialBll.Listar();
		}
	}
}