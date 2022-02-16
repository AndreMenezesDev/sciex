using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class CodigoUtilizacaoVM : PagedOptions
	{
		public int? IdCodigoUtilizacao { get; set; }
		public string Descricao { get; set; }
		public byte Status { get; set; }
		public short Codigo { get; set; }
		public byte[] RowVersion { get; set; }

		public int? Id { get; set; }
		public byte Inativar { get; set; }
		public string MensagemErro { get; set; }
		public byte FiltrarCaptura { get; set; }

		public int? IdAplicacaoPli { get; set; }
	}
}