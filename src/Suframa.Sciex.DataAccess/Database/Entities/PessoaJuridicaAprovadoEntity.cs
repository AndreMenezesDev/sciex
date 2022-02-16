using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_PESSOA_JURIDICA_APROVADO")]
    public partial class PessoaJuridicaAprovadoEntity : BaseEntity
    {
        [Column("PJA_DT_APROVADO", TypeName = "datetime2")]
        public DateTime DataAprovado { get; set; }

        [Column("PJU_ID")]
        [ForeignKey(nameof(PessoaJuridica))]
        public int IdPessoaJuridica { get; set; }

        [Key]
        [Column("PJA_ID")]
        public int IdPessoaJuridicaAprovado { get; set; }

        public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }
    }
}