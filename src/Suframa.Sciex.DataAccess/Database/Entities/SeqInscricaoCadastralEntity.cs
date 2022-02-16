using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_SEQ_INSCRICAO_CADASTRAL")]
    public partial class SeqInscricaoCadastralEntity : BaseEntity
    {
        [Key]
        [Column("SIC_ID")]
        public int IdSeqInscricaoCadastral { get; set; }

        [Column("UND_ID")]
        [ForeignKey(nameof(UnidadeCadastradora))]
        public int IdUnidadeCadastradora { get; set; }

        [Column("SIC_NU_SEQ")]
        public int NumeroSequencial { get; set; }

        [Column("SIC_TP_PESSOA")]
        public int TipoPessoa { get; set; }

        public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }
    }
}