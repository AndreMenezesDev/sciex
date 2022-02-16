using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class CodigoDescricaoInsumoDropDownController : ApiController
	{
		private readonly IProcessoInsumoBll _bll;

		/// <summary>Município injetar regras de negócio</summary>
		/// <param name="viewNcmBll"></param>
		public CodigoDescricaoInsumoDropDownController(IProcessoInsumoBll bll)
		{
			_bll = bll;
		}

		/// <summary>Obter ID e Descrição dos municípios filtrados</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] CodigoDescricaoInsumoDropDownVM view)
		{
			return _bll.ListarChave(view);
		}
	}
}