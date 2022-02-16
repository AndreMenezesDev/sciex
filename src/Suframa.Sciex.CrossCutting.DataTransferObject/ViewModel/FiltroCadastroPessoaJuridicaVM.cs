using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class FiltroCadastroPessoaJuridicaVM
	{
		public string Cnpj { get; set; }
		public int? IdNaturezaGrupo { get; set; }
		public int? IdNaturezaJuridica { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public int? IdRequerimento { get; set; }
		public bool IsQuadroSocietario { get; set; }
		public IEnumerable<NaturezaQualificacaoVM> NaturezaQualificacao { get; set; }
		public EnumTipoEntidadeRegistro TipoEntidadeRegistro { get; set; }
		public EnumTipoEstabelecimento TipoEstabelecimento { get; set; }
	}
}