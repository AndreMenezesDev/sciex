using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IServicoBll
	{
		IEnumerable<ServicoVM> Listar(ServicoVM servicoVM);

		void Salvar(IEnumerable<ServicoVM> servicos);
	}
}