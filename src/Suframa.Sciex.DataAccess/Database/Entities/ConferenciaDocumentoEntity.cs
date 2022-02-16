using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_WF_CONFERENCA_DOCUMENTO")]
    public partial class ConferenciaDocumentoEntity : BaseEntity
    {
        [Key]
        [Column("WCD_ID")]
        public int IdConferenciaDocumento { get; set; }

        [Column("TDO_ID")]
        [ForeignKey(nameof(TipoDocumento))]
        public int IdTipoDocumento { get; set; }

        [Column("WPR_ID")]
        [ForeignKey(nameof(WorkflowProtocolo))]
        public int IdWorkflowProtocolo { get; set; }

        public virtual TipoDocumentoEntity TipoDocumento { get; set; }

        public virtual WorkflowProtocoloEntity WorkflowProtocolo { get; set; }
    }
}