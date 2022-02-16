using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ManterSetorEmpresarialVM
	{
		public IEnumerable<AtividadeEconomicaVM> AtividadesEconomicasGrid { get; set; }
		public int Codigo { get; set; }
		public string Descricao { get; set; }
		public int? IdSetor { get; set; }
		public string Observacao { get; set; }
		public IEnumerable<SetorAtividadeVM> SetorAtividade { get; set; }
		public bool Status { get; set; }
		public int Tipo { get; set; }
	}
}