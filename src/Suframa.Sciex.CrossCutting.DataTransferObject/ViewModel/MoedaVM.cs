using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class MoedaVM : PagedOptions
	{		

		public int? IdMoeda { get; set; }		
		public short CodigoMoeda { get; set; }
		public string Descricao { get; set; }
		public string Sigla { get; set; }
		public int? Id { get; set; }
	}
}
