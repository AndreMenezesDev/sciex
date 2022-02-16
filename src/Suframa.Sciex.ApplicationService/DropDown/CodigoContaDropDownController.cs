using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class CodigoContaDropDownController : ApiController
	{
		private readonly ICodigoContaBll _codigoContaBll;

		/// <summary>Município injetar regras de negócio</summary>
		/// <param name="codigoContaBll"></param>
		public CodigoContaDropDownController(ICodigoContaBll codigoContaBll)
		{
			_codigoContaBll = codigoContaBll;
		}

		/// <summary>Obter ID e Descrição dos municípios filtrados</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] CodigoContaVM codigoContaVM)
		{
			return _codigoContaBll.ListarChave(codigoContaVM);
		}
	}
}