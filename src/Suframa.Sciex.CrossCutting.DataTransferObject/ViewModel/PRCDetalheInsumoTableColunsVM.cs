using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PRCDetalheInsumoTableColunsVM
	{
		//ESSA VM DEVE CONTER APENAS AS COLUNAS DA ENTITDADE PRCDetalheInsumoEntity!!!
		public int IdDetalheInsumo { get; set; }
		public int IdPrcInsumo { get; set; }
		public int? IdMoeda { get; set; }
		public int? NumeroSequencial { get; set; }
		public int? CodigoPais { get; set; }
		public decimal Quantidade { get; set; }
		public decimal? ValorUnitario { get; set; }
		public decimal? ValorFrete { get; set; }
		public decimal? ValorDolar { get; set; }
		public decimal? ValorUnitarioCFR { get; set; }
		public decimal? ValorDolarCFR { get; set; }
		public decimal? ValorDolarFOB { get; set; }
	}
}
