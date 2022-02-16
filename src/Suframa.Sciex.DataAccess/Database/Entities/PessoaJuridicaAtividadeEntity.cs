using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_PESSOA_JURIDICA_ATIVIDADE")]
    public partial class PessoaJuridicaAtividadeEntity : BaseEntity
    {
        [Key]
        [Column("ATV_ID")]
        public int IdAtividade { get; set; }

        [Column("PJU_ID")]
        [ForeignKey(nameof(PessoaJuridica))]
        public int IdPessoaJuridica { get; set; }

        [Column("SBC_ID")]
        [ForeignKey(nameof(SubClasseAtividade))]
        public int IdSubclasseAtividade { get; set; }

        public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }

        [Column("ATV_ST_ATUANTE")]
        public bool StatusAtuante { get; set; }

        public virtual SubclasseAtividadeEntity SubClasseAtividade { get; set; }

        [Column("ATV_TP")]
        public int Tipo { get; set; }
    }
}