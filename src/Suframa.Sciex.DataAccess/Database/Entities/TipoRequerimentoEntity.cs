using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_TIPO_REQUERIMENTO")]
    public partial class TipoRequerimentoEntity : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column("TRE_DS")]
        public string Descricao { get; set; }

        [Column("SRV_ID")]
        [ForeignKey(nameof(Servico))]
        public int? IdServico { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("TRE_ID")]
        public int IdTipoRequerimento { get; set; }

        [Column("TUS_ID")]
        [ForeignKey(nameof(TipoUsuario))]
        public int? IdTipoUsuario { get; set; }

        public virtual ICollection<ListaDocumentoEntity> ListaDocumento { get; set; }

        public virtual ICollection<RequerimentoEntity> Requerimento { get; set; }

        public virtual ServicoEntity Servico { get; set; }

        [Required]
        [Column("TRE_ST_EXIGE_ANALISE")]
        public bool StatusExigeAnalise { get; set; }

        [Required]
        [Column("TRE_ST_COBRANCA")]
        public bool StatusTipoCobranca { get; set; }

        public virtual TipoUsuarioEntity TipoUsuario { get; set; }

        public TipoRequerimentoEntity()
        {
            ListaDocumento = new HashSet<ListaDocumentoEntity>();
            Requerimento = new HashSet<RequerimentoEntity>();
        }
    }
}