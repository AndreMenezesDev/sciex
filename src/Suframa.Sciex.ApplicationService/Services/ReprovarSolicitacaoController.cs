using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class ReprovarSolicitacaoController : ApiController
	{
		private readonly IAnaliseInsumoImportadoBll _analiseInsumoImportadoBll;

		public ReprovarSolicitacaoController(IAnaliseInsumoImportadoBll analiseInsumoImportadoBll)
		{
			_analiseInsumoImportadoBll = analiseInsumoImportadoBll;
		}

		public ResultadoMensagemProcessamentoVM Get([FromUri]PRCSolicDetalheVM filtro)
		{
			var reprovarSolicitacao = _analiseInsumoImportadoBll.ReprovarInsumo(filtro);
			return reprovarSolicitacao;
		}

	}
}