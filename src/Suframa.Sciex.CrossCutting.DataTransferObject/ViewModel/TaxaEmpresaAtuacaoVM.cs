using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TaxaEmpresaAtuacaoVM : PagedOptions
	{		
		public int IdTaxaEmpresaAtuacao { get; set; }		
		public string CNPJ { get; set; }
		public DateTime DataCadastro { get; set; }
		public short Codigo { get; set; }
		public string RazaoSocial { get; set; }

	}
}
