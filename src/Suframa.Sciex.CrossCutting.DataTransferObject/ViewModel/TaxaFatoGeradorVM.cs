using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TaxaFatoGeradorVM : PagedOptions
	{
		public int? IdTaxaFatoGerador { get; set; }
		public string Descricao { get; set; }
		public decimal Valor { get; set; }
		public decimal ValorPercentualLimitador { get; set; }
		public DateTime DataCadastro { get; set; }
		public short Codigo { get; set; }
	}
}
