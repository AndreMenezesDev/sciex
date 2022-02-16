using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class CorrigirPlanoExportacaoController : ApiController
	{
		private readonly IPlanoExportacaoBll _planoExportacaoBll;

		public CorrigirPlanoExportacaoController(IPlanoExportacaoBll planoExportacaoBll)
		{
			_planoExportacaoBll = planoExportacaoBll;
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

		public ResultadoMensagemProcessamentoVM Put([FromBody] PlanoExportacaoVM vm)
		{
			return _planoExportacaoBll.SolicitarCorrecaoPlanoExportacao(vm);
		}

		public ResultadoMensagemProcessamentoVM Delete(int id)
		{
			return _planoExportacaoBll.DeletarCorrecaoPlanoExportacao(id);
		}
	}
}