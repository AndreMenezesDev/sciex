using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class SolicitacaoPELoteVM : PagedOptions
	{
		public int Id { get; set; }
		public int? NumeroLote { get; set; }
		public string TipoModalidade { get; set; }
		public string TipoExportacao { get; set; }
		public int Situacao { get; set; }
		public string InscricaoCadastral { get; set; }
		public string NumeroCNPJ { get; set; }
		public string RazaoSocial { get; set; }
		public int? Ano { get; set; }
		public string NumeroProcesso { get; set; }
		public int? AnoProcesso { get; set; }
		public string InscricaoCadastralExportador { get; set; }
		public long EspId { get; set; }

		//Campos Complementares
		public string DataValidacao { get; set; }
		public int QtdErros { get; set; }
		public string DescricaoUnidade { get; set; }

	}
}
