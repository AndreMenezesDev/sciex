using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AuditoriaVM : PagedOptions
	{
		public AuditoriaAplicacaoVM AuditoriaAplicacao { get; set; }

		public long IdAuditoria { get; set; }
		public int IdAuditoriaAplicacao { get; set; }
		public string CpfCnpjResponsavel { get; set; }
		public string NomeResponsavel { get; set; }
		public byte TipoAcao { get; set; }
		public DateTime DataHoraAcao { get; set; }
		public string DataHoraAcaoString { get; set; }
		public string DescricaoAcao { get; set; }
		public string Justificativa { get; set; }
		public long IdReferencia { get; set; }

	}
}