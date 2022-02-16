using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ValorFreteVM
	{
		public decimal? ValorParaCoeficienteTecnico { get; set; }
		public decimal? ValorPara { get; set; }
		public decimal? ValorDe { get; set; }
		public decimal? ValorDolarCFR { get; set; }
		public decimal? ValorUnitarioCFR { get; set; }
		public decimal? ValorQuantidade { get; set; }
		public decimal? QuantidadeSaldo { get; set; }
		public decimal? QuantidadeAdic { get; set; }
		public decimal? SaldoFinalUS { get; set; }
		public decimal? Acrescimo { get; set; }
		public decimal? ValorTotalFOB { get; set; }
		public decimal? ValorTotalCFR { get; set; }
		public decimal? ValorAprovado { get; set; }
		public decimal? Paridade { get; set; }
	}
}
