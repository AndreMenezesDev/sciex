using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPliProdutoBll
	{
		IEnumerable<PliProdutoVM> Listar(PliProdutoVM pliProdutoVM);

		PagedItems<PliProdutoVM> ListarPaginado(PliProdutoVM pagedFilter);

		PliProdutoVM Salvar(PliProdutoVM pliProdutoVM);

		PliProdutoVM Selecionar(int? idPliProduto);

		void Deletar(long id);

		PliProdutoVM AtualizarProduto(PliProdutoVM pliProdutoVM);
	}
}