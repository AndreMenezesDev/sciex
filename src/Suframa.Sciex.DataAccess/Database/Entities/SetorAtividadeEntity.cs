using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_SETOR_ATIVIDADE")]
    public partial class SetorAtividadeEntity : BaseEntity
    {
        [Column("SET_ID")]
        public int IdSetor { get; set; }

        [Key]
        [Column("SAT_ID")]
        public int IdSetorAtividade { get; set; }

        [Column("SBC_ID")]
        public int IdSubclasseAtividade { get; set; }

        public virtual SetorEntity Setor { get; set; }

        public virtual SubclasseAtividadeEntity SubclasseAtividade { get; set; }
    }
}