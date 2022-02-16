using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ListarInsumosNacionalOuImportadoPorIdProdutoParaCorrecaoController : ApiController
	{
		private readonly IPEInsumoBll _bll;

		public ListarInsumosNacionalOuImportadoPorIdProdutoParaCorrecaoController(IPEInsumoBll bll)
		{
			_bll = bll;
		}

		//public PlanoExportacaoVM Get(int id)
		//{
		//	return _planoExportacaoBll.Selecionar(id);
		//}

		public PagedItems<PEInsumoVM> Get([FromUri] ListarInsumosNacionalImportadosVM vm)
		{
			return _bll.ListarInsumosNacionalImportadosPorIdProdutoParaCorrecao(vm);
		}		

		//public bool Put([FromBody] PlanoExportacaoVM vm)
		//{
		//	return _planoExportacaoBll.CopiarPlano(vm);
		//}


	}
}