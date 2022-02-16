using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_STATUS_PROTOCOLO")]
    public partial class StatusProtocoloEntity : BaseEntity
    {
        [Required]
        [StringLength(50)]
        [Column("SPR_DS")]
        public string Descricao { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("SPR_ID")]
        public int IdStatusProtocolo { get; set; }

        [Required]
        [StringLength(500)]
        [Column("SPR_DS_ORIENTACAO")]
        public string Orientacao { get; set; }

        public virtual ICollection<ProtocoloEntity> Protocolo { get; set; }

        [Column("SPR_NU_PRIORIDADE_LISTA_PROTOCOLO")]
        public int? Rank { get; set; }

        public virtual ICollection<WorkflowProtocoloEntity> WorkflowProtocolo { get; set; }

        public StatusProtocoloEntity()
        {
            Protocolo = new HashSet<ProtocoloEntity>();
            WorkflowProtocolo = new HashSet<WorkflowProtocoloEntity>();
        }
    }
}