using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AnalistaVM : PagedOptions
	{		
		public int? IdAnalista { get; set; }
		public string CPF { get; set; }				
		public string Nome { get; set; }		
		public string SiglaSetor { get; set; }
		public string DescricaoSetor { get; set; }
		public DateTime DataHoraSincronizacao { get; set; }
		public byte? SituacaoVisual { get; set; }
		public byte? SituacaoVisualSetada { get; set; }
		public byte? SituacaoLE { get; set; }
		public byte? SituacaoLESetado { get; set; }
		public byte? SituacaoPlano { get; set; }
		public byte? SituacaoPlanoSetado { get; set; }
		public byte? Solicitacao { get; set; }
		public byte? SolicitacaoSetado { get; set; }

		//Complemento de classe
		public string Path { get; set; }
		public int QuantidadePLI { get; set; }
		public int QuantidadeLE { get; set; }
		public int QuantidadePE { get; set; }
		public int SolicAlteracaoProcesso { get; set; }
		public string Mensagem { get; set; }
		
	}
}
