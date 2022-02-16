using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPEProdutoBll
	{
		PEProdutoVM Selecionar(int idPEProduto);
		
		PEProdutoVM Salvar(PEProdutoVM vm);

		void Deletar(int idPEProduto);
	}
}