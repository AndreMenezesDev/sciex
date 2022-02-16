namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class SolicitacaoPliProcessoAnuenteVM : PagedOptions
	{		
		public long IdPliProcessoAnuente { get; set; }
		public int IdOrgaoAnuente { get; set; }
		public long IdPliMercadoria { get; set; }
		public string NumeroProcesso { get; set; }
		public string SiglaOrgaoAnuente { get; set; }
	}
}
