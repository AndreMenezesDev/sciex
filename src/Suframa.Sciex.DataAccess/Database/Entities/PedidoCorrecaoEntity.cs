using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_PEDIDO_CORRECAO")]
    public class PedidoCorrecaoEntity : BaseEntity
    {
        [Column("PCO_TP_ACAO")]
        public int Acao { get; set; }

        [Required]
        [Column("PCO_DS_CAMPO_DE")]
        public string CampoDe { get; set; }

        [Column("PCO_DS_CAMPO_PARA")]
        public string CampoPara { get; set; }

        public virtual CampoSistemaEntity CampoSistema { get; set; }

        [Column("PCO_DT_CORRECAO")]
        public DateTime? DataCorrecao { get; set; }

        [Column("PCO_DT_SOLICITACAO")]
        public DateTime? DataSolicitacao { get; set; }

        [Column("CAM_ID")]
        public int IdCampoSistema { get; set; }

        [Column("MPA_ID")]
        [ForeignKey(nameof(MensagemPadrao))]
        public int? IdMensagemPadrao { get; set; }

        [Key]
        [Column("PCO_ID")]
        public int IdPedidoCorrecao { get; set; }

        [Column("PRT_ID")]
        public int IdProtocolo { get; set; }

        [Column("PCO_ID_TABELA")]
        public int? IdTabela { get; set; }

        [Column("USI_ID")]
        public int? IdUsuarioSistema { get; set; }

        [StringLength(500)]
        [Column("PCO_DS_JUSTIFICATIVA")]
        public string Justificativa { get; set; }

        public virtual MensagemPadraoEntity MensagemPadrao { get; set; }

        [Column("PCO_ST_CORRECAO")]
        public int? Status { get; set; }
    }
}