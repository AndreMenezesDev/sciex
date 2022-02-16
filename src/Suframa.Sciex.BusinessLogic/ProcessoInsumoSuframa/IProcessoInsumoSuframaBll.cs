using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IProcessoInsumoSuframaBll
	{
		PagedItems<PRCInsumoVM> ListarProcessoInsumosNacionalOuImportadoPorIdProcessoProduto(ListarProcessoInsumosNacionalImportadosVM vm);
		PagedItems<PRCDetalheInsumoVM> ListarProcessoInsumosNacionalOuImportadoPorIdInsumo(ListarProcessoInsumosNacionalImportadosVM vm);
		PagedItems<PRCDetalheInsumoVM> ListarDetalhesImportadosPorIdInsumoParaAnalise(ListarProcessoInsumosNacionalImportadosVM vm);
		PRCProdutoVM BuscarInformacoesAdicionaisProduto(PRCProdutoVM vm);
		PRCInsumoVM SelecionarPrcInsumo(int idPEInsumo);
		DadosProcessoDetalhesInsumosVM ListarDetalhePorIdProcessoInsumo(ListarProcessoInsumosNacionalImportadosVM vm);
		PRCInsumoVM AprovarAlteracaoInsumo(PRCInsumoVM vm);
	}
}