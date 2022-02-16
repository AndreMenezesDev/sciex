using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_MENSAGEM_PADRAO")]
    public partial class MensagemPadraoEntity : BaseEntity
    {
        [Column("MPA_DS")]
        [StringLength(500)]
        public string Descricao { get; set; }

        [Key]
        [Column("MPA_ID")]
        public int IdMensagemPadrao { get; set; }

        public virtual ICollection<PedidoCorrecaoEntity> PedidoCorrecao { get; set; }

        [Column("MPA_ST")]
        public int? Status { get; set; }

        [Column("MPA_TP_GRUPO")]
        public int? TipoGrupo { get; set; }

        public virtual ICollection<WorkflowMensagemPadraoEntity> WorkflowMensagemPadrao { get; set; }

        public MensagemPadraoEntity()
        {
            WorkflowMensagemPadrao = new HashSet<WorkflowMensagemPadraoEntity>();
            PedidoCorrecao = new HashSet<PedidoCorrecaoEntity>();
        }
    }
}