using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPliHistoricoBll
	{
		IEnumerable<PliHistoricoVM> Listar(PliHistoricoVM pliHistoricoVM);

		PagedItems<PliHistoricoVM> ListarPaginado(PliHistoricoVM pagedFilter);

		void Salvar(PliHistoricoVM pliHistoricoVM);

		PliHistoricoVM Selecionar(int? idPliHistorico);

		void Deletar(int id);

	}
}