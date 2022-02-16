using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DocumentoComprobatorioVM
	{
		public DateTime? DataExpedicao { get; set; }
		public DateTime? DataInclusao { get; set; }
		public DateTime? DataVencimento { get; set; }
		public string DescricaoOrigem { get; set; }
		public string DescricaoTipoDocumento { get; set; }
		public string HoraExpedicao { get; set; }
		public int? IdArquivo { get; set; }
		public int? IdProtocolo { get; set; }
		public int? IdRequerimento { get; set; }
		public int? IdRequerimentoDocumento { get; set; }
		public int? IdTipoDocumento { get; set; }
		public bool IsQuadroSocietario { get; set; }
		public bool IsStatusInfoComplementar { get; set; }
		public string NomeArquivo { get; set; }
		public string NumeroCertidao { get; set; }
		public EnumStatus Status { get; set; }
		public EnumTipoOrigemDocumento TipoOrigem { get; set; }
	}
}