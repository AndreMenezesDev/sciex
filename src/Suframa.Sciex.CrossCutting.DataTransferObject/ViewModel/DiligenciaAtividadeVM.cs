using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DiligenciaAtividadeVM
	{
		public string CodigoSubclasse { get; set; }
		public string CodigoSubclasseAtividade { get; set; }
		public string DescricaoSubclasse { get; set; }
		public int? IdDiligencia { get; set; }
		public int? IdDiligenciaAtividade { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public int? IdProtocolo { get; set; }
		public bool IsAtividadeExercida { get; set; }
		public IEnumerable<DiligenciaAtividadeSetorVM> Setor { get; set; }
	}
}