using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PRC_DETALHE_INSUMO")]
	public partial class PRCDetalheInsumoEntity : BaseEntity
	{
		public virtual PRCInsumoEntity PrcInsumo { get; set; }
		public virtual MoedaEntity Moeda { get; set; }
		public virtual ICollection<PRCSolicDetalheEntity> PRCSolicDetalhe { get; set; }
		
		
		public PRCDetalheInsumoEntity()
		{
			PRCSolicDetalhe = new HashSet<PRCSolicDetalheEntity>();
		}
		
		[Key]
		[Column("det_id")]
		public int IdDetalheInsumo { get; set; }	
		
		[ForeignKey(nameof(PrcInsumo))]
		[Column("ins_id")]
		public int IdPrcInsumo { get; set; }

		[ForeignKey(nameof(Moeda))]
		[Column("moe_id")]
		public int? IdMoeda { get; set; }		

		[Column("det_nu_seq")]
		public int? NumeroSequencial { get; set; }
		
		[Column("pai_co")]
		public int? CodigoPais { get; set; }

		[Column("det_qt")]
		public decimal Quantidade { get; set; }

		[Column("det_vl_unitario")]
		public decimal? ValorUnitario { get; set; }

		[Column("det_vl_frete")]
		public decimal? ValorFrete { get; set; }

		[Column("det_vl_dolar")]
		public decimal? ValorDolar { get; set; }

		[Column("det_vl_unitario_cfr")]
		public decimal? ValorUnitarioCFR { get; set; }

		[Column("det_vl_dolar_cfr")]
		public decimal? ValorDolarCFR { get; set; }

		[Column("det_vl_dolar_fob")]
		public decimal? ValorDolarFOB { get; set; }

	}
}