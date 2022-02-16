using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPEInsumoBll
	{
		PagedItems<PEInsumoVM> ListarInsumosNacionalImportadosPorIdProduto(ListarInsumosNacionalImportadosVM vm);
		PagedItems<PEInsumoVM> ListarInsumosNacionalImportadosPorIdProdutoParaCorrecao(ListarInsumosNacionalImportadosVM vm);
		PagedItems<PEInsumoVM> ListarInsumosNacionalImportadosPorIdProdutoAnalise(ListarInsumosNacionalImportadosVM vm);
		PagedItems<LEInsumoVM> ListarInsumosPorCodigoPENacionalOuImportado(ListarInsumosNacionalImportadosVM vm);
		bool AdicionarInsumoAoProduto(LEInsumoVM vm);
		bool Deletar(int idPEInsumo);
		PEInsumoVM SelecionarInsumoAnteriorPorIdInsumoAtual(int idPEInsumo);
		bool AtualizarInsumo(PEInsumoVM vm);
		string FormatarQtdMax(PEInsumoVM vm);
		ResultadoMensagemProcessamentoVM AprovarTodosInsumosEDetalhes(ListarInsumosNacionalImportadosVM vm);
		ResultadoMensagemProcessamentoVM InativarInsumo(PEInsumoVM vm);
		ResultadoMensagemProcessamentoVM CorrigirDadosInsumo(PEInsumoVM vm);
		PEInsumoVM SelecionarPEInsumo(int idPEInsumo);

	}
}