using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ProrrogarSolicitacaoVM
	{
		public DateTime? DataValidade { get; set; }
		public DateTime? DataValidadeProrrogada { get; set; }
		public int? IdProcesso { get; set; }
		public bool JaPossuiProrrogacao { get; set; }
		public string Justificativa { get; set; }

		public bool Sucesso { get; set; }
		public string MensagemErro { get; set; }
	}
}
