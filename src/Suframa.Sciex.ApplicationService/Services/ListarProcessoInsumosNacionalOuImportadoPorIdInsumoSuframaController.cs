using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ListarProcessoInsumosNacionalOuImportadoPorIdInsumoSuframaController : ApiController
	{
		private readonly IProcessoInsumoSuframaBll _bll;

		public ListarProcessoInsumosNacionalOuImportadoPorIdInsumoSuframaController(IProcessoInsumoSuframaBll bll)
		{
			_bll = bll;
		}

		//public PlanoExportacaoVM Get(int id)
		//{
		//	return _planoExportacaoBll.Selecionar(id);
		//}

		public PagedItems<PRCDetalheInsumoVM> Get([FromUri] ListarProcessoInsumosNacionalImportadosVM vm)
		{
			return _bll.ListarProcessoInsumosNacionalOuImportadoPorIdInsumo(vm);
		}		

		//public bool Put([FromBody] PlanoExportacaoVM vm)
		//{
		//	return _planoExportacaoBll.CopiarPlano(vm);
		//}


	}
}