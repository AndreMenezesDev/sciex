using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PliMercadoriaVM : PagedOptions
	{		
		public long? IdPliMercadoria { get; set; }		
		public long IdPLI { get; set; }
		public long IdPLIAplicacao { get; set; }
		public int? IdMoeda { get; set; }
		public int? IdIncoterms { get; set; }
		public int? IdRegimeTributario { get; set; }
		public int? IdFabricante { get; set; }
		public int? IdFornecedor { get; set; }
		public int? IdFundamentoLegal { get; set; }
		public int? IdInstituicaoFinanceira { get; set; }
		public int? IdMotivo { get; set; }
		public int? IdModalidadePagamento { get; set; }
		public long? IdPliProduto { get; set; }
		public int? IdAladi { get; set; }
		public int? IdNaladi { get; set; }
		public int? IdURFEntrada { get; set; }
		public int? IdURFDespacho { get; set; }
		public string CodigoPais { get; set; }
		public string DescricaoPais { get; set; }
		public string CodigoPaisOrigemFabricante { get; set; }
		public string DescricaoPaisOrigemFabricante { get; set; }
		public decimal? PesoLiquido { get; set; }
		public decimal? QuantidadeUnidadeMedidaEstatistica { get; set; }
		public string NumeroComunicadoCompra { get; set; }
		public string NumeroAtoDrawback { get; set; }
		public string NumeroAgenciaSecex { get; set; }
		public decimal? ValorCRA { get; set; }
		public int? TipoCOBCambial { get; set; }
		public int? NumeroCOBCambialLimiteDiasPagamento { get; set; }
		public byte? TipoAcordoTarifario { get; set; }
		public string DescricaoInformacaoComplementar { get; set; }
		public byte? TipoBemEncomenda { get; set; }
		public byte? TipoMaterialUsado { get; set; }
		public string NumeroNCMDestaque { get; set; }
		public string CodigoNCMMercadoria { get; set; }
		public string DescricaoNCMMercadoria { get; set; }
		public short? TipoFornecedor { get; set; }
		public short? CodigoProduto { get; set; }
		public short? CodigoTipoProduto { get; set; }
		public short? CodigoModeloProduto { get; set; }
		public decimal? ValorTotalCondicaoVenda { get; set; }
		public decimal? ValorTotalCondicaoVendaReal { get; set; }
		public decimal? ValorTotalCondicaoVendaDolar { get; set; }
		public byte[] RowVersion { get; set; }
		public string DescricaoProduto { get; set; }
		public int? IdCodigoConta { get; set; }
		public int? IdCodigoUtilizacao { get; set; }


		//complemento de classe
		public int IdMercadoria { get; set; }	
		public string CodigoProdutoConcatenado { get; set; }		
		public string MensagemErro { get; set; }
		public int IdProdutoEmpresa { get; set; }
		public string IdMercadorias { get; set; }
		public List<PliDetalheMercadoriaVM> ListaPliDetalheMercadoriaVM { get; set; }
		public List<PliProcessoAnuenteVM> ListaPliProcessoAnuenteVM { get; set; }
	
		public ValidarDadosMercadoria ValidarDadosMercadoria { get; set; }
		public ValidarDetalhesMercadoria ValidarDetalhesMercadoria { get; set; }
		public ValidarFornecedorFabricanteMercadoria ValidarFornecedorFabricanteMercadoria { get; set; }
		public ValidarNegociacaoMercadoria ValidarNegociacaoMercadoria { get; set; }

		public Boolean AplicarParametros { get; set; }
		public int IdParametro { get; set; }
		public Boolean ParametroAplicado { get; set; }

		public Boolean IsValidarItemPli { get; set; }
		public Boolean ConfirmacaoClienteParametro { get; set; }
		public short? TipoAreaBeneficio { get; set; }

		public string ValorTotalCondicaoVendaFormatado { get; set; }
		public string ValorTotalCondicaoVendaRealFormatado { get; set; }
		public string ValorTotalCondicaoVendaDolarFormatado { get; set; }

		public int CodigoMoeda { get; set; }
		public string DescricaoMoeda { get; set; }
		public string CodigoDescricaoMoeda { get; set; }

		public string CodigoProdutoFormatado { get; set; }
		public string TipoProdutoFormatado { get; set; }
		public string ModeloProdutoFormatado { get; set; }

		public string CodigoIncoterms { get; set; }
		public string URFNaladi { get; set; }
		public string URFDespacho { get; set; }
		public string EntradaMercadoria { get; set; }

		public string DescricaoTipoFornecedor { get; set; }
		public string DescricaoFornecedor { get; set; }
		public long? NumeroALI { get; set; }
		public string CodigoDetalheMercadoria { get; set; }
		public byte StatusALI { get; set; }

		public string CodigoAladi { get; set; }
		public string DescricaoAladi { get; set; }
		public string CodigoRegimeTributario { get; set; }
		public string DescricaoRegimeTributario { get; set; }
		public string CodigoFundamentalLegal { get; set; }
		public string DescricaoFundamentalLegal { get; set; }
		public string TipoAcordoTarifarioDescricao { get; set; }
		public string InfCodigo { get; set; }
		public string InfDescricao { get; set; }
		public string TipoCOBCambialDescricao { get; set; }
		public string MopCodigo { get; set; }
		public string MopDescricao { get; set; }
		public string MotCodigo { get; set; }
		public string MotDescricao { get; set; }

		public string NumeroLiSERPRO { get; set; }
		public string TipoALI { get; set; }
		public string NumeroTransmissaoSERPRO { get; set; }
		public string DataALIFormatada { get; set; }
		public string DataDIFormatada { get; set; }
		public DateTime? DataALICancelada { get; set; }
		public string NumeroALISubstituida { get; set; }
		public string NumeroLISubstituida { get; set; }
		public string NumeroPLISubstituido { get; set; }
		public string CNPJ { get; set; }
		public string RazaoSocial { get; set; }
		public string NumeroPliConcatenado { get; set; }
		public string DataProcessamento { get; set; }
		public string NumeroLI { get; set; }
		public string NumeroLIRetificador { get; set; }
		public string DataGeracaoLI { get; set; }
		public string CodigoDescricaoPaisFornecedorConcatenado { get; set; }
		public string QuantidadeErroAli { get; set; }

		public int TipoOrigem { get; set; }
		public PliFornecedorFabricanteVM DadosFabricanteFornecedor { get; set; }

		public string PesoLiquidoString { get; set; }
		public string QuantidadeEstatisticaString { get; set; }
		public string DataALICanceladaFormatada { get; set; }

		public string UtilizadaDI { get; set; }
		public string NumeroDI { get; set; }

	}

	public class ValidarDadosMercadoria
	{
		public Boolean PesoLiquido { get; set; }
		public Boolean QuantidadeMedidaEstatistica { get; set; }
		public Boolean NumeroComunicadoCompra { get; set; }
		public Boolean Moeda { get; set; }
		public Boolean Incorterms { get; set; }
		public Boolean Pais { get; set; }
		public Boolean URFDespacho { get; set; }
		public Boolean URFEntrada { get; set; }
		public Boolean RegimeTributario { get; set; }
		public Boolean FundamentoLegal { get; set; }
		public int TotalItens { get; set; }
	}

	public class ValidarDetalhesMercadoria
	{
		public Boolean ItemMercadoria { get; set; }
		public Boolean UnidadeComercializada { get; set; }
		public Boolean QuantidadeUnidadeComercializada { get; set; }
		public Boolean ValorUnitarioCondicaoVenda { get; set; }		

		public int TotalItens { get; set; }
	}


	public class ValidarFornecedorFabricanteMercadoria
	{
		public Boolean TipoFornecedor { get; set; }
		public Boolean Fornecedor { get; set; }
		public Boolean LogradouroFornecedor { get; set; }
		public Boolean ComplementoFornecedor { get; set; }
		public Boolean NumeroFornecedor { get; set; }
		public Boolean CidadeFornecedor { get; set; }
		public Boolean EstadoFornecedor { get; set; }
		public int TotalItensFornecedor { get; set; }
		public Boolean Fabricante { get; set; }
		public Boolean LogradouroFabricante { get; set; }
		public Boolean ComplementoFabricante { get; set; }
		public Boolean NumeroFabricante { get; set; }
		public Boolean CidadeFabricante { get; set; }
		public Boolean EstadoFabricante { get; set; }
		public int TotalItensFabricante { get; set; }
		public Boolean PaisOrigem { get; set; }
		public int TotalItens { get; set; }
	}
	public class ValidarNegociacaoMercadoria
	{
		public Boolean TipoCobertura { get; set; }
		public Boolean ModalidadePagamento { get; set; }
		public Boolean Motivo { get; set; }
		public Boolean InstituicaoFinanceira { get; set; }
		public Boolean LimitePagamentoDias { get; set; }
		public int TotalItens { get; set; }
	}


}
