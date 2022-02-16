using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_GRUPO_ATIVIDADE")]
    public partial class GrupoAtividadeEntity : BaseEntity
    {
        public virtual ICollection<ClasseAtividadeEntity> ClasseAtividade { get; set; }

        [Column("GP_CO", TypeName = "numeric")]
        public decimal Codigo { get; set; }

        [Required]
        [StringLength(200)]
        [Column("GP_DS")]
        public string Descricao { get; set; }

        public virtual DivisaoAtividadeEntity DivisaoAtividade { get; set; }

        [Column("DIV_ID")]
        public int IdDivisaoAtividade { get; set; }

        [Key]
        [Column("GP_ID")]
        public int IdGrupoAtividade { get; set; }

        public GrupoAtividadeEntity()
        {
            ClasseAtividade = new HashSet<ClasseAtividadeEntity>();
        }
    }
}