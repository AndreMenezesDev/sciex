using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AliEntradaArquivoVM : PagedOptions
	{
		public long IdAliEntradaArquivo { get; set; }
		public long IdAliArquivoEnvio { get; set; }
		public long IdPliMercadoria { get; set; }
		public DateTime? DataEnvioArquivoRetorno { get; set; }
	}
}
