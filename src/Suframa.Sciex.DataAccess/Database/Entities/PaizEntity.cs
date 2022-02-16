using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("SCIEX_PAIS")]
    public partial class PaizEntity : BaseEntity
    {
		[Key]
		[Column("PAI_ID")]
		public int IdPaiz { get; set; }

		[Required]
		[StringLength(120)]
		[Column("PAI_DS")]
		public string Descricao { get; set; }

		[Required]
		[StringLength(3)]
		[Column("PAI_CO")]
		public string Codigo { get; set; }
	}
}