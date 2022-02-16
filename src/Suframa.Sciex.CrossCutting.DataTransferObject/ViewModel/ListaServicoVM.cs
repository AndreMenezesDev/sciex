using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ListaServicoVM : PagedOptions
	{
		public int? IdListaServico { get; set; }
		public string Descricao { get; set; }
	}
}
