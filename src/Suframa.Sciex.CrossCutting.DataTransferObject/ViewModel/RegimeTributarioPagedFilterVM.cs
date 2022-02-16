namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class RegimeTributarioPagedFilterVM : PagedOptions
    {
        public int? IdRegimeTributario { get; set; }
        public string descricao { get; set; }
		public string codigo { get; set; }
		public byte[] RowVersion { get; set; }
	}
}