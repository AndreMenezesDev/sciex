using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_PESSOA_JURIDICA_INSCRICAO_ESTADUAL")]
    public partial class PessoaJuridicaInscricaoEstadualEntity : BaseEntity
    {
        [Key]
        [Column("INS_ID")]
        public int IdInscricao { get; set; }

        [Column("PJU_ID")]
        [ForeignKey(nameof(PessoaJuridica))]
        public int IdPessoaJuridica { get; set; }

        [Required]
        [StringLength(20)]
        [Column("INS_NU")]
        public string Numero { get; set; }

        public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }
    }
}