using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_PARAMETRO_ANALISTA")]
    public partial class ParametroAnalistaEntity : BaseEntity
    {
        [Key]
        [Column("PAN_ID")]
        public int IdParametroAnalista { get; set; }

        [ForeignKey(nameof(UnidadeCadastradora))]
        [Column("UND_ID")]
        public int? IdUnidadeCadastradora { get; set; }

        [ForeignKey(nameof(UsuarioInterno))]
        [Column("USI_ID")]
        public int? IdUsuarioInterno { get; set; }

        [Column("PAN_ST_AGENDAMENTO")]
        public int? IsStatusAtivoAgendamento { get; set; }

        [Column("PAN_ST_PROTOCOLO")]
        public int? IsStatusAtivoProtocolo { get; set; }

        public virtual ICollection<ParametroAnalistaServicoEntity> ParametroAnalistaServico { get; set; }

        public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }

        public virtual UsuarioInternoEntity UsuarioInterno { get; set; }

        public ParametroAnalistaEntity()
        {
            ParametroAnalistaServico = new HashSet<ParametroAnalistaServicoEntity>();
        }
    }
}