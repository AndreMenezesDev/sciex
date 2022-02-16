using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ALADI")]
	public partial class AladiEntity : BaseEntity
	{
		public virtual ICollection<ParametrosEntity> Parametros { get; set; }
		public virtual ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }

		[Key]
		[Column("ALA_ID")]
		public int IdAladi { get; set; }

		[Required]
		[Column("ALA_CO")]
		public short Codigo { get; set; }

		[Required]
		[StringLength(120)]
		[Column("ALA_DS")]
		public string Descricao { get; set; }
		
		[Column("ALA_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

		public AladiEntity()
		{
			Parametros = new HashSet<ParametrosEntity>();
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
		}
	}
}