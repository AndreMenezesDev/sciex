using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IRecintoAlfandegaBll
	{
		PagedItems<RecintoAlfandegaVM> ListarPaginado(RecintoAlfandegaVM pagedFilter);
		RecintoAlfandegaVM VerificaCodigoCadastrado(RecintoAlfandegaVM pagedFilter);
		RecintoAlfandegaVM SelecionarRecintoAlfandega(int id);
		int Salvar(RecintoAlfandegaVM objeto);
		int AtualizarStatus(RecintoAlfandegaVM objeto);
	}
}
