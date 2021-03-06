using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class CodigoDescricaoInsumoDropDownVM : PagedOptions
	{
		public int Id { get; set; }
		public string Descricao { get; set; }
		public string Codigo { get; set; }
		public int IdProcessoProduto { get; set; }
	}
}
