using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class MotivoVM : PagedOptions
	{
		public int? IdMotivo { get; set; }
		public string Codigo { get; set; }
		public string Descricao { get; set; }

		public int? Id { get; set; }
	}
}