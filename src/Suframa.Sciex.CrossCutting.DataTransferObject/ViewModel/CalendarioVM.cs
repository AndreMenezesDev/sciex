using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class CalendarioVM
	{
		public string Color { get; set; }
		public string Feriado { get; set; }
		public int? IdUsuarioInterno { get; set; }
		public DateTime Start { get; set; }
		public string Title { get; set; }
	}
}