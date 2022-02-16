using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class ParametrosDropDownController : ApiController
	{
		private readonly IParametrosBll _parametrosBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="parametro"></param>
		public ParametrosDropDownController(IParametrosBll parametroBll)
		{
			_parametrosBll = parametroBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]ParametrosVM parametrosVM)
		{
			return _parametrosBll.ListarChave(parametrosVM);
		}
	}
}