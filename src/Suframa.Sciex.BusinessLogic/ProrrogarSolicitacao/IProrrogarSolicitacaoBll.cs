using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IProrrogarSolicitacaoBll
	{
		ResultadoMensagemProcessamentoVM CriarSolicitacao(PRCSolicitacaoAlteracaoVM vm);
		PRCSolicitacaoAlteracaoVM SelecionarSolicitacao(int idProcesso);
		bool ExcluirSolicitacao(int id);
		ResultadoMensagemProcessamentoVM EntregarSolicitacao(PRCSolicitacaoAlteracaoVM vm);
		ProrrogarSolicitacaoVM SalvarProrrogacao(ProrrogarSolicitacaoVM idProcesso);
	}
}