using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
	public class RequerimentoDto : BaseDto
	{
		public DateTime? DataFechamento { get; set; }
		public int? IdPessoaFisica { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public int? IdTipoRequerimento { get; set; }
		public int? IdTipoUsuario { get; set; }
		public int? IdUnidadeCadastradora { get; set; }
		public IEnumerable<int> StatusProtocolos { get; set; }
	}
}