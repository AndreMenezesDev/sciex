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
	public class PRCStatusVM : PagedOptions
	{
		//public ProcessoExportacaoVM Processo { get; set; }
		public int IdStatus { get; set; }
		public int IdProcesso { get; set; }
		public int? IdSolicitacaoAlteracao { get; set; }
		public string Tipo { get; set; }
		public DateTime? Data { get; set; }
		public DateTime? DataValidade { get; set; }
		public string CpfResponsavel { get; set; }
		public string NomeResponsavel { get; set; }
		public int? NumeroPlano { get; set; }
		public int? AnoPlano { get; set; }


		//complemento
		public string DescricaoStatus { get; set; }
		public string DescricaoPlanoExportacao { get; set; }
		public string DescricaoObservacao { get; set; }
		public string DescricaoTipo { get; set; }
		public string AnoNumeroSolicitacaoFormatado { get; set; }

		public string FiltroCertificado { get; set; }
		public int StatusProcessamento { get; set; }
		public int Aprovacao { get; set; }
		public string Mensagem { get; set; }

		
	}


}
