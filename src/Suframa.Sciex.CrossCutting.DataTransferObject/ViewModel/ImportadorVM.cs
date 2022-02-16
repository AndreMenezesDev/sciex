using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ImportadorVM : PagedOptions
	{
		public int? IdImportador { get; set; }
		public decimal InscricaoCadastral { get; set; }
		public string CNPJ { get; set; }
		public string RazaoSocial { get; set; }
		public string CPFRepresentanteLegal { get; set; }
		public string CNAE { get; set; }
		public string CodigoPais { get; set; }
	}
}