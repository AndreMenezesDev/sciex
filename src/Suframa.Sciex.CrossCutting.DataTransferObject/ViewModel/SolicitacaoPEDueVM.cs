using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class SolicitacaoPEDueVM : PagedOptions
	{
		public int IdDue { get; set; }
		public int? IdProdutoPais { get; set; }
		public string Numero { get; set; }
		public DateTime? DataAverbacao { get; set; }
		public decimal? ValorDolar { get; set; }
		public decimal? Quantidade { get; set; }
		public int? CodigoPais { get; set; }
		public int? NumeroLote { get; set; }
		public int? NumeroAnoLote { get; set; }
		public int? CodigoProdutoExportacao { get; set; }
		public string InscricaoCadastral { get; set; }

	}
}
