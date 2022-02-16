using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_INSCRICAO_CADASTRAL")]
    public class InscricaoCadastralEntity : BaseEntity
    {
        [Column("INS_NU_ANO")]
        public int? Ano { get; set; }

        [Column("INS_CO")]
        public int Codigo { get; set; }

        [Column("INS_DT")]
        public DateTime? Data { get; set; }

        [Key]
        [Column("INS_ID")]
        public int IdInscricaoCadastral { get; set; }

        [Column("PFI_ID")]
        [ForeignKey(nameof(PessoaFisica))]
        public int? IdPessoaFisica { get; set; }

        [Column("PJU_ID")]
        [ForeignKey(nameof(PessoaJuridica))]
        public int? IdPessoaJuridica { get; set; }

        [Column("STI_ID")]
        [ForeignKey(nameof(SituacaoInscricao))]
        public int? IdSituacaoInscricao { get; set; }

        [Column("INC_ID")]
        [ForeignKey(nameof(TipoIncentivo))]
        public int? IdTipoIncentivo { get; set; }

        [Column("UND_ID")]
        [ForeignKey(nameof(UnidadeCadastradora))]
        public int? IdUnidadeCadastradora { get; set; }

        public virtual PessoaFisicaEntity PessoaFisica { get; set; }

        public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }

        public virtual SituacaoInscricaoEntity SituacaoInscricao { get; set; }

        public virtual TipoIncentivoEntity TipoIncentivo { get; set; }

        public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }

        public virtual ICollection<WorkflowSituacaoInscricaoEntity> WorkflowSituacaoInscricao { get; set; }
    }
}