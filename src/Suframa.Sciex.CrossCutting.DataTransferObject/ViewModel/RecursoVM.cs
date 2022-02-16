using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class RecursoVM
	{
		public DateTime? DataAlteracao { get; set; }
		public DateTime? DataInclusao { get; set; }
		public int? IdArquivo { get; set; }
		public int? IdPapel { get; set; }
		public int? IdProtocolo { get; set; }
		public int? IdRecurso { get; set; }
		public string NomeArquivo { get; set; }
		public string NomeResponsavel { get; set; }
	}
}