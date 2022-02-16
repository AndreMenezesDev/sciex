using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class EstruturaPropriaLEVM : PagedOptions
	{
		public long? IdEstruturaPropria { get; set; }
		public int NumeroProtocolo { get; set; }
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
		public byte[] Arquivo { get; set; }

		// Complemento de Classe
		public string NomeArquivo { get; set; }
		public string LocalPastaEstruturaArquivo { get; set; }
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
		public List<SolicitacaoPliVM> ListaSolicitacao { get; set; }
		public string QuantidadePliConcatenado { get; set; }
		public string StatusValidacaoArquivoConcatenado { get; set; }

		// complemento de le
		public int IdCodigoProdutoSuframa { get; set; }
		public decimal CodigoProduto { get; set; }
		public string DescricaoProduto { get; set; }
		public int IdCodigoTipoProduto { get; set; }
		public decimal CodigoTipoProduto { get; set; }
		public string DescricaoTipoProduto { get; set; }
		public int IdCodigoNCM { get; set; }
		public string CodigoNCM { get; set; }
		public string DescricaoNCM { get; set; }
		public int IdCodigoUnidadeMedida { get; set; }
		public short CodigoUnidadeMedida { get; set; }
		public string DescricaoModelo { get; set; }
		public string DescricaoModeloExportador { get; set; }
		public string DescricaoCentroCusto { get; set; }
		List<SolicitacaoLeInsumoVM> listaSolicitacaoLeInsumo { get; set; }

		// Complemento de Classe
		public string DataValidacao { get; set; }
		public int QtdErrosPli { get; set; }
		public int QtdSucessoPli { get; set; }
		public List<ErroProcessamentoVM> ListaErros { get; set; }
		public string StatusSolicitacaoNome { get; set; }
		public string NumeroPliSuframa { get; set; }

		public long? IdPLI { get; set; }
		public long? NumeroPLI { get; set; }
		public long? AnoPLI { get; set; }

	}

}
