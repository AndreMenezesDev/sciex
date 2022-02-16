using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TipoEmbalagemVM : PagedOptions
	{
		
		public int? Id { get; set; }

		public string Descricao { get; set; }

		public byte Status { get; set; }

		public short Codigo { get; set; }

		public byte[] RowVersion { get; set; }

	}
}
