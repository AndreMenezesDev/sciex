using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_DI_ARMAZEM_ENTRADA")]
	public partial class DiArmazemEntradaEntity : BaseEntity
	{		
		[Key]
		[Column("DAE_ID")]
		public long Id { get; set; }
		
		[Column("DAE_DS_ARMAZEM")]
		public string Descricao { get; set; }

		[Column("DAE_NU_DI")]
		public string Numero { get; set; }

		public virtual DiEntradaEntity DiEntrada { get; set; }

		[Column("DIE_ID")]
		[ForeignKey(nameof(DiEntrada))]
		public long IdDiEntrada { get; set; }
	}
}