using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AtividadePessoaJuridicaVM
	{
		public string CodigoSetor { get; set; }
		public string CodigoSubclasse { get; set; }
		public string CodigoSubClasseAtividade { get; set; }
		public string[] DescricaoSetorEmpresarial { get; set; }
		public string DescricaoSubClasseAtividade { get; set; }
		public int? IdAtividade { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public int IdSubClasseAtividade { get; set; }
		public bool StatusAtuante { get; set; }
		public EnumTipoAtividade Tipo { get; set; }
		public string TipoAtividade { get; set; }
	}
}