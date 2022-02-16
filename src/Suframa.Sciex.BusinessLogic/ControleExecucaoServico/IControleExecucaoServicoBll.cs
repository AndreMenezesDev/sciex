using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IControleExecucaoServicoBll
	{
		IEnumerable<ControleExecucaoServicoVM> Listar(ControleExecucaoServicoVM controleExecucaoServicoVM);

		PagedItems<ControleExecucaoServicoVM> ListarPaginado(ControleExecucaoServicoVM pagedFilter);

		void Salvar(ControleExecucaoServicoVM controleExecucaoServicoVM);

		ControleExecucaoServicoVM Selecionar(int? idControleExecucaoServico);

		void Deletar(int id);

	}
}