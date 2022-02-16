using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AuditoriaAplicacaoVM : PagedOptions
	{
		public IEnumerable<AuditoriaVM> ListaAuditoria { get; set; }
		public int IdAuditoriaAplicacao { get; set; }
		public byte CodigoAplicacao { get; set; }
		public string Descricao { get; set; }

	}
}