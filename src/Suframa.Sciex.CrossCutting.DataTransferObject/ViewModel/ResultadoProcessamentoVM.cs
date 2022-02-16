using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ResultadoMensagemProcessamentoVM
	{
		public bool Resultado { get; set; }
		public string Mensagem { get; set; }
		public int CodigoErro { get; set; }
	}

	public class ResultadoCorrigirDetalheInsumoVM : ResultadoMensagemProcessamentoVM
	{
		public int? IdNovoInsumo { get; set; }
	}
}
