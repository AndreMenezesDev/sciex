using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewSetorVM : PagedOptions
	{
		public int? IdSetor { get; set; }
		public int Codigo { get; set; }
		public string Descricao { get; set; }
		public int Tipo { get; set; }
		public string DescricaoObservacao { get; set; }
		public bool Status { get; set; }
		public DateTime? DataInclusao { get; set; }
		public DateTime? DataAlteracao { get; set; }
	}
}
