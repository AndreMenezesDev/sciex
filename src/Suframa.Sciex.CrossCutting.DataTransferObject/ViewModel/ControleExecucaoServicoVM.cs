using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ControleExecucaoServicoVM : PagedOptions
	{
		
		public int? IdControleExecucaoServico { get; set; }
		public int IdListaServico { get; set; }
		public DateTime DataHoraExecucaoInicio { get; set; }
		public DateTime? DataHoraExecucaoFim { get; set; }
		public string MemoObjetoEnvio { get; set; }
		public string MemoObjetoRetorno { get; set; }
		public int StatusExecucao { get; set; }
		public string NumeroCPFCNPJInteressado { get; set; }
		public string NomeCPFCNPJInteressado { get; set; }


	}
}
