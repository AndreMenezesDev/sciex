using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_NATUREZA_GRUPO")]
    public partial class NaturezaGrupoEntity : BaseEntity
    {
        [Column("NGR_CO")]
        public int Codigo { get; set; }

        [Required]
        [StringLength(200)]
        [Column("NGR_DS")]
        public string Descricao { get; set; }

        [Key]
        [Column("NGR_ID")]
        public int IdNaturezaGrupo { get; set; }

        public virtual ICollection<NaturezaJuridicaEntity> NaturezaJuridica { get; set; }

        public NaturezaGrupoEntity()
        {
            NaturezaJuridica = new HashSet<NaturezaJuridicaEntity>();
        }
    }
}