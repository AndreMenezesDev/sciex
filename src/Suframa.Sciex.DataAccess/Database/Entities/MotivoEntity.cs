using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_MOTIVO")]
	public partial class MotivoEntity : BaseEntity
	{
		public virtual ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }

		public MotivoEntity()
		{
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
		}


		[Key]
		[Column("MOT_ID")]
		public int IdMotivo { get; set; }

		[Required]
		[Column("MOT_CO")]
		public short Codigo { get; set; }

		[Required]
		[StringLength(120)]
		[Column("MOT_DS")]
		public string Descricao { get; set; }
	}
}