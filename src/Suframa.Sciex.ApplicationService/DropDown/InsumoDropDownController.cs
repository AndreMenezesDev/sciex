using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown dos Insumos</summary>
	public class InsumoDropDownController : ApiController
    {

		private readonly IInsumoBll _insumoBll;

		/// <summary>Países injetar regras de negócio</summary>
		/// <param name="insumoBll"></param>
		public InsumoDropDownController(IInsumoBll insumoBll)
		{
			_insumoBll = insumoBll;
		}

		/// <summary>Obter ID e Descrição dos Insumos</summary>
		/// <returns></returns>
		// GET: api/InsumoDropDown/5
		public IEnumerable<object> Get([FromUri] InsumoVM insumoVM)
        {
			return _insumoBll.PesquisarInsumo(insumoVM);
		}
    }
}
