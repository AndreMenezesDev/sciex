namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
	public class FabricantePagedFilterVM : PagedOptions
	{
		public int? IdFabricante { get; set; }
		public string Codigo { get; set; }
		public string razaoSocial { get; set; }
		public byte[] RowVersion { get; set; }
	}
}