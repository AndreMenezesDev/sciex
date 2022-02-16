using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_QUALIFICACAO")]
    public partial class QualificacaoEntity : BaseEntity
    {
        [Column("QUA_CO")]
        public int Codigo { get; set; }

        [Required]
        [StringLength(50)]
        [Column("QUA_DS")]
        public string Descricao { get; set; }

        [Key]
        [Column("QUA_ID")]
        public int IdQualificacao { get; set; }

        public virtual ICollection<NaturezaQualificacaoEntity> NaturezaQualificacao { get; set; }

        public virtual ICollection<PessoaJuridicaAdministradorEntity> PessoaJuridicaAdministrador { get; set; }

        public virtual ICollection<PessoaJuridicaSocioEntity> PessoaJuridicaSocio { get; set; }

        public QualificacaoEntity()
        {
            NaturezaQualificacao = new HashSet<NaturezaQualificacaoEntity>();
            PessoaJuridicaAdministrador = new HashSet<PessoaJuridicaAdministradorEntity>();
            PessoaJuridicaSocio = new HashSet<PessoaJuridicaSocioEntity>();
        }
    }
}