using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ILEAnaliseProdutoBll
	{
		IEnumerable<LEProdutoVM> Listar(LEProdutoVM vm);

		PagedItems<LEProdutoVM> ListarPaginado(LEProdutoVM pagedFilter);

		LEInsumoVM SalvarAnalise(LEInsumoVM vm);
		LEProdutoVM AprovarAnalise(LEProdutoVM vm);
		LEProdutoVM Entregar(LEProdutoVM vm);
		LEProdutoVM Selecionar(int id);

		void Deletar(long id);

		IEnumerable<object> Listar();
	}
}