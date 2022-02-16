using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TaxaPliDetalheMercadoriaVM : PagedOptions
	{
		public long IdPliDetalheMercadoria { get; set; }
		public int? IdTaxaFatoGerador { get; set; }
		public long? IdPliMercadoria { get; set; }
		public int? Isencao { get; set; }
		public int? Reducao { get; set; }
		public string DescricaoDetalhe { get; set; }		
		public decimal? ValorBaseFatoGeradorItem { get; set; }	
		public decimal? ValorPercentualLimitadorItem { get; set; }
		public decimal? ValorPercentualReducaoItem { get; set; }	
		public decimal? QtdUnidadeComercializada { get; set; }
		public decimal? ValorUnidadeCondicaoVenda { get; set; }
		public decimal? ValorUnidadeReais { get; set; }
		public decimal? ValorCalculadoLimitadorItem { get; set; }		
		public decimal? ValorPrevalenciaItem { get; set; }		
		public decimal? ValorReducaoItem { get; set; }		
		public decimal? ValorTCIFItem { get; set; }
		public DateTime? DataCadastro { get; set; }		
		public decimal? ValorReducaoBaseItem { get; set; }
	}
}
