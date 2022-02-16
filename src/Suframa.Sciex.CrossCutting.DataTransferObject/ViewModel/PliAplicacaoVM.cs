using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PliAplicacaoVM : PagedOptions
	{		
		public int? IdPliAplicacao { get; set; }
		public string Descricao { get; set; }
		public short Codigo { get; set; }

	}
}
