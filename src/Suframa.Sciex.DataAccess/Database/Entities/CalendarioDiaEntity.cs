using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_CALENDARIO_DIA")]
    public partial class CalendarioDiaEntity : BaseEntity
    {
        public virtual CalendarioAgendamentoEntity CalendarioAgendamento { get; set; }

        public virtual ICollection<CalendarioHoraEntity> CalendarioHora { get; set; }

        [Column("CAD_DT_ATENDIMENTO")]
        public DateTime? DataAtendimento { get; set; }

        [Column("CAG_ID")]
        [ForeignKey(nameof(CalendarioAgendamento))]
        public int? IdCalendarioAgendamento { get; set; }

        [Key]
        [Column("CAD_ID")]
        public int IdCalendarioDia { get; set; }

        [Column("USI_ID")]
        public int? IdUsuario { get; set; }

        public CalendarioDiaEntity()
        {
            CalendarioHora = new HashSet<CalendarioHoraEntity>();
        }
    }
}