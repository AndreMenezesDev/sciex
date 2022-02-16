using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_REGIME_TRIBUTARIO")]
	public partial class RegimeTributarioEntity : BaseEntity
	{
		public virtual ICollection<ParametrosEntity> Parametros { get; set; }
		public virtual ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }

		[Key]
		[Column("RTB_ID")]
		public int IdRegimeTributario { get; set; }

		[Required]
		[StringLength(120)]
		[Column("RTB_DS")]
		public string Descricao { get; set; }

		[Column("RTB_CO")]
		public short Codigo { get; set; }

		[Column("RTB_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }


		public RegimeTributarioEntity()
		{
			Parametros = new HashSet<ParametrosEntity>();
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
		}
	}
}