namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
	public class PessoaFisicaDto : BaseDto
	{
		public int ProtocoloEmAberto { get; set; }
		public int TipoRequerimento { get; set; }
		public int TotalCredenciamento { get; set; }
		public int TotalInscricao { get; set; }
		public int TotalPessoaFisica { get; set; }
		public int TotalRequerimento { get; set; }
	}
}