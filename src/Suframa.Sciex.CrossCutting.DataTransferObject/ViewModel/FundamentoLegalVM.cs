using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class FundamentoLegalVM : PagedOptions
	{
		public int? IdFundamentoLegal { get; set; }
		public string Descricao { get; set; }
		public short Codigo { get; set; }
		public short? TipoAreaBeneficio { get; set; }
		public byte[] RowVersion { get; set; }
		public int? Id { get; set; }
		public string DescricaoArea { get; set; }
	}
}