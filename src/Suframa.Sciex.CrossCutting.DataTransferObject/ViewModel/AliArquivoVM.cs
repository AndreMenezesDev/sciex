using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AliArquivoVM : PagedOptions
	{
		public long? IdAliArquivo { get; set; }
		public byte[] Arquivo { get; set; }
	}

}
