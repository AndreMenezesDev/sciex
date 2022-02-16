using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("VW_CADSUF_FILA_ANALISTA")]
    public partial class FilaAnalistaEntity : BaseEntity
    {
        [Key]
        [Column("USI_ID")]
        public int IdUsuario { get; set; }

        [Column("QT_DIAS_FILA")]
        public int QuantidadeDiasFila { get; set; }
    }
}