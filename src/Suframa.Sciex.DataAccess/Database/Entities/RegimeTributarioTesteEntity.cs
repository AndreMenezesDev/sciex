using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_REGIME_TRIBUTARIO_TESTE")]
	public partial class RegimeTributarioTesteEntity : BaseEntity
	{
		[Key]
		[Column("RTB_ID")]
		public int IdRegimeTributario { get; set; }

		[Required]
		[StringLength(120)]
		[Column("RTB_DS")]
		public string Descricao { get; set; }

		[Required]
		[StringLength(3)]
		[Column("RTB_CO")]
		public string Codigo { get; set; }
	}
}