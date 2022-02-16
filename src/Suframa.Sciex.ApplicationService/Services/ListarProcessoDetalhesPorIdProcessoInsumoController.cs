using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ListarProcessoDetalhesPorIdProcessoInsumoController : ApiController
	{
		private readonly IProcessoInsumoBll _bll;

		public ListarProcessoDetalhesPorIdProcessoInsumoController(IProcessoInsumoBll bll)
		{
			_bll = bll;
		}

		//public PlanoExportacaoVM Get(int id)
		//{
		//	return _planoExportacaoBll.Selecionar(id);
		//}

		public DadosProcessoDetalhesInsumosVM Get([FromUri] ListarProcessoInsumosNacionalImportadosVM vm)
		{
			return _bll.ListarDetalhePorIdProcessoInsumo(vm);
		}		

		//public bool Put([FromBody] PlanoExportacaoVM vm)
		//{
		//	return _planoExportacaoBll.CopiarPlano(vm);
		//}


	}
}