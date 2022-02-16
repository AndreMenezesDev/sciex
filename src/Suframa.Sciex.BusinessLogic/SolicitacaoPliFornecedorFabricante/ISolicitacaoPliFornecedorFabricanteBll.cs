using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ISolicitacaoFornecedorFabricanteBll
	{
		IEnumerable<SolicitacaoFornecedorFabricanteVM> Listar(SolicitacaoFornecedorFabricanteVM solicitacaoFornecedorFabricanteVM);
		SolicitacaoFornecedorFabricanteVM Salvar(SolicitacaoFornecedorFabricanteVM solicitacaoFornecedorFabricanteVM);
	}
}