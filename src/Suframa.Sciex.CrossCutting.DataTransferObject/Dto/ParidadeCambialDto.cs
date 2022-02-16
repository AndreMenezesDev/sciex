using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ParidadeCambialDto : PagedOptions
	{
		public int? IdParidadeCambial { get; set; }
		public string DataParidade { get; set; }
		public string DataCadastro { get; set; }
		public string DataArquivo { get; set; }
		public string NomeUsuario { get; set; }
		public string CodDscMoeda { get; set; }
		public decimal Valor { get; set; }
		public int? Total { get; set; }
	}
}