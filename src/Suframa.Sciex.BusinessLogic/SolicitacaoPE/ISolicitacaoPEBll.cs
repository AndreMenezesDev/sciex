using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ISolicitacaoPEBll
	{
		IEnumerable<SolicitacaoPliVM> Listar(SolicitacaoPliVM solicitacaoPliVM);
		SolicitacaoPliVM SalvarDoArquivo(SolicitacaoPliVM solicitacaoPliVM);
		string LeituraArquivoInserirDados();
		EstruturaPropriaPEVM SelecionarEstruturaPropriaPE(EstruturaPropriaPEVM pagedFilter);
		PagedItems<SolicitacaoPEProdutoVM> ListarPaginadoSolicitacaoPE(EstruturaPropriaPEVM pagedFilter);
	}
}