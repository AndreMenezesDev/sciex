using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_MOTIVO_SITUACAO_INSCRICAO")]
    public partial class MotivoSituacaoInscricaoEntity : BaseEntity
    {
        [Column("MSI_CO")]
        [Required]
        public int Codigo { get; set; }

        [Column("MSI_DS")]
        [Required]
        [StringLength(20)]
        public string Descricao { get; set; }

        [Column("MSI_DS_EXPLICACAO")]
        [StringLength(500)]
        public string Explicacao { get; set; }

        public virtual ICollection<HistoricoSituacaoInscricaoEntity> HistoricoSituacaoInscricao { get; set; }

        [Column("MSI_ID")]
        [Key]
        public int IdMotivoSituacaoInscricao { get; set; }

        [Column("STI_ID")]
        [ForeignKey(nameof(SituacaoInscricao))]
        public int? IdSituacaoInscricao { get; set; }

        [Column("USE_ID")]
        [ForeignKey(nameof(UsuarioInternoSetor))]
        public int? IdUsuarioInternoSetor { get; set; }

        [Column("MSI_DS_ORIENTACAO")]
        [StringLength(500)]
        public string Orientacao { get; set; }

        public virtual SituacaoInscricaoEntity SituacaoInscricao { get; set; }

        [Column("MSI_TP_AREA")]
        public string TipoArea { get; set; }

        public virtual UsuarioInternoSetorEntity UsuarioInternoSetor { get; set; }

        public MotivoSituacaoInscricaoEntity()
        {
            HistoricoSituacaoInscricao = new HashSet<HistoricoSituacaoInscricaoEntity>();
        }
    }
}