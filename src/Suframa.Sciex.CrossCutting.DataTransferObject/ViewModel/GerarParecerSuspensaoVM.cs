namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class GerarParecerSuspensaoVM : PagedOptions
	{
		public string Hash { get; set; }
		public int IdProcesso { get; set; }
		public int IdSolicitacaoAlteracao { get; set; }
		public decimal? QtdProdutoAprov { get; set; }
		public decimal? ValorProdutoAprov { get; set; }
	}
}
