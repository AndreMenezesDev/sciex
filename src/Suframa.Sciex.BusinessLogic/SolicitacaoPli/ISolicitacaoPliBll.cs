using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ISolicitacaoPliBll
	{
		IEnumerable<SolicitacaoPliVM> Listar(SolicitacaoPliVM solicitacaoPliVM);
		SolicitacaoPliVM SalvarDoArquivo(SolicitacaoPliVM solicitacaoPliVM);
		string LeituraArquivoInserirDados();
		PagedItems<SolicitacaoPliVM> ListarPaginado(SolicitacaoPliVM pagedFilter);
	}
}