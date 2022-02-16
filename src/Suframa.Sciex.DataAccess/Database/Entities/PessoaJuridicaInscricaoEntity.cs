using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_PESSOA_JURIDICA_INSCRICAO")]
    public class PessoaJuridicaInscricaoEntity
    {
        [Column("PJU_ID")]
        public int IdPessoaJuridica { get; set; }

        [Key]
        [Column("INS_ID")]
        public int IdPessoaJuridicaInscricao { get; set; }

        [Required]
        [StringLength(20)]
        [Column("INS_NU")]
        public string Numero { get; set; }

        public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }

        [Column("INS_TP_ESTADUAL_MUNICIPAL", TypeName = "numeric")]
        public decimal TipoEstadualMunicipal { get; set; }

        [Column("INS_TP_PRINCIPAL_SECUNDARIA", TypeName = "numeric")]
        public decimal TipoPrincipalSecundaria { get; set; }
    }
}