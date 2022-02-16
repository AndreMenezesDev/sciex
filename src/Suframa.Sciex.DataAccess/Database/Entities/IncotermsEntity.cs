using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_INCOTERMS")]
	public partial class IncotermsEntity : BaseEntity
	{
		public virtual ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }

		public IncotermsEntity()
		{
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
		}

		[Key]
		[Column("INC_ID")]
		public int IdIncoterms { get; set; }

		[Required]
		[StringLength(3)]
		[Column("INC_CO")]
		public string Codigo { get; set; }

		[Required]
		[StringLength(120)]
		[Column("INC_DS")]
		public string Descricao { get; set; }
	}
}