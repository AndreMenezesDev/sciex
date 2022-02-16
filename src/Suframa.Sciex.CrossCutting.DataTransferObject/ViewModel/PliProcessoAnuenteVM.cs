using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PliProcessoAnuenteVM : PagedOptions
	{
        public int? IdPliProcessoAnuente { get; set; }
        public long IdPliMercadoria { get; set; }
        public int IdOrgaoAnuente { get; set; }
        public string NumeroProcesso { get; set; }
		public string Descricao { get; set; }

		// complemento de classe
		public bool Excluir { get; set; }
		public string Sigla { get; set; }
	}
}
