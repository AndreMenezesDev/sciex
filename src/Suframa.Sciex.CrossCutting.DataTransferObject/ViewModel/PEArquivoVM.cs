using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PEArquivoVM : PagedOptions
	{
		public int IdPlanoExportacaoArquivo { get; set; }
		public int IdPlanoExportacao { get; set; }
		public string NomeArquivo { get; set; }
		public byte[] Anexo { get; set; }
	}
}
