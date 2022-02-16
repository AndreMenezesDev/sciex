using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AliHistoricoVM : PagedOptions
	{
		public long? IdAliHistorico { get; set; }
		public long IdPliMercadoria { get; set; }
		public short? StatusAliAnterior { get; set; }
		public short? StatusLiAnterior { get; set; }
		public DateTime DataOperacao { get; set; }
		public string CPFCNPJResponsavel { get; set; }
		public string NomeResponsavel { get; set; }
		public string Observacao { get; set; }

		//complemente
		public string LoginResponsavel { get; set; }
		public string DataFormadata { get; set; }
		public string DescricaoStatus { get; set; }
	}
}