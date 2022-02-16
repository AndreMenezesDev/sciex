using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class CorrigirInsumoController : ApiController
	{
		private readonly IPEInsumoBll _bll;

		public CorrigirInsumoController(IPEInsumoBll bll)
		{
			_bll = bll;
		}

		//public PlanoExportacaoVM Get(int id)
		//{
		//	return _planoExportacaoBll.Selecionar(id);
		//}

		//public PagedItems<PlanoExportacaoVM> Get([FromUri] ConsultarPlanoExportacaoVM vm)
		//{
		//	return _planoExportacaoBll.ListarPaginado(vm);
		//}

		//public NovoPlanoExportacaoVM Post([FromBody] NovoPlanoExportacaoVM vm)
		//{
		//	return _planoExportacaoBll.SalvarNovoPlano(vm);
		//}

		//public ResultadoMensagemProcessamentoVM Put([FromBody] PlanoExportacaoVM vm)
		//{
		//	return _bll.SolicitarCorrecaoInsumo(vm);
		//}

		public ResultadoMensagemProcessamentoVM Put([FromBody] PEInsumoVM vm)
		{
			return _bll.InativarInsumo(vm);
		}
	}
}