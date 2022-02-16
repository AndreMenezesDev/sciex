using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class RegimeTributarioTesteVM
	{
		public string Descricao { get; set; }
		public int? IdRegimeTributario { get; set; }
		public string Codigo { get; set; }
		public byte[] RowVersion { get; set; }
	}
}