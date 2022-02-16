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
	public class LEInsumoErroVM : PagedOptions
	{
		public int IdLeInsumoErro { get; set; }
		public int IdLeInsumo { get; set; }
		public DateTime DataErroRegistro { get; set; }
		public string DescricaoErro { get; set; }
		public string CpfResponsavel { get; set; }
		public string NomeResponsavel { get; set; }

	}

}
