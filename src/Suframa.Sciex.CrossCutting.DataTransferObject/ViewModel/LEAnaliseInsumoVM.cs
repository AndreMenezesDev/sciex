using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class LEAnaliseInsumoVM : PagedOptions
	{
		public LEInsumoVM InsumoOriginal { get; set; }
		public LEInsumoVM InsumoAlterado { get; set; }
	}

}
