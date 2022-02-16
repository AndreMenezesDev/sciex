using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AliSubstituidaVM : PagedOptions
	{
		public long IdPliMercadoria { get; set; }
		public long NumeroAliSubstituida { get; set; }
	}
}
