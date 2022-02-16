using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class RecintoAlfandegaVM : PagedOptions
	{
		
		public int? Id { get; set; }

		public string Descricao { get; set; }

		public byte Status { get; set; }

		public int Codigo { get; set; }

		public byte[] RowVersion { get; set; }
	}
}
