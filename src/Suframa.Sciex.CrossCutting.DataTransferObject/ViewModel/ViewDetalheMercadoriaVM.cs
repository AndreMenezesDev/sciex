using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewDetalheMercadoriaVM : PagedOptions
	{
		public int IdDetalheMercadoria { get; set; }
		public short CodigoProduto { get; set; }
		public string CodigoNCMMercadoria { get; set; }
		public int CodigoDetalheMercadoria { get; set; }
		public string Descricao { get; set; }
		public short StatusDetalheMercadoria { get; set; }
		public short StatusItemCritico { get; set; }

		// Complemento de classe 
		public int? Id { get; set; }
	}
}
