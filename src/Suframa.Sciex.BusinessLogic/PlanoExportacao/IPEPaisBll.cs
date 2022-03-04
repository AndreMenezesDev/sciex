using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPEPaisBll
	{
		PagedItems<PEProdutoPaisVM> ListarPaginado(PEProdutoVM pagedFilter);
		PagedItems<PEProdutoPaisVM> ListarPaginadoCorrecao(PEProdutoVM pagedFilter);
		PEProdutoPaisVM Selecionar(int id);
		PEProdutoPaisVM Salvar(PEProdutoPaisVM vm);
		void Deletar(int idPEProduto);
	}
}