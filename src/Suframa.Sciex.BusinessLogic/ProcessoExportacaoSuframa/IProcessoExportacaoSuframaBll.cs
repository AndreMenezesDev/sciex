using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IProcessoExportacaoSuframaBll
	{
		PagedItems<ProcessoExportacaoVM> ListarPaginado(ConsultarProcessoExportacaoVM pagedFilter);
		ProcessoExportacaoVM Selecionar(int idProcesso);

		bool DeletarPlano(int idPlanoExportacao);
		NovoPlanoExportacaoVM SalvarNovoPlano(NovoPlanoExportacaoVM vm);
		bool CopiarPlano(PlanoExportacaoVM vm);
		ResultadoProcessamentoVM EntregarPlano(PlanoExportacaoVM vm);
		ResultadoProcessamentoVM ValidarPlano(int idPlanoExportacao, ResultadoProcessamentoVM retorno);
		PlanoExportacaoVM SalvarAnexo(PlanoExportacaoVM vm);
	}
}