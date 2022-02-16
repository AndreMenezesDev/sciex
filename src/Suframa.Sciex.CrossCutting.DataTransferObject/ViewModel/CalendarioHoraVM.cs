using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class CalendarioHoraVM
    {
        public string Horario { get; set; }
        public DateTime? HorarioAtendimento { get; set; }
        public int? IdCalendarioAgendamento { get; set; }
        public int? IdCalendarioHora { get; set; }
        public bool IsSelecionado { get; set; }
    }
}