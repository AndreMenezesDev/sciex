using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_CALENDARIO_AGENDAMENTO")]
    public partial class CalendarioAgendamentoEntity : BaseEntity
    {
        [Column("CAG_NU_ANO")]
        public int Ano { get; set; }

        public virtual ICollection<CalendarioDiaEntity> CalendarioDia { get; set; }

        [Key]
        [Column("CAG_ID")]
        public int IdCalendarioAgendamento { get; set; }

        [Column("UND_ID")]
        [ForeignKey(nameof(UnidadeCadastradora))]
        public int? IdUnidadeCadastradora { get; set; }

        [Column("CAG_NU_MES")]
        public int Mes { get; set; }

        [Column("CAG_ST")]
        public int? Status { get; set; }

        public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }

        public CalendarioAgendamentoEntity()
        {
            CalendarioDia = new HashSet<CalendarioDiaEntity>();
        }
    }
}