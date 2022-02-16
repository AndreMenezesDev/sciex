using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class FabricanteVM : PagedOptions
	{

		public int? IdFabricante { get; set; }
		public string RazaoSocial { get; set; }
		public string Logradouro { get; set; }
		public string Numero { get; set; }
		public string Complemento { get; set; }
		public string Cidade { get; set; }
		public string Estado { get; set; }
		public string CodigoPais { get; set; }
		public string DescricaoPais { get; set; }
		public string CNPJImportador { get; set; }
		public byte[] RowVersion { get; set; }
		public int Codigo { get; set; }

		public int? Id { get; set; }
		public string Descricao { get; set; }
		public string CodigoDescricaoPaisConcatenado { get; set; }

	}
}