using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ParidadeCambialGenerator : BaseDto
	{
		public DateTime DataParidade { get; set; }
		public DateTime DataParidadeInicio { get; set; }
		public DateTime DataOrigem { get; set; } // Se TipoGeracao=1, Data do arquivo - Se TipoGeracao=2, Data da cópia		
		public int Dias { get; set; }
		public bool AdicionaParidade { get; set; }
		public int TipoGeracao { get; set; } // 1-Baixar Arquivo, 2-Copiar Arquivo
		public int IndSobrepor { get; set; } // 0-Não, 1-Sim
		public int IndRetorno { get; set; } // 1: 
		public DateTime? DataParidadeProxima { get; set; }
		public string UrlPathArquivo { get; set; }
		public TimeSpan HoraMaximaDownload { get; set; }
		public int NumeroTentativasDownload { get; set; }
		public string EmailDestinatario { get; set; }
		public string TituloEmail { get; set; }
		public List<ParidadeCambialVM> ListaParidadeCambialAdd { get; set; }
		public List<ParidadeCambialVM> ListaParidadeCambialRemover { get; set; }
		public string Mensagem { get; set; }
		public int? IdParidadeCambialUltimaCopia { get; set; }
		public ParidadeCambialVM ParidadeCambialVM { get; set; }
		public int DiaAtual { get; set; }

	}
}