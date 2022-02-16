using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ResultadoAnaliseVM
	{
		public IEnumerable<WorkflowProtocoloVM> ConferenciasAdministrativas { get; set; }
		public IEnumerable<ConsultaPublicaVM> ConsultasPublica { get; set; }
		public IEnumerable<WorkflowProtocoloVM> Diligencias { get; set; }
		public int? IdProtocolo { get; set; }
		public bool IsConsultaPublicaPendente { get; set; }
		public bool IsDiligenciaAberta { get; set; }
		public bool IsStatusEmAberto { get; set; }
		public IEnumerable<ResumoVM> PedidosAtualizar { get; set; }
		public IEnumerable<ResumoVM> PedidosCorrigir { get; set; }
	}
}