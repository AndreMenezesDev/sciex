using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_CLASSE_ATIVIDADE")]
    public partial class ClasseAtividadeEntity : BaseEntity
    {
        [Column("CLA_CO", TypeName = "numeric")]
        public decimal Codigo { get; set; }

        [Required]
        [StringLength(200)]
        [Column("CLA_DS")]
        public string Descricao { get; set; }

        public virtual GrupoAtividadeEntity GrupoAtividade { get; set; }

        [Key]
        [Column("CLA_ID")]
        public int IdClasseAtividade { get; set; }

        [Column("GP_ID")]
        public int IdGrupoAtividade { get; set; }

        public virtual ICollection<SubclasseAtividadeEntity> SubclasseAtividade { get; set; }

        public ClasseAtividadeEntity()
        {
            SubclasseAtividade = new HashSet<SubclasseAtividadeEntity>();
        }
    }
}