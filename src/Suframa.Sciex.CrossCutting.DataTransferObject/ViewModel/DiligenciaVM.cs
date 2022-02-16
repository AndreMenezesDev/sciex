using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DiligenciaVM : PagedOptions
	{
		public string AnalistaResponsavel { get; set; }
		public IEnumerable<DiligenciaAnexoVM> Arquivos { get; set; }
		public IEnumerable<DiligenciaAtividadeVM> Atividades { get; set; }
		public string CpfCnpj { get; set; }
		public DateTime? DataDiligencia { get; set; }
		public DateTime? DataDiligenciaAte { get; set; }
		public string Hora { get; set; }

		public string HoraDiligencia
		{
			get
			{
				if (!DataDiligencia.HasValue) { return ""; }

				return DataDiligencia.Value.ToString("HH:mm");
			}
		}

		public int? IdDiligencia { get; set; }
		public int? IdPessoaFisica { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public int? IdProtocolo { get; set; }
		public int? IdUnidadeCadastradora { get; set; }
		public int? IdUsuarioInterno { get; set; }
		public string Motivo { get; set; }
		public string NomeRazaoSocial { get; set; }
		public string Parecer { get; set; }
		public string PessoaResponsavel { get; set; }
		public EnumStatusDiligencia Status { get; set; }
		public bool StatusRealizada { get; set; }
	}
}