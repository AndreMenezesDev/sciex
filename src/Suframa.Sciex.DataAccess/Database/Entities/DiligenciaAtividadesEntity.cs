using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CADSUF_DILIGENCIA_ATIVIDADES")]
	public partial class DiligenciaAtividadesEntity : BaseEntity
	{
		[Column("DGA_CO_SUBCLASSE")]
		public string CodigoSubclasse { get; set; }

		[Column("DGA_DS_SUBCLASSE")]
		public string DescricaoSubclasse { get; set; }

		public virtual DiligenciaEntity Diligencia { get; set; }

		public virtual ICollection<DiligenciaAtividadesSetorEntity> DiligenciaAtividadeSetor { get; set; }

		[Column("DLG_ID")]
		[ForeignKey(nameof(Diligencia))]
		public int? IdDiligencia { get; set; }

		[Key]
		[Column("DGA_ID")]
		public int IdDiligenciaAtividade { get; set; }

		[Column("DGA_ST_ATV_EXERCIDA")]
		public bool? StatusAtividadeExercida { get; set; }

		public DiligenciaAtividadesEntity()
		{
			DiligenciaAtividadeSetor = new HashSet<DiligenciaAtividadesSetorEntity>();
		}
	}
}