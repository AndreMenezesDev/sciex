using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TaxaPliVM : PagedOptions
	{		
		public long IdPli { get; set; }
		public int IdTaxaFatoGerador { get; set; }
		public short IdTaxaServicoSac { get; set; }
		public decimal ValorBaseFatoGerador { get; set; }
		public decimal ValorPercentualLimitadorPli { get; set; }
		public DateTime DataEnvioSac { get; set; }
		public int CodigoLocalidade { get; set; }
		public int CodigoMunicipio { get; set; }
		public int NumeroProcessoPEXPAM { get; set; }
		public short AnoProcessoPEXPAM { get; set; }		
		public decimal ValorTotalMercadoriaReais { get; set; }		
		public decimal ValorTotalCalculadoLimitadorPLI { get; set; }		
		public decimal ValorPrevalenciaPLI { get; set; }		
		public decimal ValorPercentualReducaoPLI { get; set; }
		public decimal ValorReducaoPLI { get; set; }
		public decimal ValorTCIFPLI { get; set; }
		public decimal ValorReducaoBasePLI { get; set; }
		public decimal ValorTotalReducaoItens { get; set; }
		public decimal ValorTotalReducaoBaseItens { get; set; }
		public decimal ValorGeralReducaoTCIF { get; set; }
		public decimal ValorGeralTCIF { get; set; }
		public decimal ValorGeralReducaoBase { get; set; }
		public short NumeroControleCobrancaTCIF { get; set; }
		public string CNPJ { get; set; }
		public short NumeroServicoSAC { get; set; }
		public int IdPliAplicacao { get; set; }
		public int Isencao { get; set; }
		public int Reducao { get; set; }
		public decimal ValorTotalTCIFItens { get; set; }
		public int NumeroDebito { get; set; }
		public short AnoDebito { get; set; }
		public DateTime DataDebitoVencimento { get; set; }
		public DateTime DataDebitoPagamento { get; set; }
		public DateTime DataDebitoCancelamento { get; set; }
		public DateTime DataCadastro { get; set; }
		public int StatusTaxa { get; set; }
		public DateTime DataEnvioPLI { get; set; }
		//public List<TaxaPliDebitoVM> TaxaPliDebito { get; set; }
	}
}
