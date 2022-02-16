using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TaxaServicoSacVM : PagedOptions
	{
		public short? IdTaxaServicoSac { get; set; }
		public string Descricao { get; set; }
		public DateTime DataCadastro { get; set; }
		public short NumeroServicoSac { get; set; }
	}
}
