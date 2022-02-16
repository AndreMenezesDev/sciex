using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_TAXA_SERVICO")]
    public partial class TaxaServicoEntity : BaseEntity
    {
        [Column("TXS_NU_ANO_DEBITO")]
        public int? AnoDebito { get; set; }

        [Column("TXS_DT_EXPIRACAO")]
        public DateTime? DataExpiracao { get; set; }

        [Column("TXS_DT_PAGAMENTO")]
        public DateTime? DataPagamento { get; set; }

        [Column("TXS_DT_VENCIMENTO")]
        public DateTime? DataVencimento { get; set; }

        [StringLength(100)]
        [Column("TXS_DS_MSG_RETORNO")]
        public string DescricaoMsgRetorno { get; set; }

        [Column("TXS_EXTRATO", TypeName = "text")]
        [Required]
        public string Extrato { get; set; }

        [Column("PRT_ID")]
        [ForeignKey(nameof(Protocolo))]
        public int IdProtocolo { get; set; }

        [Key]
        [Column("TXS_ID")]
        public int IdTaxaServico { get; set; }

        [Column("TXS_NU_DEBITO")]
        public int? NumeroDebito { get; set; }

        public virtual ProtocoloEntity Protocolo { get; set; }

        [Column("TXS_ST")]
        public int? Status { get; set; }
    }
}