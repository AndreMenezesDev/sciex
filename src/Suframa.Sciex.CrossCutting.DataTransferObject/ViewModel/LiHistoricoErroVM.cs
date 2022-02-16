using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class LiHistoricoErroVM : PagedOptions
	{
		public long IdPliMercadoria { get; set; }
		public long? IdLIArquivoRetorno { get; set; }
		public long IdLIHistoricoErro { get; set; }
		public byte? TipoLI { get; set; }
		public long? NumeroLI { get; set; }
		public long? NumeroLIProtocolo { get; set; }
		public DateTime? DataGeração { get; set; }
		public string MensagemErro { get; set; }
		public byte? CodigoDiagnosticoErro { get; set; }
		public DateTime? DataCadastro { get; set; }
		public DateTime? DataCancelamento { get; set; }
	}
}
