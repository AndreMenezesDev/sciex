using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class EnviaRecursoVM
	{
		public int? Ano { get; set; }
		public IEnumerable<ArquivoVM> Arquivos { get; set; }
		public string DescricaoServico { get; set; }
		public string DescricaoUnidadeCadastradora { get; set; }
		public IEnumerable<string> Documentos { get; set; }
		public int? IdPapel { get; set; }
		public int? IdProtocolo { get; set; }
		public bool IsConferenciaAdministrativa { get; set; }
		public bool IsIndeferidoAguardandoRecurso { get; set; }
		public string Justificativa { get; set; }
		public string JustificativaIndeferimento { get; set; }
		public IEnumerable<string> MotivoIndeferimento { get; set; }

		public string NumeroProtocolo
		{
			get
			{
				if (NumeroSequencial.HasValue && Ano.HasValue)
				{
					return (NumeroSequencial.Value.ToString("D6") + "/" + Ano.Value).ToString();
				}
				return "";
			}
		}

		public int? NumeroSequencial { get; set; }
	}
}