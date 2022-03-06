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
	public class PRCProdutoPaisVM : PagedOptions
	{
		public virtual PRCProdutoVM PrcProduto { get; set; }
		public virtual List<PRCDueVM> ListaPrcDue { get; set; }

		public int IdProdutoPais { get; set; }
		public int IdPrcProduto { get; set; }
		public decimal? QuantidadeAprovado { get; set; }
		public decimal? ValorDolarAprovado { get; set; }
		public decimal? QuantidadeComprovado { get; set; }
		public decimal? ValorDolarComprovado { get; set; }
		public int? CodigoPais { get; set; }
		public string DescricaoPais { get; set; }

	}


}
