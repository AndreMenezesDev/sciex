using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;

namespace Suframa.Sciex.BusinessLogic
{
	public class HistoricoSituacaoInscricaoBll : IHistoricoSituacaoInscricaoBll
	{
		private readonly IUnitOfWork _uow;

		public HistoricoSituacaoInscricaoBll(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public PagedItems<HistoricoSituacaoInscricaoVM> ListarPaginado(HistoricoSituacaoInscricaoVM historicoSituacaoInscricaoVM)
		{
			if (historicoSituacaoInscricaoVM.DataHoraFim.HasValue)
			{
				historicoSituacaoInscricaoVM.DataHoraFim = historicoSituacaoInscricaoVM.DataHoraFim.Value.AddDays(1).AddMilliseconds(-1);
			}

			var lista = _uow.QueryStack.HistoricoSituacaoInscricao.ListarPaginado<HistoricoSituacaoInscricaoVM>(x =>
				(
					(x.WorkflowSituacaoInscricao.IdInscricaoCadastral == historicoSituacaoInscricaoVM.IdInscricaoCadastral) &&
					((!historicoSituacaoInscricaoVM.DataHoraInicio.HasValue && !historicoSituacaoInscricaoVM.DataHoraFim.HasValue) ||
					(x.DataHoraFim >= historicoSituacaoInscricaoVM.DataHoraInicio && x.DataHoraFim <= historicoSituacaoInscricaoVM.DataHoraFim))
				)
			, historicoSituacaoInscricaoVM);

			return lista;
		}
	}
}