using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PRC_INSUMO")]
	public partial class PRCInsumoEntity : BaseEntity
	{
		public virtual PRCProdutoEntity PrcProduto { get; set; }
		public virtual PRCSolicitacaoAlteracaoEntity PrcSolicitacaoAlteracao{ get; set; }
		public virtual ICollection<PRCDetalheInsumoEntity> ListaDetalheInsumos { get; set; }
		public virtual ICollection<PRCSolicDetalheEntity> PRCSolicDetalhe { get; set; }

		public PRCInsumoEntity()
		{
			ListaDetalheInsumos = new HashSet<PRCDetalheInsumoEntity>();
			PRCSolicDetalhe = new HashSet<PRCSolicDetalheEntity>();
		}

		[Key]
		[Column("ins_id")]
		public int IdInsumo { get; set; }	
		
		[ForeignKey(nameof(PrcProduto))]
		[Column("pro_id")]
		public int IdPrcProduto { get; set; }

		[ForeignKey(nameof(PrcSolicitacaoAlteracao))]
		[Column("soa_id")]
		public int? IdPrcSolicitacaoAlteracao { get; set; }

		[Column("ins_co_insumo")]
		public int? CodigoInsumo { get; set; }
		
		[Column("ins_co_unidade")]
		public int? CodigoUnidade { get; set; }

		[StringLength(1)]
		[Column("ins_tp_insumo")]
		public string TipoInsumo { get; set; }

		[StringLength(8)]
		[Column("ins_co_ncm")]
		public string CodigoNCM { get; set; }

		[Column("ins_vl_percentual_perda")]
		public decimal? ValorPercentualPerda { get; set; }
		

		[Column("ins_co_detalhe")]
		public int? CodigoDetalhe { get; set; }

		[StringLength(255)]
		[Column("ins_ds_insumo")]
		public string DescricaoInsumo { get; set; }

		[StringLength(20)]
		[Column("ins_ds_part_number")]
		public string DescricaoPartNumber { get; set; }

		[StringLength(3723)]
		[Column("ins_ds_especificacao_tecnica")]
		public string DescricaoEspecificacaoTecnica { get; set; }

		[Column("ins_vl_coeficiente_tecnico")]
		public decimal? ValorCoeficienteTecnico { get; set; }

		[Column("ins_vl_dolar_aprov")]
		public decimal? ValorDolarAprovado { get; set; }

		[Column("ins_qt_aprov")]
		public decimal? QuantidadeAprovado { get; set; }

		[Column("ins_vl_nacional_aprov")]
		public decimal? ValorNacionalAprovado { get; set; }

		[Column("ins_vl_dolar_fob_aprov")]
		public decimal? ValorDolarFOBAprovado { get; set; }

		[Column("ins_vl_dolar_cfr_aprov")]
		public decimal? ValorDolarCFRAprovado { get; set; }

		[Column("ins_vl_frete_aprov")]
		public decimal? ValorFreteAprovado { get; set; }

		[Column("ins_vl_dolar_comp")]
		public decimal? ValorDolarComp { get; set; }

		[Column("ins_qt_comp")]
		public decimal? QuantidadeComp { get; set; }

		[Column("ins_vl_dolar_saldo")]
		public decimal? ValorDolarSaldo { get; set; }

		[Column("ins_qt_saldo")]
		public decimal? QuantidadeSaldo { get; set; }

		[Column("ins_vl_dolar_unitario")]
		public decimal? ValorDolarUnitario { get; set; }

		[Column("ins_vl_dolar_unitario_cfr")]
		public decimal? ValorDolarUnitarioCrf { get; set; }

		[Column("ins_qt_adicional")]
		public decimal? QuantidadeAdicional { get; set; }

		[Column("ins_st")]
		public int? StatusInsumo { get; set; }
		
		[Column("ins_st_novo")]
		public int? StatusInsumoNovo { get; set; }

		[Column("ins_dt_inclusao")]
		public DateTime? DataInclusao { get; set; }

		[Column("ins_vl_dolar_adicional")]
		public decimal? ValorAdicional { get; set; }

		[Column("ins_vl_dolar_adicional_frete")]
		public decimal? ValorAdicionalFrete { get; set; }

		[Column("ins_vl_dolar_saldo_cancelado")]
		public decimal? ValorDolarSaldoCancelado { get; set; }

		[Column("ins_qt_saldo_cancelado")]
		public decimal? QuantidadeSaldoCancelado { get; set; }

	}
}