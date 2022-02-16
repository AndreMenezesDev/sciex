using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CADSUF_PAPEL")]
	public partial class PapelEntity : BaseEntity
	{
		[Column("PAP_DS")]
		[StringLength(50)]
		public string Descricao { get; set; }

		[Key]
		[Column("PAP_ID")]
		public int IdPapel { get; set; }

		public virtual ICollection<RecursoEntity> Recurso { get; set; }

		public virtual ICollection<UsuarioPapelEntity> UsuarioInternoPapel { get; set; }

		public PapelEntity()
		{
			Recurso = new HashSet<RecursoEntity>();
		}
	}
}