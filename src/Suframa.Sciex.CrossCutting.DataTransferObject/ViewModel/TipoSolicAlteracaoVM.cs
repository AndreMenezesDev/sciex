using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TipoSolicAlteracaoVM : PagedOptions
	{
		public virtual IList<PRCSolicDetalheVM> ListaSolicDetalhe { get; set; }
		public int Id { get; set; }
		public string Descricao { get; set; }
	}
}
