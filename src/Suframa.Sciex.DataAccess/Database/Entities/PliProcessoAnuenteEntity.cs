using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PLI_PROCESSO_ANUENTE")]
	public partial class PliProcessoAnuenteEntity : BaseEntity
	{
		public virtual PliMercadoriaEntity PLIMercadoria { get; set; }
		public virtual OrgaoAnuenteEntity OrgaoAnuente { get; set; }

		[Key]
		[Column("PPA_ID")]
		public int IdPliProcessoAnuente { get; set; }

		[Required]
		[Column("PME_ID")]
		[ForeignKey(nameof(PLIMercadoria))]
		public long IdPliMercadoria { get; set; }

		[Required]
		[Column("OAN_ID")]
		[ForeignKey(nameof(OrgaoAnuente))]
		public int IdOrgaoAnuente { get; set; }

		[Required]
		[StringLength(20)]
		[Column("PPA_NU_PROCESSO")]
		public string NumeroProcesso { get; set; }
		
	}
}