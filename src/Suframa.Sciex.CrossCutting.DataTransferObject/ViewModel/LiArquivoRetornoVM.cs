using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class LiArquivoRetornoVM : PagedOptions
	{
		public long? IdLiArquivoRetorno { get; set; }
		public string NomeArquivo { get; set; }
		public DateTime? DataRecepcaoArquivo { get; set; }
		public byte?[] Arquivo { get; set; }
	}
}
