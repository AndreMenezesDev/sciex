using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ILEConsultarInsumoBll
	{
		IEnumerable<LEInsumoVM> Listar(LEInsumoVM vm);

		PagedItems<LEInsumoVM> ListarPaginado(LEInsumoVM pagedFilter);

		LEInsumoVM Salvar(LEInsumoVM vm);

		LEInsumoVM Selecionar(int id);

		void Deletar(long id);

		IEnumerable<object> Listar();
	}
}