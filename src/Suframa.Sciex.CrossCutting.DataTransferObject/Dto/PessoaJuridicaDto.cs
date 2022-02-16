namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
	public class PessoaJuridicaDto : BaseDto
	{
		public bool ProtocoloEmAberto { get; set; }

		public bool RequerimentoExiste { get; set; }

		public int TotalCredenciamento { get; set; }

		public int? TotalEncontradoCnpj { get; set; }

		public int TotalInscricao { get; set; }

		public int TotalProtocoloEmAberto { get; set; }

		public int TotalRequerimento { get; set; }
	}
}