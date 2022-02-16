using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PliDetalheMercadoriaVM : PagedOptions
	{
		public long? IdPliDetalheMercadoria { get; set; }	
		public long? IdPliMercadoria { get; set; }
		public int IdUnidadeMedida { get; set; }
		public string DescricaoUnidadeMedida { get; set; }
		public string SiglaUnidadeMedida { get; set; }
		public int? CodigoDetalheMercadoria { get; set; }
		public string DescricaoDetalhe { get; set; }
		public string DescricaoComplemento { get; set; }
		public string DescricaoMateriaPrimaBasica { get; set; }
		public string DescricaoPartNumber { get; set; }
		public string DescricaoREFFabricante { get; set; }
		public decimal? QuantidadeComercializada { get; set; }
		public decimal? ValorUnitarioCondicaoVenda { get; set; }
		public decimal? ValorCondicaoVenda { get; set; }
		public decimal? ValorTotalCondicaoVendaReal { get; set; }
		public decimal? ValorTotalCondicaoVendaDolar { get; set; }
		public decimal? ValorUnitarioCondicaoVendaDolar { get; set; }

		//complemento de classe
		public string MensagemErro { get; set; }
		public bool Excluir { get; set; }
		public string QuantidadeComercializadaFormatada { get; set; }
		public string ValorUnitarioCondicaoVendaFormatada { get; set; }
		public string ValorTotalCondicaoVendaDolarFormatada { get; set; }
		public long IdPli { get; set; }
		public string ValorUnitarioCondicaoVendaDolarFormatada { get; set; }
		public string CodigoDetalheMercadoriaFormatado { get; set; }

	}
}
