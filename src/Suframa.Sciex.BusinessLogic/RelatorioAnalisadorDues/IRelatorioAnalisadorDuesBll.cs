using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IRelatorioAnalisadorDuesBll
	{

		//List<RelatorioAnalisadorDuesVM> GerarRelatorio(RelatorioAnalisadorDuesVM RelatorioAnalisadorDuesVM);
		List<RelatoriosAnalisadorListaDuesVM> GetInfoRelatorio(RelatorioAnalisadorDuesVM filterVm);


	}
}