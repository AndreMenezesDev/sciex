using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewNcmVM : PagedOptions
	{
		public int IdNcm { get; set; }
		public string CodigoNCM { get; set; }
		public string Descricao { get; set; }
		public short IdNcmUnidadeMedida { get; set; }
		public short Status { get; set; }

		public int Id { get; set; }

		//complemento de class
		public string DescricaoUnidadeMedida { get; set; }
	}
}
