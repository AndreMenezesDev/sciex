using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AnalisarPlanoExportacaoVM : ResultadoMensagemProcessamentoVM
	{
		public string TelaSolicitada { get; set; }
		public string DescricaoJustificativaErro { get; set; }
		public bool AcaoIsAprovar { get; set; }
		public int IdPEProdutoPais { get; set; }
		public int IdPEInsumo { get; set; }
		public int IdPEDetalheInsumo { get; set; }
		public int IdPlanoExportacao { get; set; }
	}

}
