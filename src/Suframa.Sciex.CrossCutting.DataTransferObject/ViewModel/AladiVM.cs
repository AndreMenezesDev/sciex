namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AladiVM : PagedOptions
	{		
		public int? IdAladi { get; set; }
		public short Codigo { get; set; }
		public string Descricao { get; set; }		
		public byte[] RowVersion { get; set; }
		public int? Id { get; set; }

	}
}
