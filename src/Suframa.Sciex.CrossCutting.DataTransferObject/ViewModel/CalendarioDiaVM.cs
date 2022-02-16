using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class CalendarioDiaVM
	{
		public DateTime DataAtendimento { get; set; }
		public DateTime? DataFinal { get; set; }
		public DateTime? DataInicial { get; set; }
		public int? Dia { get; set; }

		public int? DiaAtendimento
		{
			get
			{
				return (int?)DataAtendimento.Day ?? null;
			}
		}

		public List<CalendarioHoraVM> Horas { get; set; }
		public int? IdCalendarioDia { get; set; }
		public int? IdUnidadeCadastradora { get; set; }
		public int? IdUsuarioInterno { get; set; }
	}
}