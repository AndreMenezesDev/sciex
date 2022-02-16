using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class NaladiVM : PagedOptions
	{
		public int? IdNaladi { get; set; }
		public int Codigo { get; set; }
		public string Descricao { get; set; }
		public byte[] RowVersion { get; set; }

		public int? Id { get; set; }
	}
}
