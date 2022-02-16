using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_DIVISAO_ATIVIDADE")]
    public partial class DivisaoAtividadeEntity : BaseEntity
    {
        [Column("DIV_CO", TypeName = "numeric")]
        public decimal Codigo { get; set; }

        [Required]
        [StringLength(200)]
        [Column("DIV_DS")]
        public string Descricao { get; set; }

        public virtual ICollection<GrupoAtividadeEntity> GrupoAtividade { get; set; }

        [Key]
        [Column("DIV_ID")]
        public int IdDivisaoAtividade { get; set; }

        [Column("SEC_ID")]
        public int IdSecaoAtividade { get; set; }

        public virtual SecaoAtividadeEntity SecaoAtividade { get; set; }

        public DivisaoAtividadeEntity()
        {
            GrupoAtividade = new HashSet<GrupoAtividadeEntity>();
        }
    }
}