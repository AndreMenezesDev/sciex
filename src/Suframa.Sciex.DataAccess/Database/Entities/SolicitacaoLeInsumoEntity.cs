using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SOLIC_LE_INSUMO")]
	public partial class SolicitacaoLeInsumoEntity : BaseEntity
	{
		public virtual EstruturaPropriaLEEntity EstruturaPropriaLe { get; set; }
		public SolicitacaoLeInsumoEntity()
		{

		}

		[Key]
		[Column("sli_id")]
		public int IdSolicitacaoLeInsumo { get; set; }

		[ForeignKey(nameof(EstruturaPropriaLe))]
		[Column("esp_id")]
		public long? IdEstruturaPropriaLe { get; set; }

		[Required]
		[Column("sli_co_destaque")]
		public int CodigoDestaque { get; set; }

		[Column("sli_co_ncm")]
		[StringLength(8)]
		public string CodigoNCM { get; set; }

		[Column("sli_tp_insumo")]
		[StringLength(1)]
		public string TipoInsumo { get; set; }

		[Column("sli_co_unidade")]
		public decimal? CodigoUnidade { get; set; }

		[Column("sli_ds_insumo")]
		[StringLength(254)]
		public string DescricaoInsumo { get; set; }

		[Column("sli_vl_coeficiente_tecnico")]
		public decimal? ValorCoeficienteTecnico { get; set; }

		[Column("sli_st_insumo")]
		public int SituacaoInsumo { get; set; }

		[Column("sli_co_part_number")]
		[StringLength(30)]
		public string CodigoPartNumber { get; set; }

		[Column("sli_ds_especificacao_tecnica")]
		[StringLength(3723)]
		public string DescricaoEspecificacaoTecnica { get; set; }

		[Column("sli_nu_linha")]
		public int NumeroLinha { get; set; }

	}
}