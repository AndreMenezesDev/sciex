using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_HISTORICO_SITUACAO_INSCRICAO")]
    public class HistoricoSituacaoInscricaoEntity : BaseEntity
    {
        [Column("HSI_DH_FIM")]
        public DateTime? DataHoraFim { get; set; }

        [Column("HSI_DH_INICIO")]
        public DateTime? DataHoraInicio { get; set; }

        [Column("HSI_DS_JUSTIFICATIVA_FIM")]
        [StringLength(500)]
        public string DescricaoJustificativaFim { get; set; }

        [Column("HSI_DS_JUSTIFICATIVA_INICIO")]
        [StringLength(500)]
        public string DescricaoJustificativaInicio { get; set; }

        [Column("HSI_DS_SETOR_FIM")]
        [StringLength(20)]
        public string DescricaoSetorFim { get; set; }

        [Column("HSI_DS_SETOR_INICIO")]
        [StringLength(20)]
        public string DescricaoSetorInicio { get; set; }

        [Column("HBD_ID")]
        [Key]
        public int IdHistoricoSituacaoInscricao { get; set; }

        [Column("MSI_ID")]
        [ForeignKey(nameof(MotivoSituacaoInscricao))]
        public int? IdMotivoSituacaoInscricao { get; set; }

        [Column("WSI_ID")]
        [ForeignKey(nameof(WorkflowSituacaoInscricao))]
        public int? IdWorkflowSituacaoInscricao { get; set; }

		[Column("HSI_ST_SINC_BLOQUEIO")]
		public int? SituacaoSincronismoBloqueio { get; set; }

		[Column("HSI_ST_SINC_DESBLOQUEIO")]
		public int? SituacaoSincronismoDesbloqueio { get; set; }

		[Column("HSI_DS_LOGIN_FIM")]
        [StringLength(20)]
        public string LoginFim { get; set; }

        [Column("HSI_DS_LOGIN_INICIO")]
        [StringLength(20)]
        public string LoginInicio { get; set; }

        public virtual MotivoSituacaoInscricaoEntity MotivoSituacaoInscricao { get; set; }

        [Column("HSI_NM_RESPONSAVEL_FIM")]
        [StringLength(50)]
        public string NomeResponsavelFim { get; set; }

        [Column("HSI_NM_RESPONSAVEL_INICIO")]
        [StringLength(50)]
        public string NomeResponsavelInicio { get; set; }

        public virtual WorkflowSituacaoInscricaoEntity WorkflowSituacaoInscricao { get; set; }
    }
}