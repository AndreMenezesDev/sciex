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
	public class ParecerComplementarVM : PagedOptions
	{
		#region CampoTabela
		public int IdParecerComplementar { get; set; }
		public string DescricaoResolucao { get; set; }
		public string DescricaoConclusao { get; set; }
		#endregion
		#region Complemento
		#endregion
	}
}