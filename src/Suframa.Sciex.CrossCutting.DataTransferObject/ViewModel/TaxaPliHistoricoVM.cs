using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TaxaPliHistoricoVM : PagedOptions
	{		
		public long? IdTaxaPliHistorico { get; set; }	
		public short StatusTaxa { get; set; }		
		public DateTime DataEvento { get; set; }
		public string Observacao { get; set; }
		public long IdPli { get; set; }
		public string RetornoSac { get; set; }
	}
}
