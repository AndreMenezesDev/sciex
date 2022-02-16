using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SOLIC_PLI_DETALHE_MERCADORIA")]
	public partial class SolicitacaoPliDetalheMercadoriaEntity : BaseEntity
	{

		public virtual SolicitacaoPliMercadoriaEntity SolicitacaoPliMercadoria { get; set; }

		[Column("SPM_ID")]
		[ForeignKey(nameof(SolicitacaoPliMercadoria))]
		public long? IdSolicitacaoPliMercadoria { get; set; }

		[Key]
		[Column("SDM_ID")]
		public long IdSolicitacaoDetalheMercadoria { get; set; }

		[Column("SDM_DS_DETALHE")]
		[StringLength(274)]
		public string DescricaoDetalhe { get; set; }

		[Column("SDM_DS_COMPLEMENTO")]
		[StringLength(3783)]
		public string Complemento { get; set; }

		[Column("SDM_DS_PART_NUMBER")]
		[StringLength(20)]
		public string PartNumber { get; set; }

		[Column("SDM_DS_REF_FABRICANTE")]
		[StringLength(20)]
		public string ReferenteFabricante { get; set; }

		[Column("SDM_QT_UNID_COMERCIALIZADA")]
		public decimal? QuantidadeUnidadeComercializada { get; set; }

		[Column("SDM_VL_UNITARIO_COND_VENDA")]
		public decimal? ValorunidadeCondicaoVenda { get; set; }

		[Column("DME_CO_DETALHE_MERCADORIA")]
		public int? CodigoDetalheMercadoria { get; set; }

		[Column("SDM_VL_CONDICAO_VENDA")]
		public decimal? ValorCondicaoVenda { get; set; }

		[Column("UME_DS")]
		[StringLength(40)]
		public string DescricaoUnidadeMedida { get; set; }

		[Column("SDM_DS_MAT_PRIMA_BASICA")]
		[StringLength(20)]
		public string DescricaoMateriaPrima { get; set; }


	}
}