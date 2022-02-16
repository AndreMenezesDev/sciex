using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class ControleImportacaoDropDownController : ApiController
	{
		private readonly IControleImportacaoBll _controleImportacaoBll;
		private readonly ICodigoContaBll _codigoContaBll;
		private readonly ICodigoUtilizacaoBll _codigoUtilizacaoBll;

		/// <summary>Município injetar regras de negócio</summary>
		/// <param name="controleImportacaoBll"></param>
		public ControleImportacaoDropDownController(IControleImportacaoBll controleImportacaoBll, ICodigoContaBll codigoContaBll, ICodigoUtilizacaoBll codigoUtilizacaoBll)
		{
			_controleImportacaoBll = controleImportacaoBll;
			_codigoContaBll = codigoContaBll;
			_codigoUtilizacaoBll = codigoUtilizacaoBll;
		}

		/// <summary>Obter ID e Descrição dos Código Conta</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] CodigoContaVM codigoContaVM)
		{
			return _codigoContaBll.ListarChave(codigoContaVM);
		}

		/// <summary>Obter ID e Descrição dos Código Utilização</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] CodigoUtilizacaoVM codigoUtilizacaoVM)
		{
			return _codigoUtilizacaoBll.ListarChave(codigoUtilizacaoVM);
		}
	}
}