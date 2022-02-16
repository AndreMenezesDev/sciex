using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_TAXA_PLI_DETALHE_MERCADORIA")]
	public partial class TaxaPliDetalheMercadoriaEntity : BaseEntity
	{

		public virtual PliMercadoriaEntity PliMercadoriaEntity { get; set; }
		public virtual PliDetalheMercadoriaEntity PliDetalheMercadoria { get; set; }
		public virtual TaxaFatoGeradorEntity TaxaFatoGerador { get; set; }

		[Key]
		[ForeignKey(nameof(PliDetalheMercadoria))]
		[Column("PDM_ID")]		
		public long IdPliDetalheMercadoria { get; set; }

		[ForeignKey(nameof(TaxaFatoGerador))]
		[Column("TFG_ID")]
		public int? IdTaxaFatoGerador { get; set; }

		[ForeignKey(nameof(PliMercadoriaEntity))]
		[Column("PME_ID")]
		public long? IdPliMercadoria { get; set; }

		[Column("TGB_ID_ISENCAO")]
		public int? Isencao { get; set; }

		[Column("TGB_ID_REDUCAO")]
		public int? Reducao { get; set; }

		[StringLength(255)]
		[Column("PDM_DS_DETALHE")]
		public string DescricaoDetalhe { get; set; }

		[Column("PDM_VL_BASE_FATO_GERADOR_ITEM")]
		public decimal? ValorBaseFatoGeradorItem { get; set; }

		[Column("PDM_VL_PERC_LIMITADOR_ITEM")]
		public decimal? ValorPercentualLimitadorItem { get; set; }

		[Column("PDM_VL_PERC_REDUCAO_ITEM")]
		public decimal? ValorPercentualReducaoItem { get; set; }

		[Column("PDM_QT_UNID_COMERCIALIZADA")]
		public decimal? QtdUnidadeComercializada { get; set; }

		[Column("PDM_VL_UNID_CONDICAO_VENDA")]
		public decimal? ValorUnidadeCondicaoVenda { get; set; }

		[Column("PDM_VL_UNID_REAIS")]
		public decimal? ValorUnidadeReais { get; set; }

		[Column("PDM_VL_CALC_LIMITADOR_ITEM")]
		public decimal? ValorCalculadoLimitadorItem { get; set; }

		[Column("PDM_VL_PREVALENCIA_ITEM")]
		public decimal? ValorPrevalenciaItem { get; set; }

		[Column("PDM_VL_REDUCAO_ITEM")]
		public decimal? ValorReducaoItem { get; set; }

		[Column("PDM_VL_TCIF_ITEM")]
		public decimal? ValorTCIFItem { get; set; }

		[Column("PDM_DH_CADASTRO")]
		public DateTime? DataCadastro { get; set; }

		[Column("PDM_VL_REDUCAO_BASE_ITEM")]
		public decimal? ValorReducaoBaseItem { get; set; }
	}
}