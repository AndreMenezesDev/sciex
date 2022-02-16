using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewSetorEmpresaVM : PagedOptions
	{
		public int? IdSetor { get; set; }
		public int Codigo { get; set; }
		public string Descricao { get; set; }
		public string Cnpj { get; set; }
	}
}
