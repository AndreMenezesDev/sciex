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
	public class PliVM : PagedOptions
	{
		public long? IdPLI { get; set; }
		public int IdPLIAplicacao { get; set; }
		public byte TipoDocumento { get; set; }
		public long NumeroPli { get; set; }
		public int Ano { get; set; }
		public string Cnpj { get; set; }
		public int? InscricaoCadastral { get; set; }
		public short? StatusAnaliseVisual { get; set; }
		public short? StatusDistribuicao { get; set; }
		public decimal? ValorTCIF { get; set; }
		public decimal? ValorTECIFItens { get; set; }
		public int? Debito { get; set; }
		public short? DebitoAno { get; set; }
		public DateTime? DataDebitoPagamento { get; set; }
		public DateTime? DataDebitoGeracao { get; set; }
		public string NumeroLIReferencia { get; set; }
		public long IdLiReferencia { get; set; }
		public string NumeroALIReferencia { get; set; }
		public string NumeroDIReferencia { get; set; }
		public int? NumeroPEXPAM { get; set; }
		public short? AnoPEXPAM { get; set; }
		public string LotePEXPAM { get; set; }
		public string MEALIArquivo { get; set; }
		public byte? TipoOrigem { get; set; }
		public string NumeroResponsavelRegistro { get; set; }
		public string NomeResponsavelRegistro { get; set; }
		public DateTime DataCadastro { get; set; }
		public DateTime? DataEnvioPli { get; set; }
		public string NumCPFRepLegalSISCO { get; set; }
		public string CodigoCNAE { get; set; }
		public int CodigoSetor { get; set; }
		public string DescricaoSetor { get; set; }
		public decimal? ValorTotalCondicaoVenda { get; set; }
		public decimal? ValorTotalCondicaoVendaReal { get; set; }
		public decimal? ValorTotalCondicaoVendaDolar { get; set; }
		public byte[] RowVersion { get; set; }
		public string RazaoSocial { get; set; }
		public byte StatusPli { get; set; }
		public byte? StatusPliProcessamento { get; set; }
		public string DataProcessamento { get; set; }		
		public string NumeroPliImportador { get; set; }		
		public byte? StatusPliTecnologiaAssistiva { get; set; }		
		public string StatusIndicacaoPliExigencia { get; set; }
		public long? IdEstruturaPropria { get; set; }

		/* complemento da classe */
		public string DescricaoDebito { get; set; }
		public string Situacao { get; set; }
		public string DescricaoValorGeralTcif { get; set; }
		public string DescricaoAplicacao { get; set; }
		public string DescricaoTipoDocumento { get; set; }
		public string DescricaoStatus { get; set; }
		public string DescricaoTipoPli { get; set; }
		public int CodigoPliAplicao { get; set; }
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
		public string Mensagem { get; set; }
		public Boolean CopiaPli { get; set; }
		public string NumeroPliFormatado { get; set; }
		public string NumeroPliConcatenado { get; set; }
		public string DataPliFormatada { get; set; }
		public string DataDiFormatada { get; set; }
		public string DataEnvioPliFormatada { get; set; }
		public int ConsultarPli { get; set; }
		public int StatusTaxa { get; set; }
		public string DescricaoHistoricoPli { get; set; }
		public int QuantidadeErroProcessamento { get; set; }
		public List<long> ListaPli { get; set; }
		public int StatusPliSelecionado { get; set; }
		public string NomeAnexo { get; set; }
		public byte[] Anexo { get; set; }

		//DI
		public DiVM Di { get; set; }
		public string IdDI { get; set; }
		public string UtilizadaDI { get; set; }
		public string NumeroDI { get; set; }
		public string SetorSelect { get; set; }
		public long? IdPliAnalise { get; set; }
		public Decimal? StatusPliAnalise { get; set; }
		public string StatusPliAnaliseFormatado { get; set; }
		public string Motivo { get; set; }
		public int? IdUtilizacao { get; set; }
		public int? IdConta { get; set; }
		public string CodigoUtilizacaoFormatada { get; set; }
		public string CodigoContaFormatada { get; set; }
		public string TemProjetoAprovado { get; set; }

		//PliAnalise Visual

		public decimal? AnaliseVisualStatus{ get; set; }
		public string AnaliseVisualStatusFormatado { get; set; }
		public string DescricaoMotivo { get; set; }
		public int? IdCodigoUtilizacao { get; set; }
		public int? IdCodigoConta { get; set; }
		public DateTime? DataAnalise { get; set; }
		public DateTime? DataPendencia { get; set; }
		public string DescricaoResposta { get; set; }
		public string AnaliseVisualNomeAnexo { get; set; }
		public byte[] AnaliseVisualAnexo { get; set; }
		public string NomeAnalistaPendencia { get; set; }
		public string MotivoPendencia { get; set; }
		public string LocalPastaEstruturaArquivo { get; set; }
		public string DataPendenciaFormatada { get; set; }
		public string DataAnaliseFormatada { get; set; }
		//
		//DesignarPli
		public int? IdAnalistaDesignado { get; set; }
		public string AnalistaDesignado { get; set; }

		//

		public List<PliMercadoriaVM> ListaMercadorias { get; set; }
		public Boolean ValidarPli { get; set; }
		public Boolean IsPliValidado { get; set; }
		public Boolean IsMercadorias { get; set; }
		public long IdPliProduto { get; set; }
		public string MensagemErro { get; set; }
		public int TipoErro { get; set; }
		public bool EntregarPli { get; set; }
		public int CodigoPLIStatus { get; set; }

		public string Endereco { get; set; }
		public string Numero { get; set; }
		public string Complemento { get; set; }
		public string Bairro { get; set; }
		public string CodigoMunicipio { get; set; }
		public string Municipio { get; set; }
		public string UF { get; set; }
		public string CEP { get; set; }
		public string DescricaoCNAE { get; set; }
		public string PaisCodigo { get; set; }
		public string PaisDescricao { get; set; }
		public string Telefone { get; set; }
		public string CodigoUtilizacao { get; set; }
		public string DescricaoUtilizacao { get; set; }
		public string CodigoConta { get; set; }
		public string DescricaoConta { get; set; }
		public string CodigoProduto { get; set; }
		public int QuantidadeErro { get; set; }

		public int QuantidadeMercadorias { get; set; }
		public string ValorTotalDolarMercadorias { get; set; }
		public string ValorTotalRealMercadorias { get; set; }
		public byte TemDiDesembaracada { get; set; }
		public List<long> ListaSelecionados { get; set; }
		public string Observacao { get; set; }
		public byte EnviadoAoSiscomex { get; set; }
		public byte RespondidoPeloSiscomex { get; set; }
		public long IdPliMercadoria { get; set; }
		public byte StatusALI { get; set; }
		public byte StatusArquivoALI { get; set; }
		public bool TemALIIndeferida { get; set; }
		//-----------------------------
		public long? NumeroALISubstitutiva { get; set; }
		public int? NumeroLISubstitutivo { get; set; }
		public long? NumeroPLISubstitutivo { get; set; }
		public long IdPLISubstitutivo { get; set; }
		public long IdPliMercadoriaSubstitutivo { get; set; }
		public string NumeroPliSubstitutivoConcatenado { get; set; }
		public int? AnoPliSubstitutivo { get; set; }

	}

}
