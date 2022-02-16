using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_TIPO_DECLARACAO")]
	public partial class TipoDeclaracaoEntity : BaseEntity
	{
		public TipoDeclaracaoEntity()
		{

		}

		[Key]
		[Column("TDE_ID")]
		public int IdTipoDeclaracao { get; set; }

		[Required]
		[StringLength(255)]
		[Column("TDE_DS")]
		public string Descricao { get; set; }

		[Required]
		[Column("TDE_ST")]
		public byte Status { get; set; }

		[Required]
		[Column("TDE_CO")]
		public short Codigo { get; set; }

		[Column("TDE_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }
	}
}