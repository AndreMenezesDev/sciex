using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class StatusPLIVM : PagedOptions
	{		
		public int? IdStatusPLI { get; set; }
		public string Descricao { get; set; }	
		public bool Checked { get; set; }
	}
}
