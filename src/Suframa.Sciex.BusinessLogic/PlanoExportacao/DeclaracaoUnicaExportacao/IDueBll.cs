using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IDueBll
	{
		PagedItems<PlanoExportacaoDUEComplementoVM> ListarDUEPaginadoParaAnalise(PEProdutoVM pagedFilter);
		ResultadoMensagemProcessamentoVM AprovarTodasDeclaracoesUnicasExportacao(PlanoExportacaoDUEComplementoVM vm);
		ResultadoMensagemProcessamentoVM AprovarOuReprovarDeclaracaoUnicaExportacao(AnalisePlanoExportacaoDUEVM vm);
	}
}