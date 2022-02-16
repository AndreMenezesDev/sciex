using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TaxaGrupoBeneficioVM : PagedOptions
	{
		public int? IdTaxaGrupoBeneficio { get; set; }
		public string Descricao { get; set; }
		public DateTime DataCadastro { get; set; }
		public decimal ValorPercentualReducao { get; set; }
		public short Codigo { get; set; }
		public string DescricaoAmparoLegal { get; set; }
		public short TipoBeneficio { get; set; }
		public string Justificativa { get; set; }
		public byte StatusBeneficio { get; set; }

		//ATRIBUTOS PARA AUDITORIA
		public string DescricaoAlteracoes { get; set; }
		public int TipoAcao { get; set; }

		//VALORES QUE SERÃO USADO NA IMPORTAÇÃO DO GRID
		public string PercentualConcatenado { get; set; }
		public string TipoBeneficioConcatenado { get; set; }
		public string DataConcatenada { get; set; }
	}
}
