using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_NALADI")]
	public partial class NaladiEntity : BaseEntity
	{
		public virtual ICollection<ParametrosEntity> Parametros { get; set; }
		public virtual ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }

		[Key]
		[Column("NLD_ID")]
		public int IdNaladi { get; set; }

		[Required]
		[Column("NLD_CO")]
		public int Codigo { get; set; }

		[Required]
		[StringLength(120)]
		[Column("NLD_DS")]
		public string Descricao { get; set; }

		[Column("NLD_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte [] RowVersion { get; set; }

		public NaladiEntity()
		{
			Parametros = new HashSet<ParametrosEntity>();
			PliMercadoria = new HashSet<PliMercadoriaEntity>();

		}
	}
}