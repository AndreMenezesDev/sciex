using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DiLiVM : PagedOptions
	{
		public long Id { get; set; }
		public int NumeroLi { get; set; }
		public decimal TipoMultimodal { get; set; }
		public string TipoMultimodalFormatado { get; set; }
		public decimal ValorPesoLiquido { get; set; }
		public decimal ValorFreteMoedaNegociada { get; set; }
		public decimal ValorFreteDolar { get; set; }
		public decimal ValorFrete { get; set; }
		public decimal ValorSeguroMoedaNegociada { get; set; }
		public decimal ValorSeguroDolar { get; set; }
		public decimal ValorSeguro { get; set; }
		public decimal ValorMercadoriaDolar { get; set; }
		public decimal ValorMercadoriaMoedaNegociada { get; set; }
		public string ValorPesoLiquidoFormatado { get; set; }
		public string ValorFreteMoedaNegociadaFormatado { get; set; }
		public string ValorFreteDolarFormatado { get; set; }
		public string ValorFreteFormatado { get; set; }
		public string ValorSeguroMoedaNegociadaFormatado { get; set; }
		public string ValorSeguroDolarFormatado { get; set; }
		public string ValorSeguroFormatado { get; set; }
		public string ValorMercadoriaDolarFormatado { get; set; }
		public string ValorMercadoriaMoedaNegociadaFormatado { get; set; }
		public int CodigoViaTransporte { get; set; }
		public ViaTransporteVM ViaTransporte { get; set; }
		public string ViaTransporteDescricao { get; set; }
		public long IdDi { get; set; }
		public int IdMoedaFrete { get; set; }
		public string MoedaFreteDescricao { get; set; }
		public int IdMoedaSeguro { get; set; }
		public string MoedaSeguroDescricao { get; set; }
		public int IdFundamentoLegal { get; set; }
		public FundamentoLegalVM FundamentoLegal { get; set; }
		public string FundamentoLegalDescricao { get; set; }
	}
}