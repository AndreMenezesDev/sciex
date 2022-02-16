using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class VisualizarCadastroVM
	{
		public int? IdPessoaFisica { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public int? IdProtocolo { get; set; }
		public int? IdRequerimento { get; set; }
		public bool IsCredenciamentoAuditor { get; set; }
		public bool IsCredenciamentoTransportador { get; set; }
		public bool IsInscricaoCadastral { get; set; }
		public bool IsQuadroSocietario { get; set; }
	}
}