namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class SolicitacaoFornecedorFabricanteVM : PagedOptions
	{		
		public long IdSolicitacaoFornecedorFabricante { get; set; }
		public long? IdSolicitacaoPliMercadoria { get; set; }
		public string DescricaoFornecedor { get; set; }		
		public string LogradouroFornecedor { get; set; }		
		public string NumeroFornecedor { get; set; }
		public string ComplementoFornecedor { get; set; }		
		public string CidadeFornecedor { get; set; }		
		public string EstadoFornecedor { get; set; }		
		public string CodigoPaisFornecedor { get; set; }		
		public byte? CodigoAusenciaFabricante { get; set; }		
		public string DescricaoFabricante { get; set; }		
		public string LogradouroFabricante { get; set; }
		public string NumeroFabricante { get; set; }
		public string ComplementoFabricante { get; set; }		
		public string CidadeFabricante { get; set; }
		public string EstadoFabricante { get; set; }
		public string CodigoPaisFabricante { get; set; }

	}
}
