using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ListarProcessoInsumosNacionalOuImportadoPorIdProdutoController : ApiController
	{
		private readonly IProcessoInsumoBll _bll;
		//ListarProcessoInsumosNacionalOuImportadoPorIdProduto
		public ListarProcessoInsumosNacionalOuImportadoPorIdProdutoController(IProcessoInsumoBll bll)
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

		//public bool Put([FromBody] PlanoExportacaoVM vm)
		//{
		//	return _planoExportacaoBll.CopiarPlano(vm);
		//}


	}
}