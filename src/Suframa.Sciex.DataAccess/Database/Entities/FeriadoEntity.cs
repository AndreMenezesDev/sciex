using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_FERIADO")]
    public partial class FeriadoEntity : BaseEntity
    {
        [Column("FER_DT")]
        public DateTime Data { get; set; }

        [Required]
        [Column("FER_DS")]
        public string Descricao { get; set; }

        [Key]
        [Column("FER_ID")]
        public int IdFeriado { get; set; }
    }
}