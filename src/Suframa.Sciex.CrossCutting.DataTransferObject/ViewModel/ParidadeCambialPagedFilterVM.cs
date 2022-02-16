using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ParidadeCambialPagedFilterVM : PagedOptions
	{
		public DateTime DataParidade { get; set; }
		public int IdMoeda { get; set; }
	}
}