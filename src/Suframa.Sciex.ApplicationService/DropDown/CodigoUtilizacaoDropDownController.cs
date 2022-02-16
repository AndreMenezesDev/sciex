using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class CodigoUtilizacaoDropDownController : ApiController
	{
		private readonly ICodigoUtilizacaoBll _codigoUtilizacaoBll;

		/// <summary>Município injetar regras de negócio</summary>
		/// <param name="setorBll"></param>
		public CodigoUtilizacaoDropDownController(ICodigoUtilizacaoBll codigoUtilizacaoBll)
		{
			_codigoUtilizacaoBll = codigoUtilizacaoBll;
		}

		/// <summary>Obter ID e Descrição dos municípios filtrados</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] CodigoUtilizacaoVM codigoUtilizacaoVM)
		{
			return _codigoUtilizacaoBll.ListarChave(codigoUtilizacaoVM);
		}
	}
}