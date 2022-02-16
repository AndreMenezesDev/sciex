using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewRelatorioAnaliseProcessamentoPliVM : PagedOptions
	{

		public long IdPli { get; set; }
		public string NumeroPli { get; set; }
		public long? NumeroAli { get; set; }
		public long IdPliMercadoria { get; set; }
		public string NomenclaturaComumMercosul { get; set; }
		public string CodigoProduto { get; set; }
		public string TipoProduto { get; set; }
		public string ModeloProduto { get; set; }
		public int? Item { get; set; }
		public string DescricaoErroProcessamento { get; set; }
		public string Origem { get; set; }
		public int? QuantidadeErro { get; set; }
		public string Status { get; set; }

	}
}
