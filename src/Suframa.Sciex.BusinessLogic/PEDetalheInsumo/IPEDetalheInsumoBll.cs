using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPEDetalheInsumoBll
	{
		DadosDetalhesInsumosVM ListarDetalhePorIdInsumo(SalvarDetalheVM vm);
		PagedItems<PEDetalheInsumoImportadoVM> ListarPaginadoDetalhePorIdInsumo(SalvarDetalheVM vm);
		PagedItems<PEDetalheInsumoImportadoVM> ListarPaginadoDetalhePorIdInsumoParaAnalise(SalvarDetalheVM vm);
		PagedItems<PEDetalheInsumoImportadoVM> ListarPaginadoDetalheAnterioresPorIdInsumoParaAnalise(SalvarDetalheVM vm);
		int SalvarNovoDetalhe(SalvarDetalheVM vm);
		int AtualizarDetalhe(SalvarDetalheVM vm);
		ResultadoCorrigirDetalheInsumoVM CorrigirDetalhe(SalvarDetalheVM vm);
		bool Deletar(int idPEDetalheInsumo);
		ResultadoMensagemProcessamentoVM InativarDetalheInsumo(PEDetalheInsumoVM vm);

	}
}