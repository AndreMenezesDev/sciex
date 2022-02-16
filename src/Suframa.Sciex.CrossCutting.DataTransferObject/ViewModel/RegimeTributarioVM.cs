using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class RegimeTributarioVM: PagedOptions
	{
		public string Descricao { get; set; }
		public int? IdRegimeTributario { get; set; }
		public short Codigo { get; set; }
		public byte[] RowVersion { get; set; }

		public int? Id { get; set; }

	}
}