using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_CREDENCIAMENTO")]
    public class CredenciamentoEntity : BaseEntity
    {
        [Column("CRE_DT_CREDENCIAMENTO")]
        public DateTime? DataCredenciamento { get; set; }

        [Column("CRE_DT_VALIDADE")]
        public DateTime? DataValidade { get; set; }

        [Column("CRE_ID")]
        [Key]
        public int IdCredenciamento { get; set; }

        [Column("PFI_ID")]
        [ForeignKey(nameof(PessoaFisica))]
        public int? IdPessoaFisica { get; set; }

        [Column("PJU_ID")]
        [ForeignKey(nameof(PessoaJuridica))]
        public int? IdPessoaJuridica { get; set; }

        [Column("TUS_ID")]
        [ForeignKey(nameof(TipoUsuario))]
        public int? IdTipoUsuario { get; set; }

        [Column("UND_ID")]
        [ForeignKey(nameof(UnidadeCadastradora))]
        public int? IdUnidadeCadastradora { get; set; }

        [Column("CRE_ST_ATIVO")]
        public bool IsAtivo { get; set; }

        [Column("CRE_TP_MODALIDADE_TRANSP")]
        public int? ModalidadeTransportador { get; set; }

        public virtual PessoaFisicaEntity PessoaFisica { get; set; }

        public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }

        public virtual TipoUsuarioEntity TipoUsuario { get; set; }

        public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }
    }
}