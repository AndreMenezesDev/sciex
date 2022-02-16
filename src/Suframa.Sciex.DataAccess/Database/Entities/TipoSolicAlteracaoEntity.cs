using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_TIPO_SOLIC_ALTERACAO")]
	public class TipoSolicAlteracaoEntity : BaseEntity
	{
		[Key]
		[Column("TSO_ID")]
		public int Id { get; set; }

		[Column("TSO_DS")]
		public string Descricao { get; set; }
	}
}
