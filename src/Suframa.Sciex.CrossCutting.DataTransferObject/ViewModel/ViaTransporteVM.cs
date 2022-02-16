using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViaTransporteVM :PagedOptions
	{
		public int? IdViaTransporte { get; set; }
		public string Descricao { get; set; }
		public byte Status { get; set; }
		public short Codigo { get; set; }
		public byte[] RowVersion { get; set; }
	}
}
