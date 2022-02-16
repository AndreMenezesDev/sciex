using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ErroProcessamentoVM : PagedOptions
	{
		public int? IdErroProcessamento { get; set; }
		public long? IdPli { get; set; }
		public long? IdSolicitacaoPli { get; set; }
		public short? IdErroMensagem { get; set; }
		public long? IdDiEntrada { get; set; }
		public byte? CodigoNivelErro { get; set; }
		public long? IdPliMercadoriaOuPliDetalheMercadoria { get; set; }
		public string Descricao { get; set; }
		public DateTime? DataProcessamento { get; set; }
		public string CNPJImportador { get; set; }
		public string NumeroPLIImportador { get; set; }
		public int? IdLote { get; set; }

		// Complemento de classe
		public string LocalErro { get; set; }
		public string MensagemErro { get; set; } 
		public string OrigemErro { get; set; }
		public long IdPliMercadoria { get; set; }
		public virtual int IdSolicitacaoLe { get; set; }
	}
}
