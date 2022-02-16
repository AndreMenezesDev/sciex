using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewMunicipioVM : PagedOptions
	{
		public int IdMunicipio { get; set; }
		public decimal CodigoMunicipio { get; set; }
		public string Descricao { get; set; }
		public string SiglaUF { get; set; }
		public int? CodigoUF { get; set; }

		// Complemento de Classe
		public bool Checkbox { get; set; }
	}
}
