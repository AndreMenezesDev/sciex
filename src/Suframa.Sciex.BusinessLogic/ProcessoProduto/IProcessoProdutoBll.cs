using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IProcessoProdutoBll
	{
		PRCProdutoVM Selecionar(int idProduto);
		PRCSolicitacaoAlteracaoVM Validar (PRCSolicitacaoAlteracaoVM vm);
		PRCProdutoVM SelecionarProdutoEmAnalisePorIdProcesso(int idProcesso);
		
		PEProdutoVM Salvar(PEProdutoVM vm);

		void Deletar(int idPEProduto);
	}
}