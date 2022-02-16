using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_RECINTO_ALFANDEGA")]
	public class RecintoAlfandegaEntity : BaseEntity
	{
		public virtual ICollection<SetorArmazenamentoEntity> SetorArmazenamento { get; set; }
		public RecintoAlfandegaEntity()
		{
			SetorArmazenamento = new HashSet<SetorArmazenamentoEntity>();
		}

		[Key]
		[Column("RAL_ID")]
		public int? Id { get; set; }

		[Required]
		[StringLength(200)]
		[Column("RAL_DS")]
		public string Descricao { get; set; }

		[Column("RAL_ST")]
		public byte Status { get; set; }

		[Column("RAL_CO")]
		public int Codigo { get; set; }

		[Column("RAL_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

	}
}
