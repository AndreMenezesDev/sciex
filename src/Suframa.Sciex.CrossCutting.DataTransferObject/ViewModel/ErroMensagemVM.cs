namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ErroMensagemVM : PagedOptions
	{
		public short IdErroMensagem { get; set; } // O Id não deverá aceitar NULL. Esta VM será somente para leitura
		public string Descricao { get; set; }
		public byte? CodigoFase { get; set; }
		public byte? CodigoSistemaOrigem { get; set; }

	}
}
