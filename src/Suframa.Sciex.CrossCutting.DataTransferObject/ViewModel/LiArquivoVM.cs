using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class LiArquivoVM : PagedOptions
	{
		public long? IdLiArquivo { get; set; }
		public byte[] Arquivo { get; set; }
	}
}
