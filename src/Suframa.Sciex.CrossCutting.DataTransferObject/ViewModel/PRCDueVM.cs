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
	public class PRCDueVM : PagedOptions
	{
		public int IdDue { get; set; }
		public int? IdPRCProdutoPais { get; set; }
		public string Numero { get; set; }
		public DateTime DataAverbacao { get; set; }
		public decimal ValorDolar { get; set; }
		public decimal Quantidade { get; set; }
		public int CodigoPais { get; set; }

	}

	public class PRCDueComplementoVM : PRCDueVM
	{
		public PRCProdutoPaisVM PRCProdutoPais { get; set; }
		public int IdPRCProduto { get; set; }
		public string DescricaoPais { get; set; }
		public string DataAverbacaoFormatada { get; set; }
		public string SituacaoAnaliseString { get; set; }

	}

}
