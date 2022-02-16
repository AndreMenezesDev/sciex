using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class CodigoDescricaoAlfandegaSetorArmazenamentoVM : PagedOptions
	{		
		public int CodigoRecintoAlfandega { get; set; }
		public int CodigoSetorArmazenamento { get; set; }
	}
}
