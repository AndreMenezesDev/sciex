using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class CalcularMoedaVM
	{
		public decimal? Paridade { get; set; }
		public decimal? SaldoQuantidade { get; set; }
		public decimal? SaldoValorUS { get; set; }
		public decimal? AcrescimoUS { get; set; }
		public decimal? SaldoFinalUS { get; set; }
	}
}
