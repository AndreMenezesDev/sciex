using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("SCIEX_PARECER_TECNICO_PRODUTO")]
    public partial class ParecerTecnicoProdutoEntity : BaseEntity
    {
		public virtual ParecerTecnicoEntity ParecerTecnico { get; set; }


		[Key]
		[Column("ptp_id")]
		public long IdParecerTecnicoProduto { get; set; }

		[ForeignKey(nameof(ParecerTecnico))]
		[Column("pat_id")]
		public long? IdParecerTecnico { get; set; }

		[Column("ptp_nu_seq")]
		public int? NumeroSequencia { get; set; }

		[Column("ptp_co_produto_exportacao")]
		public int CodigoProdutoExportacao { get; set; }

		[Column("ptp_co_produto_suframa")]
		public int? CodigoProdutoSuframa { get; set; }

		[StringLength(8)]
		[Column("ptp_co_ncm")]
		public string CodigoNCM { get; set; }

		[Column("ptp_co_tp_produto")]
		public int? CodigoTipoProduto { get; set; }

		[StringLength(255)]
		[Column("ptp_ds_tipo")]
		public string DescricaoTipo { get; set; }
		
		[StringLength(255)]
		[Column("ptp_ds_produto")]
		public string DescricaoProduto { get; set; }

		[StringLength(255)]
		[Column("ptp_ds_modelo")]
		public string DescricaoModelo { get; set; }

		[StringLength(40)]
		[Column("ptp_ds_unidade")]
		public string DescricaoUnidade { get; set; }

		[Column("ptp_vl_unitario_produto_aprov")]
		public decimal? ValorUnitarioProdutoAprovado { get; set; }

		[Column("ptp_qt_produto_aprov")]
		public decimal? QuantidadeProdutoAprovado { get; set; }

		[Column("ptp_vl_insumo_nac_prod")]
		public decimal? ValorInsumoNacionalProduto { get; set; }

		[Column("ptp_vl_insumo_imp_prod")]
		public decimal? ValorInsumoImportacaoProduto { get; set; }

		[Column("ptp_vl_insumo_imp_prod_fob")]
		public decimal? ValorInsumoImportacaoProdutoFob { get; set; }

		[Column("ptp_vl_insumo_imp_prod_cfr")]
		public decimal? ValorInsumoImportacoCfr { get; set; }

		[StringLength(100)]
		[Column("ptp_ds_pais")]
		public string DescricaoPais { get; set; }

		[Column("ptp_qt_pais")]
		public decimal? QuantidadePais { get; set; }

		[Column("ptp_vl_pais")]
		public decimal? ValorPais { get; set; }

		[Column("ptp_vl_unitario_produto_comp")]
		public decimal? ValorUnitarioProdutoComprovado { get; set; }
		
		[Column("ptp_vl_insumo_nac_adquirido")]
		public decimal? ValorInsumoNacionalAdquirido { get; set; }
		
		[Column("ptp_vl_insumo_importado")]
		public decimal? ValorInsumoImportado { get; set; }
		
		[Column("ptp_vl_exportacao_comp")]
		public decimal? ValorExportacaoComprovado { get; set; }
		
		[Column("ptp_vl_exportacao_nacional_comp")]
		public decimal? ValorExportacaoNacionalComprovado { get; set; }
	}
}