using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ISolicitacaoLeBll
	{
		IEnumerable<SolicitacaoPliVM> Listar(SolicitacaoPliVM solicitacaoPliVM);
		SolicitacaoPliVM SalvarDoArquivo(SolicitacaoPliVM solicitacaoPliVM);
		string LeituraArquivoInserirDados();
		EstruturaPropriaLEEntityVM ListarPaginado(EstruturaPropriaLEEntityVM pagedFilter);
		PagedItems<SolicitacaoLeInsumoVM> ListarPaginadoSolicitacaoInsumos(EstruturaPropriaLEEntityVM pagedFilter);
	}
}