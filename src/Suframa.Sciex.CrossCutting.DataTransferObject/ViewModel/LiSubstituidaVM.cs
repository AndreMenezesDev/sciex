using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class LiSubstituidaVM : PagedOptions
	{
		public long IdPliMercadoria { get; set; }
		public long? NumeroLiSubstituida { get; set; }
	}
}
