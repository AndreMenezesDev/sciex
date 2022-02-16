using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PliHistoricoVM : PagedOptions
	{

		public int? IdPliHistorico { get; set; }
		public DateTime? DataEvento { get; set; }
		public long? IdPLI { get; set; }
		public string CPFCNPJ { get; set; }
		public string NomeResponsavel { get; set; }
		public string Observacao { get; set; }
		public byte? StatusPli { get; set; }
		public string DescricaoStatusPli { get; set; }

	}
}
