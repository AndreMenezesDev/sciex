using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("VW_UF")]
    public partial class UFEntity : BaseEntity
    {
        [Key]
        [StringLength(2)]
        [Column("MUN_SG_UF")]
        public string SiglaUF { get; set; }
    }
}