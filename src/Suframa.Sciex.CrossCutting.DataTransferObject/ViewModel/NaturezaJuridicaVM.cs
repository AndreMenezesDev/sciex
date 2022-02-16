using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class NaturezaJuridicaVM
	{
		public string Descricao { get; set; }
		public int? IdNaturezaGrupo { get; set; }
		public int? IdNaturezaJuridica { get; set; }
		public IEnumerable<NaturezaQualificacaoVM> NaturezaQualificacoes { get; set; }
		public bool StatusQuadroSocial { get; set; }
	}
}