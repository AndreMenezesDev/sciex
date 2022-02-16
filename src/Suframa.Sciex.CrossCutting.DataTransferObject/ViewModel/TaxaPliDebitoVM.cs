using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TaxaPliDebitoVM : PagedOptions
	{
		public long IdDebito { get; set; }
		
		public long IdPli { get; set; }
	
		public short? NumeroControleCobrancaTCIF { get; set; }

		public int? NumeroDebito { get; set; }

		public short? AnoDebito { get; set; }

		public DateTime? DataDebitoVencimento { get; set; }

		public DateTime? DataDebitoPagamento { get; set; }

		public DateTime? DataDebitoCancelamento { get; set; }

		public DateTime? DataDebitoFimAcao { get; set; }
	}
}
