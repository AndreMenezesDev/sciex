using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class SolicitacaoPliMercadoriaVM
	{
		public long IdSolicitacaoPliMercadoria { get; set; }
		public decimal? PesoLiquido { get; set; }
		public decimal? QuantidadeUnidadeMedidaEstatistica { get; set; }
		public string NumeroComunicadoCompra { get; set; }
		public string NumeroAtoDrawBack { get; set; }
		public string NumeroAgenciaSecex { get; set; }
		public int? IdMoeda { get; set; }
		public int? IdInconterms { get; set; }
		public int? IdUnidadeReceitaDespacho { get; set; }
		public int? IdRegimeTributario { get; set; }
		public int? IdFundamentoLegal { get; set; }
		public decimal? ValorCRA { get; set; }
		public int? TipoCoberturaCambial { get; set; }
		public int? LimiteDiasCoberturaCambial { get; set; }
		public int? InstituicaoFinanceira { get; set; }
		public int? IdMotivo { get; set; }
		public int? IdModalidadePagamento { get; set; }
		public byte? TipoAcordoTarifario { get; set; }
		public string InformacoesComplementares { get; set; }
		public byte? TipoBemEncomenda { get; set; }
		public byte? MaterialUsado { get; set; }
		public long IdSolicitacaoPli { get; set; }
		public short? CodigoProduto { get; set; }
		public short? CodigoTipoProduto { get; set; }
		public short? CodigoModeloProduto { get; set; }
		public string NCMMercadoria { get; set; }
		public string NCMDestaque { get; set; }
		public short? TipoFornecedor { get; set; }
		public int? IdNaladi { get; set; }
		public int? IdAladi { get; set; }
		public string DescricaoNCMMercadoria { get; set; }
		public decimal? ValorTotalCondicaoVenda { get; set; }
		public string DescricaoProduto { get; set; }
		public string CodigoPaisOrigemFabricante { get; set; }
		public string DescricaoPaisOrigemFabricante { get; set; }
		public string CodigoPaisOrigemMercadoria { get; set; }
		public string DescricaoPaisOrigemMercadoria { get; set; }
		public short? CodigoMoeda { get; set; }
		public string CodigoIncoterms { get; set; }
		public short? CodigoFundamentoLegal { get; set; }
		public short? CodigoInstituicaoFinanceira { get; set; }
		public short? CodigoMotivo { get; set; }
		public short? CodigoModalidadePagamento { get; set; }
		public short? CodigoAladi { get; set; }
		public int? CodigoNaladi { get; set; }
		public int? CodigoUnidadeReceitaFederalDespacho { get; set; }
		public string CodigoRegimeTributario { get; set; }
		public int? CodigoUnidadeReceitaFederalEntrada { get; set; }
		public int? IdUnidadeReceitaFederalEntrada { get; set; }
	}
}
