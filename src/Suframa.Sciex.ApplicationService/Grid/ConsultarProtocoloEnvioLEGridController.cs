using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ConsultarProtocoloEnvioLEGridController : ApiController
    {
		private readonly ISolicitacaoLeBll _solicitacaoLeBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="consultarLeBll"></param>
		public ConsultarProtocoloEnvioLEGridController(ISolicitacaoLeBll solicitacaoLeBll)
		{
			_solicitacaoLeBll = solicitacaoLeBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<SolicitacaoLeInsumoVM> Get([FromUri] EstruturaPropriaLEEntityVM pagedFilter)
		{
			return _solicitacaoLeBll.ListarPaginadoSolicitacaoInsumos(pagedFilter);
		}
	}
}
