using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_SERVICO")]
    public partial class ServicoEntity : BaseEntity
    {
        public virtual ICollection<AgendaAtendimentoEntity> AgendaAtendimento { get; set; }

        [Column("SRV_CO")]
        public int Codigo { get; set; }

        [Column("SRV_CO_SAC")]
        public int? CodigoSac { get; set; }

        [Required]
        [StringLength(30)]
        [Column("SRV_DS")]
        public string Descricao { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("SRV_ID")]
        public int IdServico { get; set; }

        public virtual ICollection<ParametroAnalistaServicoEntity> ParametroAnalistaServico { get; set; }

        [Column("SRV_QT_DIAS_ANALISE")]
        public int? QuantidadeDiasAnalise { get; set; }

        [Column("SRV_ST")]
        public bool Status { get; set; }

        public virtual ICollection<TipoRequerimentoEntity> TipoRequerimento { get; set; }

        [Column("SRV_VL_TX")]
        public double? ValorTaxa { get; set; }

        public ServicoEntity()
        {
            TipoRequerimento = new HashSet<TipoRequerimentoEntity>();
            AgendaAtendimento = new HashSet<AgendaAtendimentoEntity>();
            ParametroAnalistaServico = new HashSet<ParametroAnalistaServicoEntity>();
        }
    }
}