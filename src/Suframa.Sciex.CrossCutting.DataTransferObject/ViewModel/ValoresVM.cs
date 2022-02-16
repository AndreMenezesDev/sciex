using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ValoresVM
	{
		public decimal ValorQuantidade { get; set; }
		public decimal ValorCondicao { get; set; }

		public string ValorTotalFormatado { get; set; }
		public decimal ValorTotalDecimal { get; set; }
	}
}
