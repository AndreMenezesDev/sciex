using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TaxaNCMBeneficioVM : PagedOptions
	{
		public int? IdTaxaNCMBeneficio { get; set; }
		public int IdNcm { get; set; }
		public string CodigoNCM { get; set; }
		public DateTime DataCadastro { get; set; }
		public int IdTaxaGrupoBeneficio { get; set; }
		public string DescricaoNCM { get; set; }

		//ATRIBUTOS PARA AUDITORIA
		public string DescricaoAlteracoes { get; set; }
		public int TipoAcao { get; set; }
		public string Justificativa { get; set; }
	}
}
