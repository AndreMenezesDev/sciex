using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DiArmazemVM : PagedOptions
	{
		public long Id { get; set; }
		public string Descricao { get; set; }
		public long IdDi { get; set; }
	}
}
