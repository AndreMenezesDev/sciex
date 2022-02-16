using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class LancamentoVM : PagedOptions
	{
		public int? IdLancamento { get; set; }
		public long IdPli { get; set; }
		public long IdPliMercadoria { get; set; }
		public short? IdCodigoLancamento { get; set; }
		public string Observacao { get; set; }
		public DateTime DataCadastro { get; set; }
		public string NumeroResponsavel { get; set; }
		public int CodigoUnidadeCadastradora { get; set; }
	}
}
