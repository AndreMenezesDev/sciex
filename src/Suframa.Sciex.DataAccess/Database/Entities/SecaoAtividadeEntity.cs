using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_SECAO_ATIVIDADE")]
    public partial class SecaoAtividadeEntity : BaseEntity
    {
        [Required]
        [StringLength(1)]
        [Column("SEC_CO")]
        public string Codigo { get; set; }

        [Required]
        [StringLength(200)]
        [Column("SEC_DS")]
        public string Descricao { get; set; }

        public virtual ICollection<DivisaoAtividadeEntity> DivisaoAtividade { get; set; }

        [Key]
        [Column("SEC_ID")]
        public int IdSecaoAtividade { get; set; }

        public SecaoAtividadeEntity()
        {
            DivisaoAtividade = new HashSet<DivisaoAtividadeEntity>();
        }
    }
}