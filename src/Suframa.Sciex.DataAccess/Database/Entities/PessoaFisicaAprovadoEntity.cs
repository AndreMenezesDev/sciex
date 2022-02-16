using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_PESSOA_FISICA_APROVADO")]
    public partial class PessoaFisicaAprovadoEntity : BaseEntity
    {
        [Column("PFA_DT_APROVADO", TypeName = "datetime2")]
        public DateTime DataAprovado { get; set; }

        [Column("PFI_ID")]
        [ForeignKey(nameof(PessoaFisica))]
        public int IdPessoaFisica { get; set; }

        [Key]
        [Column("PFA_ID")]
        public int IdPessoaFisicaAprovado { get; set; }

        public virtual PessoaFisicaEntity PessoaFisica { get; set; }
    }
}