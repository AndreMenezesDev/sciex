using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ILEProdutoBll
	{
		IEnumerable<LEProdutoVM> Listar(LEProdutoVM vm);

		PagedItems<LEProdutoVM> ListarPaginado(LEProdutoVM pagedFilter);

		LEProdutoVM Salvar(LEProdutoVM vm);
		LEProdutoVM Entregar(LEProdutoVM vm);
		LEProdutoVM Selecionar(int id);
		IEnumerable<object> ListarChave(LEProdutoVM vm);
		void Deletar(long id);
		IEnumerable<object> Listar();

		LEProdutoVM SolicitarAlteracaoLE(LEProdutoVM vm);
	}
}