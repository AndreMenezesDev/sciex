using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class RequerimentoVM
	{
		public DateTime? DataFechamento { get; set; }
		public int? IdRequerimento { get; set; }
		public int? IdTipoRequerimento { get; set; }
		public int? IdUnidadeCadastradora { get; set; }
		public IEnumerable<ProtocoloVM> Protocolo { get; set; }
		public TipoRequerimentoVM TipoRequerimento { get; set; }
		public int? IdPessoaFisica { get; set; }
		public int? IdPessoaJuridica { get; set; }
	}
}