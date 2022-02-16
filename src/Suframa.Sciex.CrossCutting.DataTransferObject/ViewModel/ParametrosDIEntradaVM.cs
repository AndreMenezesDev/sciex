using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ParametrosDIEntradaVM : PagedOptions
	{
		public int Identificador { get; set; }
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
	}
}
