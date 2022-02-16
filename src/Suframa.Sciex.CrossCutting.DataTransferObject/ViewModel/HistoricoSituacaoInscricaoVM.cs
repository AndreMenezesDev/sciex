using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class HistoricoSituacaoInscricaoVM : PagedOptions
	{
		public DateTime? DataHoraFim { get; set; }

		public DateTime? DataHoraInicio { get; set; }

		public string DescricaoExplicacao { get; set; }

		public string DescricaoJustificativaFim { get; set; }

		public string DescricaoJustificativaInicio { get; set; }

		public string DescricaoMotivoSituacaoInscricao { get; set; }

		public string DescricaoOrientacao { get; set; }

		public string DescricaoSetorFim { get; set; }

		public string DescricaoSetorInicio { get; set; }

		public string DescricaoSituacaoInscricao { get; set; }

		public int IdHistoricoSituacaoInscricao { get; set; }

		public int? IdInscricaoCadastral { get; set; }

		public int? IdMotivoSituacaoInscricao { get; set; }

		public int? IdWorkflowSituacaoInscricao { get; set; }

		public bool IsBloquearDesbloquear { get; set; }

		public string NomeResponsavelFim { get; set; }

		public string NomeResponsavelInicio { get; set; }
	}
}