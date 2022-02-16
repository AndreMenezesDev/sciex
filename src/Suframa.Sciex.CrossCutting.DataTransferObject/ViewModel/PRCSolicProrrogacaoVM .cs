using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PRCSolicProrrogacaoVM : PagedOptions
	{
		public int IdProcesso { get; set; }
		public int IdSolicitacaoProcesso { get; set; }
		public string JustificativaReprovado { get; set; }
		public DateTime Data { get; set; }
		public int Status { get; set; }
		public string NumeroCpfResponsavel { get; set; }
		public string NumeroResponsavel { get; set; }
		public string Justificativa { get; set; }



		//Complemento de Classe

		public bool Reprovacao { get; set;}

		
	}
}
