using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_WORFLOW_SITUACAO_INSCRICAO")]
    public class WorkflowSituacaoInscricaoEntity : BaseEntity
    {
        [Column("WSI_DT_SITUACAO")]
        [Required]
        public DateTime Data { get; set; }

        public virtual ICollection<HistoricoSituacaoInscricaoEntity> HistoricoSituacaoInscricao { get; set; }

        [Column("INS_ID")]
        [ForeignKey(nameof(InscricaoCadastral))]
        [Required]
        public int IdInscricaoCadastral { get; set; }

        [Column("STI_ID")]
        [ForeignKey(nameof(SituacaoInscricao))]
        [Required]
        public int IdSituacaoInscricao { get; set; }

        [Column("WSI_ID")]
        [Key]
        public int IdWorkflowSituacaoInscricao { get; set; }

        public virtual InscricaoCadastralEntity InscricaoCadastral { get; set; }

        public virtual SituacaoInscricaoEntity SituacaoInscricao { get; set; }

        public WorkflowSituacaoInscricaoEntity()
        {
            HistoricoSituacaoInscricao = new HashSet<HistoricoSituacaoInscricaoEntity>();
        }
    }
}