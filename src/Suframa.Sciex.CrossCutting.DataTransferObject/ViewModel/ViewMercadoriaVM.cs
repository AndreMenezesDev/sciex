using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewMercadoriaVM : PagedOptions
	{

		public int IdMercadoria { get; set; }
		public short CodigoProdutoMercadoria { get; set; }
		public string CodigoNCMMercadoria { get; set; }
		public string Descricao { get; set; }
		public short StatusMercadoria { get; set; }

		// complemento de class
		public int? Id { get; set; }
		
	}
}
