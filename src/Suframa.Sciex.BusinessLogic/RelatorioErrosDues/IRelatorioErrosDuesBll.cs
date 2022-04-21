using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IRelatorioErrosDuesBll
	{
		List<RelatorioErrosDuesVM> GerarRelatorio(RelatorioErrosDuesVM filterVm);
	}
}
