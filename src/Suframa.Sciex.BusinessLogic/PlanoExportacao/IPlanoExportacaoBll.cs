using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPlanoExportacaoBll
	{
		PagedItems<PlanoExportacaoVM> ListarPaginado(ConsultarPlanoExportacaoVM pagedFilter);
		PagedItems<PlanoExportacaoDUEComplementoVM> ListarDUEPaginado(PEProdutoVM pagedFilter);
		PlanoExportacaoVM Selecionar(int idPlanoExportacao);
		bool DeletarPlano(int idPlanoExportacao);
		bool DeletarDUE(int idDue);
		NovoPlanoExportacaoVM SalvarNovoPlano(NovoPlanoExportacaoVM vm);
		bool CopiarPlano(PlanoExportacaoVM vm);
		ResultadoProcessamentoVM EntregarPlano(PlanoExportacaoVM vm);
		ResultadoProcessamentoVM EntregarPlanoComprovacao(int idPlanoExportacao, ResultadoProcessamentoVM retorno);
		ResultadoProcessamentoVM ValidarPlano(int idPlanoExportacao, ResultadoProcessamentoVM retorno);
	    ResultadoProcessamentoVM ValidarPlanoExportacaoComprovacao(int idPlanoExportacao, ResultadoProcessamentoVM retorno);
		PlanoExportacaoVM SalvarAnexo(PlanoExportacaoVM vm);
		ResultadoMensagemProcessamentoVM SolicitarCorrecaoPlanoExportacao(PlanoExportacaoVM vm);
		ResultadoMensagemProcessamentoVM DeletarCorrecaoPlanoExportacao(int id);
		DuePorProdutoVM SalvarDocumentosComprobatorios(DuePorProdutoVM vm);
		DuePorProdutoVM EditarDocumentosCombprobatorios(DuePorProdutoVM vm);
		PagedItems<PlanoExportacaoDUEComplementoVM> ListarDUEPaginadoParaAnalise(PEProdutoVM pagedFilter);
	}
}