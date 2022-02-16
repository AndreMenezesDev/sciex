using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ListarInsumosNacionalOuImportadoPorCodigoProdutoController : ApiController
	{
		private readonly IPEInsumoBll _peInsumoBll;

		public ListarInsumosNacionalOuImportadoPorCodigoProdutoController(IPEInsumoBll peInsumoBll)
		{
			_peInsumoBll = peInsumoBll;
		}

		//public PlanoExportacaoVM Get(int id)
		//{
		//	return _planoExportacaoBll.Selecionar(id);
		//}

		public PagedItems<LEInsumoVM> Get([FromUri] ListarInsumosNacionalImportadosVM vm)
		{
			return _peInsumoBll.ListarInsumosPorCodigoPENacionalOuImportado(vm);
		}

		public bool Post([FromBody] LEInsumoVM vm)
		{
			return _peInsumoBll.AdicionarInsumoAoProduto(vm);
		}

		//public bool Put([FromBody] PlanoExportacaoVM vm)
		//{
		//	return _planoExportacaoBll.CopiarPlano(vm);
		//}


	}
}