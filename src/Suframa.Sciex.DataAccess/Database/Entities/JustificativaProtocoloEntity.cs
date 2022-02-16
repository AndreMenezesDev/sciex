using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_JUSTIFICATIVA_PROTOCOLO")]
    public partial class JustificativaProtocoloEntity : BaseEntity
    {
        [Column("JPR_DS")]
        public string Descricao { get; set; }

        [Key]
        [Column("JPR_ID")]
        public int IdJustificativa { get; set; }

        [Column("WPR_ID")]
        public int? IdWorkflowProtocolo { get; set; }

        [Column("JPR_NU_SEQ")]
        public int? NumeroSequencial { get; set; }

        public virtual WorkflowProtocoloEntity WorkflowProtocolo { get; set; }
    }
}