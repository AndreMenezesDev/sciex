using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class QuadrosAdministradoresVM
	{
		public bool HasQuadroSocietario { get; set; }
		public int? IdNaturezaJuridica { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public IEnumerable<NaturezaQualificacaoVM> NaturezaQualificacao { get; set; }
		public IEnumerable<QuadroAdministradoresVM> QuadrosAdministradores { get; set; }
	}
}