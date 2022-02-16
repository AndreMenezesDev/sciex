using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class JulgamentoRecursoVM
	{
		public int? Ano { get; set; }

		public string CpfCnpj { get; set; }

		public string DescricaoPapel { get; set; }

		public int? IdPapel { get; set; }

		public int? IdProtocolo { get; set; }

		public int? IdRecurso { get; set; }

		public EnumStatusProtocolo IdStatusProtocolo { get; set; }

		public bool IsCoordenadorDescentralizado { get; set; }

		public bool IsCoordenadorGeral { get; set; }

		public bool IsSuperintendenteAdjunto { get; set; }

		public string Justificativa { get; set; }

		public string JustificativaIndeferimento { get; set; }

		public IEnumerable<string> MotivoIndeferimento { get; set; }

		public string NomeRazaoSocial { get; set; }

		public string NomeResponsavel { get; set; }

		public string NomeUsuarioInterno { get; set; }

		public string NumeroProtocolo
		{
			get
			{
				return Ano.HasValue && NumeroSequencial.HasValue ? NumeroSequencial.Value.ToString("D6") + "/" + Ano.Value : string.Empty;
			}
		}

		public int? NumeroSequencial { get; set; }

		public string ParecerCoordenador { get; set; }

		public string ParecerSuperintendente { get; set; }

		public IEnumerable<RecursoVM> Recursos { get; set; }

		public string TipoProtocolo { get; set; }
	}
}