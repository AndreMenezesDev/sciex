using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class QuadroSocietarioVM
	{
		public string CnpjCpf { get; set; }
		public string DescricaoNaturezaJuridica { get; set; }
		public string DescricaoPais { get; set; }
		public string DescricaoQualificacao { get; set; }
		public int? IdNaturezaJuridica { get; set; }
		public int? IdPais { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public int? IdQualificacao { get; set; }
		public int? IdSocio { get; set; }
		public string Nome { get; set; }
		public string NomeSocial { get; set; }
		public EnumTipoPessoa TipoPessoa { get; set; }

		public EnumTipoSocio TipoSocio { get; set; }
		public double? ValorParticipacao { get; set; }
	}
}