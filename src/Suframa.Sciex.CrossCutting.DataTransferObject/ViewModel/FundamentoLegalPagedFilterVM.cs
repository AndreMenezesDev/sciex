namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class FundamentoLegalPagedFilterVM : PagedOptions
    {
        public int? IdFundamentoLegal { get; set; }
        public string Descricao { get; set; }
		public int Codigo { get; set; }
		public short? TipoAreaBeneficio { get; set; }
		public byte[] RowVersion { get; set; }
	}
}