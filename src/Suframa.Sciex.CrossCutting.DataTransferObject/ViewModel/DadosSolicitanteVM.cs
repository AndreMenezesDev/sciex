namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DadosSolicitanteVM
	{
		public string Cpf { get; set; }
		public string Email { get; set; }
		public int? IdPessoaFisica { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public int? IdRequerimento { get; set; }
		public int? IdSolicitante { get; set; }
		public int? IdTipoRequerimento { get; set; }
		public bool IsPessoaFisicaInterna { get; set; }
		public bool IsPessoaJuridicaInterna { get; set; }
		public string Nome { get; set; }
		public string NomeSocial { get; set; }
		public int? Ramal { get; set; }
		public decimal? Telefone { get; set; }
	}
}