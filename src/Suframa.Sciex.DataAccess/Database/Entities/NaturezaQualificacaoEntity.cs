using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_NATUREZA_QUALIFICACAO")]
    public partial class NaturezaQualificacaoEntity : BaseEntity
    {
        [Column("NJU_ID")]
        public int IdNaturezaJuridica { get; set; }

        [Key]
        [Column("NQU_ID")]
        public int IdNaturezaQualificacao { get; set; }

        [Column("QUA_ID")]
        public int IdQualificacao { get; set; }

        public virtual NaturezaJuridicaEntity NaturezaJuridica { get; set; }

        public virtual QualificacaoEntity Qualificacao { get; set; }
    }
}