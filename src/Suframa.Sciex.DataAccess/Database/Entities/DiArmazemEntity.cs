using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_DI_ARMAZEM")]
	public partial class DiArmazemEntity : BaseEntity
	{		
		[Key]
		[Column("DIA_ID")]
		public long Id { get; set; }
		
		[Column("DIA_DS_ARMAZEM")]
		public string Descricao { get; set; }

		public virtual DiEntity Di { get; set; }

		[Column("DI_ID")]
		[ForeignKey(nameof(Di))]
		public long IdDi { get; set; }
	}
}