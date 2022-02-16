using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	/// <summary>HistoricoSituacaoInscricaoGridController</summary>
	public class HistoricoSituacaoInscricaoGridController : ApiController
	{
		private readonly IHistoricoSituacaoInscricaoBll _historicoSituacaoInscricaoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="historicoSituacaoInscricaoBll"></param>
		public HistoricoSituacaoInscricaoGridController(IHistoricoSituacaoInscricaoBll historicoSituacaoInscricaoBll)
		{
			_historicoSituacaoInscricaoBll = historicoSituacaoInscricaoBll;
		}

		/// <summary>Get</summary>
		/// <param name="historicoSituacaoInscricaoVM"></param>
		/// <returns></returns>
		public PagedItems<HistoricoSituacaoInscricaoVM> Get([FromUri]HistoricoSituacaoInscricaoVM historicoSituacaoInscricaoVM)
		{
			return _historicoSituacaoInscricaoBll.ListarPaginado(historicoSituacaoInscricaoVM);
		}
	}
}