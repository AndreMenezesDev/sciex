using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("SCIEX_PRC_DETALHE_HISTORICO")]
    public partial class PRCDetalheHistoricoInsumoEntity : BaseEntity
    {
		public virtual PRCHistoricoInsumoEntity HistoricoInsumo { get; set; }

		[Key]
		[Column("deh_id")]
		public int IdDetalheHistoricoInsumo { get; set; }

		[ForeignKey(nameof(HistoricoInsumo))]
		[Column("his_id")]
		public int? IdPRCHistoricoInsumo { get; set; }

		[StringLength(50)]
		[Column("deh_ds_evento")]
		public string DescricaoEvento { get; set; }

		[StringLength(150)]
		[Column("deh_ds_detalhe")]
		public string DescricaoDetalhe { get; set; }

		[StringLength(50)]
		[Column("deh_tp_evento")]
		public string TipoEvento { get; set; }
		
	}
}