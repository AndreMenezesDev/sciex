using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ParidadeCambialVM
	{
		public int? IdParidadeCambial { get; set; }
		public DateTime? DataParidade { get; set; }
		public DateTime? DataCadastro { get; set; }
		public DateTime? DataArquivo { get; set; }
		public string NomeUsuario { get; set; }
		public string NumeroUsuario { get; set; }
	}
}