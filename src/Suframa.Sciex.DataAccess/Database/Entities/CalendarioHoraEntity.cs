using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_CALENDARIO_HORA")]
    public partial class CalendarioHoraEntity : BaseEntity
    {
        public virtual AgendaAtendimentoEntity AgendaAtendimento { get; set; }

        public virtual CalendarioDiaEntity CalendarioDia { get; set; }

        [Column("CAH_HR_ATENDIMENTO")]
        public DateTime? HorarioAtendimento { get; set; }

        [Column("AGD_ID")]
        [ForeignKey(nameof(AgendaAtendimento))]
        public int? IdAgendaAtendimento { get; set; }

        [Column("CAD_ID")]
        [ForeignKey(nameof(CalendarioDia))]
        public int? IdCalendarioDia { get; set; }

        [Key]
        [Column("CAH_ID")]
        public int IdCalendarioHora { get; set; }

        [Column("CAH_ST_HORARIO_ATENDIMENTO")]
        public int? Status { get; set; }
    }
}