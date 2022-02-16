using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_WF_MENSAGEM_PADRAO")]
    public partial class WorkflowMensagemPadraoEntity : BaseEntity
    {
        [Column("MPA_ID")]
        [ForeignKey(nameof(MensagemPadrao))]
        public int IdMensagemPadrao { get; set; }

        [Key]
        [Column("WMP_ID")]
        public int IdWorkflowMensagemPadrao { get; set; }

        [Column("WPR_ID")]
        [ForeignKey(nameof(WorkflowProtocolo))]
        public int IdWorkflowProtocolo { get; set; }

        public virtual MensagemPadraoEntity MensagemPadrao { get; set; }

        public virtual WorkflowProtocoloEntity WorkflowProtocolo { get; set; }
    }
}