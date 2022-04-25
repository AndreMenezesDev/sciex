using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class RelatoriosAnalisadorListaDuesVM : PagedOptions
	{
		public List<RelatorioAnalisadorDuesVM> RelatoriosAnaliseDue { get; set; }

	}


}