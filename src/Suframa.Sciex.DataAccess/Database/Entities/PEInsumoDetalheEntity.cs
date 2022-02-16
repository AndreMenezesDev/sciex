
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PE_DETALHE_INSUMO")]
	public partial class PEDetalheInsumoEntity : BaseEntity
	{
		public virtual PEInsumoEntity PEInsumo { get; set; }
		public virtual MoedaEntity Moeda { get; set; }
		public PEDetalheInsumoEntity()
		{
		}

		[Key]
		[Column("DIN_ID")]
		public int IdPEDetalheInsumo { get; set; }

		[Column("INS_ID")]
		[ForeignKey(nameof(PEInsumo))]
		public int? IdPEInsumo { get; set; }

		[Column("MOE_ID")]
		[ForeignKey(nameof(Moeda))]
		public int? IdMoeda { get; set; }

		[Column("DIN_NU_SEQ")]
		public int NumeroSequencial { get; set; }

		[Column("PAI_CO")]
		public int CodigoPais { get; set; }

		[Column("DIN_QT")]
		public decimal Quantidade { get; set; }

		[Column("DIN_VL_UNITARIO")]
		public decimal ValorUnitario { get; set; }

		[Column("DIN_VL_FRETE")]
		public decimal? ValorFrete { get; set; }

		[Column("DIN_VL_DOLAR")]
		public decimal? ValorDolar { get; set; }

		[Column("DIN_VL_DOLAR_FOB")]
		public decimal? ValorDolarFOB { get; set; }

		[Column("DIN_VL_DOLAR_CFR")]
		public decimal? ValorDolarCRF { get; set; }

		[Column("DIN_ST_ANALISE")]
		public int? SituacaoAnalise { get; set; }

		[StringLength(1000)]
		[Column("DIN_DS_JUSTIFICATIVA_ERRO")]
		public string DescricaoJustificativaErro { get; set; }

	}
}