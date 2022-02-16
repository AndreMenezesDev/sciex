using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ISetorArmazenamentoBll
	{
		IEnumerable<object> ListarRecintoAlfandega();
		PagedItems<SetorArmazenamentoVM> ListarPaginado(SetorArmazenamentoVM pagedFilter);
		SetorArmazenamentoVM VerificaCodigoCadastrado(SetorArmazenamentoVM pagedFilter);
		SetorArmazenamentoVM SelecionarArmazenamento(int id);
		int Salvar(SetorArmazenamentoVM objeto);
		int AtualizarStatus(SetorArmazenamentoVM objeto);
	}
}
