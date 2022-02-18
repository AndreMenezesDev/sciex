using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AnalisePlanoExportacaoDUEVM : ResultadoMensagemProcessamentoVM
	{
		public int IdDue { get; set; }
		public bool AcaoIsAprovar { get; set; }
		public string DescricaoJustificativa { get; set; }
	}
}
