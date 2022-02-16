using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ILEInsumoBll
	{
		IEnumerable<LEInsumoVM> Listar(LEInsumoVM vm);

		PagedItems<LEInsumoVM> ListarPaginado(LEInsumoVM pagedFilter);

		LEInsumoVM Salvar(LEInsumoVM vm);

		LEInsumoVM Selecionar(int id);
		LEAnaliseInsumoVM SelecionarInsumoAtualEAnterior(int id);

		void Deletar(long id);
		int DeletarLeInsumoOriginal(long id);

		IEnumerable<object> Listar();

		LEInsumoVM CancelarInsumo(LEInsumoVM vm);
		LEInsumoVM AlterarInsumoBloq(LEInsumoVM vm);
	}
}