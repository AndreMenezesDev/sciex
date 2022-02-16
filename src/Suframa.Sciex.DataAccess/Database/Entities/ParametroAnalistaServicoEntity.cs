using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_PARAMETRO_ANALISTA_SERVICO")]
    public partial class ParametroAnalistaServicoEntity : BaseEntity
    {
        [ForeignKey(nameof(ParametroAnalista))]
        [Column("PAN_ID")]
        public int IdParametroAnalista { get; set; }

        [Key]
        [Column("PAS_ID")]
        public int IdParametroAnalistaServico { get; set; }

        [ForeignKey(nameof(Servico))]
        [Column("SRV_ID")]
        public int IdServico { get; set; }

        public virtual ParametroAnalistaEntity ParametroAnalista { get; set; }

        public virtual ServicoEntity Servico { get; set; }
    }
}