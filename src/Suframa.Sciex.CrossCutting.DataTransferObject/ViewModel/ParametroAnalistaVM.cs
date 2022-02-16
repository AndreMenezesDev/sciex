using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public partial class ParametroAnalistaVM : PagedOptions
	{
		public virtual ICollection<string> DescricaoServicos { get; set; }

		public int? IdParametroAnalista { get; set; }

		public ICollection<int> IdsServicos { get; set; }

		public int? IdUnidadeCadastradora { get; set; }

		public int? IdUsuarioInterno { get; set; }

		public int? IsStatusAtivoAgendamento { get; set; }

		public int? IsStatusAtivoProtocolo { get; set; }

		public string NomeUsuarioInterno { get; set; }

		public IEnumerable<ParametroAnalistaServicoVM> ParametroAnalistaServico { get; set; }
	}
}