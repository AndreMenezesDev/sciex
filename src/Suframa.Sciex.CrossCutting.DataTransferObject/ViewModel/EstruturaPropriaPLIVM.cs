using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class EstruturaPropriaPLIVM : PagedOptions
	{
		public long? IdEstruturaPropria { get; set; }
		public int? NumeroProtocolo { get; set; }
		public DateTime DataEnvio { get; set; }
		public DateTime? DataInicioProcessamento { get; set; }
		public DateTime? DataFimProcessamento { get; set; }
		public byte? StatusProcessamentoArquivo { get; set; }
		public short? QuantidadePLIArquivo { get; set; }
		public short? QuantidadePLIProcessadoSucesso { get; set; }
		public short? QuantidadePLIProcessadoFalha { get; set; }
		public string VersaoEstrutura { get; set; }
		public string LoginUsuarioEnvio { get; set; }
		public string NomeUsuarioEnvio { get; set; }
		public string CNPJImportador { get; set; }
		public string RazaoSocialImportador { get; set; }
		public int? InscricaoCadastral { get; set; }
		public string NomeArquivoEnvio { get; set; }
		public string ListaDePLI { get; set; }
		public byte? StatusPLITecnologiaAssistiva { get; set; }
		public string DescricaoPendenciaImportador { get; set; }
		public byte StatusUltimoArquivoProcessamento { get; set; }
		public decimal TipoArquivo { get; set; }

		// Complemento de Classe
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
		public List<SolicitacaoPliVM> ListaSolicitacao { get; set; }
		public string QuantidadePliConcatenado { get; set; }
		public string StatusValidacaoArquivoConcatenado { get; set; }

	}

}
