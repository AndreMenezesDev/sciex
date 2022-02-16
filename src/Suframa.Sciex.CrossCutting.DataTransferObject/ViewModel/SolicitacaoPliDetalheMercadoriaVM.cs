using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class SolicitacaoPliDetalheMercadoriaVM
	{
		public long? IdSolicitacaoPliMercadoria { get; set; }
		public long IdSolicitacaoDetalheMercadoria { get; set; }
		public string DescricaoDetalhe { get; set; }
		public string Complemento { get; set; }
		public string PartNumber { get; set; }
		public string ReferenteFabricante { get; set; }
		public decimal? QuantidadeUnidadeComercializada { get; set; }
		public decimal? ValorunidadeCondicaoVenda { get; set; }
		public int? CodigoDetalheMercadoria { get; set; }
		public decimal? ValorCondicaoVenda { get; set; }
		public string DescricaoUnidadeMedida { get; set; }
		public string DescricaoMateriaPrima { get; set; }
	}
}
