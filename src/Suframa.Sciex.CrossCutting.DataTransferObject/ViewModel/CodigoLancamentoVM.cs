
namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class CodigoLancamentoVM : PagedOptions
	{
		public short IdCodigoLancamento { get; set; } // O Id não deverá aceitar NULL. Esta VM será somente para leitura
		public string Descricao { get; set; }
		public byte Status { get; set; }
		public byte TipoOperacao { get; set; }
		public byte TipoLancamento { get; set; }
	}
}
