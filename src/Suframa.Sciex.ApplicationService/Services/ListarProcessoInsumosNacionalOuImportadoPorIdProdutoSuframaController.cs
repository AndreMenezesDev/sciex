using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ListarProcessoInsumosNacionalOuImportadoPorIdProdutoSuframaController : ApiController
	{
		private readonly IProcessoInsumoSuframaBll _bll;

		public ListarProcessoInsumosNacionalOuImportadoPorIdProdutoSuframaController(IProcessoInsumoSuframaBll bll)
		{
			_bll = bll;
		}

		public PRCInsumoVM Get(int id)
		{
			return _bll.SelecionarPrcInsumo(id);
		}

		public PagedItems<PRCInsumoVM> Get([FromUri] ListarProcessoInsumosNacionalImportadosVM vm)
		{
			return _bll.ListarProcessoInsumosNacionalOuImportadoPorIdProcessoProduto(vm);
		}

		public PRCInsumoVM Put([FromBody] PRCInsumoVM vm)
		{
			return _bll.AprovarAlteracaoInsumo(vm);
		}

		//public bool Put([FromBody] PlanoExportacaoVM vm)
		//{
		//	return _planoExportacaoBll.CopiarPlano(vm);
		//}


	}
}