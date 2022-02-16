using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_TIPO_DOCUMENTO")]
    public partial class TipoDocumentoEntity : BaseEntity
    {
        [Required]
        [StringLength(18)]
        [Column("TDO_CO")]
        public string Codigo { get; set; }

        public virtual ICollection<ConferenciaDocumentoEntity> ConferenciaDocumento { get; set; }

        [Required]
        [StringLength(500)]
        [Column("TDO_DS")]
        public string Descricao { get; set; }

        [StringLength(200)]
        [Column("TDO_DS_FUND_LEGAL")]
        public string DescricaoLegal { get; set; }

        [Key]
        [Column("TDO_ID")]
        public int IdTipoDocumento { get; set; }

        [StringLength(500)]
        [Column("TDO_WW")]
        public string Link { get; set; }

        public virtual ICollection<ListaDocumentoEntity> ListaDocumento { get; set; }

        public virtual ICollection<RequerimentoDocumentoEntity> RequerimentoDocumento { get; set; }

        [Required]
        [Column("TDO_ST_INFO_COMPLEMENTAR")]
        public bool StatusInformacaoComplementar { get; set; }

        [Column("TDO_ST_OBRIGATORIO")]
        public bool StatusObrigatorio { get; set; }

        [Column("TDO_TP_ORIGEM", TypeName = "numeric")]
        public decimal TipoOrigem { get; set; }

        public TipoDocumentoEntity()
        {
            ListaDocumento = new HashSet<ListaDocumentoEntity>();
            RequerimentoDocumento = new HashSet<RequerimentoDocumentoEntity>();
            ConferenciaDocumento = new HashSet<ConferenciaDocumentoEntity>();
        }
    }
}