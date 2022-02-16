using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IListaServicoBll
	{
		IEnumerable<ListaServicoVM> Listar(ListaServicoVM listaServicoVM);

		PagedItems<ListaServicoVM> ListarPaginado(ListaServicoVM pagedFilter);

		void Salvar(ListaServicoVM listaServicoVM);

		ListaServicoVM Selecionar(int? idAladi);

		void Deletar(int id);

	}
}