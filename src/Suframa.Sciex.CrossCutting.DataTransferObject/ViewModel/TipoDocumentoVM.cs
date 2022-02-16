using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TipoDocumentoVM
	{
		public string Descricao { get; set; }
		public int? IdProtocolo { get; set; }
		public int IdTipoDocumento { get; set; }
		public int? IdTipoRequerimento { get; set; }
		public bool IsObrigatorio { get; set; }
		public bool IsSelecionado { get; set; }
		public string Link { get; set; }
		public bool StatusInformacaoComplementar { get; set; }
		public EnumTipoOrigemDocumento TipoOrigem { get; set; }
	}
}