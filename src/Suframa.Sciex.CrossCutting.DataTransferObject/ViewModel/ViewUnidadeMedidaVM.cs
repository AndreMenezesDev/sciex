using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewUnidadeMedidaVM : PagedOptions
	{
		public int IdUnidadeMedida { get; set; }
		public int CodigoSetor { get; set; }
		public string Descricao { get; set; }
		public string Sigla { get; set; }

		//complemento de classe
		public int? Id { get; set; }
	}
}
