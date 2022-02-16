using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DIArquivoEntradaVM : PagedOptions
	{
		public long Id { get; set; }
		public DateTime DataHoraRecepcao { get; set; }
		public Byte SituacaoLeitura { get; set; }
		public Int16 QuantidadeDi { get; set; }
		public Int16 QuantidadeDiProcessada { get; set; }
		public Int16 QuantidadeDiErro { get; set; }
		public DateTime? DataHoraInicioProcesso { get; set; }
		public DateTime? DataHoraFimProcesso { get; set; }

		public string NomeArquivo { get; set; } = "SERPRODI.FILE";
		public string quantidadeDiConcatenado { get; set; }
		public string DescricaoSituacaoLeitura { get; set; }
		public string DataRecepcaoFormatada { get; set; }

		public DIArquivoVM  DiArquivo { get; set; }

	}
}
