using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PLI_DETALHE_MERCADORIA")]
	public partial class PliDetalheMercadoriaEntity : BaseEntity
	{

		public virtual PliMercadoriaEntity PliMercadoria { get; set; }
		public virtual TaxaPliDetalheMercadoriaEntity TaxaPliDetalheMercadoria { get; set; }


		[Key]
		[Column("PDM_ID")]
		public long IdPliDetalheMercadoria { get; set; }

		[Required]
		[Column("PME_ID")]
		[ForeignKey(nameof(PliMercadoria))]
		public long? IdPliMercadoria { get; set; }

		[Column("UME_CO")]	
		public int? IdUnidadeMedida { get; set; }

		[StringLength(40)]
		[Column("UME_DS")]
		public string DescricaoUnidadeMedida { get; set; }

		[StringLength(5)]
		[Column("UME_SG")]
		public string SiglaUnidadeMedida { get; set; }

		[Column("DME_CO_DETALHE_MERCADORIA")]
		public int? CodigoDetalheMercadoria { get; set; }

		[StringLength(254)]
		[Column("PDM_DS_DETALHE")]
		public string DescricaoDetalhe { get; set; }

		[StringLength(3783)]
		[Column("PDM_DS_COMPLEMENTO")]
		public string DescricaoComplemento { get; set; }

		[StringLength(20)]
		[Column("PDM_DS_MAT_PRIMA_BASICA")]
		public string DescricaoMateriaPrimaBasica { get; set; }

		[StringLength(20)]
		[Column("PDM_DS_PART_NUMBER")]
		public string DescricaoPartNumber { get; set; }

		[StringLength(20)]
		[Column("PDM_DS_REF_FABRICANTE")]
		public string DescricaoREFFabricante { get; set; }
		

		[Column("PDM_QT_UNID_COMERCIALIZADA")]
		public decimal? QuantidadeComercializada { get; set; }
		
		[Column("PDM_VL_UNITARIO_COND_VENDA")]
		public decimal? ValorUnitarioCondicaoVenda { get; set; }

		[Column("PDM_VL_CONDICAO_VENDA")]
		public decimal? ValorCondicaoVenda { get; set; }

		[Column("PDM_VL_CONDICAO_VENDA_REAL")]
		public decimal? ValorTotalCondicaoVendaReal { get; set; }

		[Column("PDM_VL_CONDICAO_VENDA_DOLAR")]
		public decimal? ValorTotalCondicaoVendaDolar { get; set; }

		[Column("PDM_VL_UNITARIO_COND_VENDA_DOLAR")]
		public decimal? ValorUnitarioCondicaoVendaDolar { get; set; }

		[Column("PME_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

	}
}