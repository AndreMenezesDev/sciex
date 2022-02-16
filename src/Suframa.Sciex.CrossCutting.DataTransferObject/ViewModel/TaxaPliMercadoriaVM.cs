using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TaxaPliMercadoriaVM : PagedOptions
	{
		public long IdPliMercadoria { get; set; }
		public int? IdFundamentoLegal { get; set; }
		public int? IdRegimeTributario { get; set; }
		public long? IdPli { get; set; }
		public int? Isencao { get; set; }
		public int? Reducao { get; set; }
		public int? IdMoedaNegociada { get; set; }
		public decimal? ValorPercentualReducao { get; set; }
		public decimal? ValorMercadoriaMoedaNegociada { get; set; }
		public decimal? ValorMercadoriaReais { get; set; }
		public short? QtdItens { get; set; }
		public DateTime? DataCadastro { get; set; }
		public DateTime? DataParidade { get; set; }
		public string CodigoNCM { get; set; }
	}
}
