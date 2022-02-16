using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
	public class SubClasseAtividadeDto : BaseDto
	{
		public int? Codigo { get; set; }
		public int? CodigoClasseAtividade { get; set; }
		public string CodigoCNAE { get; set; }
		public int? CodigoDivisaoAtividade { get; set; }
		public int? CodigoGrupoAtividade { get; set; }
		public DateTime? DataAlteracao { get; set; }
		public DateTime? DataInclusao { get; set; }
		public string Descricao { get; set; }
		public IEnumerable<string> DescricaoSetorEmpresarial { get; set; }
		public int? IdClasseAtividade { get; set; }
		public int? IdDivisaoAtividade { get; set; }
		public int? IdGrupoAtividade { get; set; }
		public int? IdSecaoAtividade { get; set; }
		public int? IdSetorEmpresarial { get; set; }
		public int? IdSubClasseAtividade { get; set; }
		public bool Status { get; set; }

		/// <summary>Utilizado para validar se a subclasse já foi utilizada</summary>
		public int? TotalSubClasseAtividade { get; set; }
	}
}