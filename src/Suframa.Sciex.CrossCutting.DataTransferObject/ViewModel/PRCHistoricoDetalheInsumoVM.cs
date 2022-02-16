using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PRCHistoricoDetalheInsumoVM : PagedOptions
	{
		#region CampoTabela
		public int IdDetalheHistoricoInsumo { get; set; }
		public int? IdPRCHistoricoInsumo { get; set; }
		public string DescricaoEvento { get; set; }
		public string DescricaoDetalhe { get; set; }
		public string TipoEvento { get; set; }
		public string DescricaoEventoDetalhe { get; set; }
		#endregion
	}
}
