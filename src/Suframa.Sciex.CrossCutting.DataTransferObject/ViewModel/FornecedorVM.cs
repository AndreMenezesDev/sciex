using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class FornecedorVM : PagedOptions
	{

		public int? IdFornecedor { get; set; }
		public string Cidade { get; set; }
		public string CodigoPais { get; set; }
		public string DescricaoPais { get; set; }
		public string Complemento { get; set; }
		public string Estado { get; set; }
		public string Logradouro { get; set; }
		public string Numero { get; set; }
		public string RazaoSocial { get; set; }
		public string CNPJImportador { get; set; }
		public byte Ativo { get; set; }
		public int Codigo { get; set; }
		public byte[] RowVersion { get; set; }

		public int? Id { get; set; }
		public string Descricao { get; set; }
	}
}
