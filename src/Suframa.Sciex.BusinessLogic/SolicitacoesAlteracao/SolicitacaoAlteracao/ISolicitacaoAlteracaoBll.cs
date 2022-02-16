using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ISolicitacaoAlteracaoBll
	{
		List<PRCSolicDetalheVM> BuscarDados(SolicitacoesAlteracaoVM objeto);

		IEnumerable<object> ListarChave(PRCSolicitacaoAlteracaoVM id);
	}
}
