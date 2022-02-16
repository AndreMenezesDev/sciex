using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_LI_SUBSTITUIDA")]
	public partial class LiSubstituidaEntity : BaseEntity
	{
		public virtual LiEntity LiOrigem { get; set; }

		[Key]
		[Column("pme_id_substituida",Order = 0)]
		public long IdLiSubstituida { get; set; }

		[Key]
		[Column("pme_id_substituta", Order = 1)]
		public long IdLiSubstituta { get; set; }

		[Column("pme_id_origem")]
		[ForeignKey(nameof(LiOrigem))]
		public long IdLiOrigem { get; set; }

		[Required]
		[Column("lsu_nu")]
		public byte NumeroLsu { get; set; }

		[Required]
		[Column("lsu_st")]
		public byte Status { get; set; }

		[Column("lsu_dh_operacao")]
		public DateTime? DataOperacao { get; set; }
	}
}