using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SOLIC_PLI_MERCADORIA")]
	public partial class SolicitacaoPliMercadoriaEntity : BaseEntity
	{
		public virtual ICollection<SolicitacaoPliDetalheMercadoriaEntity> SolicitacaoPliDetalheMercadoria { get; set; }
		public virtual ICollection<SolicitacaoPliProcessoAnuenteEntity> SolicitacaoPliProcessoAnuente { get; set; }

		public virtual SolicitacaoPliEntity SolicitacaoPli { get; set; }		

		public SolicitacaoPliMercadoriaEntity()
		{
			SolicitacaoPliDetalheMercadoria = new HashSet<SolicitacaoPliDetalheMercadoriaEntity>();
			SolicitacaoPliProcessoAnuente = new HashSet<SolicitacaoPliProcessoAnuenteEntity>();	
		}

		[Key]
		[Column("SPM_ID")]
		public long IdSolicitacaoPliMercadoria { get; set; }

		[Column("SPM_NU_PESO_LIQUIDO")]
		public decimal? PesoLiquido { get; set; }

		[Column("SPM_QT_UNID_MEDIDA_ESTATISTICA")]
		public decimal? QuantidadeUnidadeMedidaEstatistica { get; set; }

		[Column("SPM_NU_COMUNICADO_COMPRA")]
		[StringLength(13)]
		public string NumeroComunicadoCompra { get; set; }

		[Column("SPM_NU_ATO_DRAWBACK")]
		[StringLength(13)]
		public string NumeroAtoDrawBack { get; set; }

		[Column("SPM_NU_AGENCIA_SECEX")]
		[StringLength(5)]
		public string NumeroAgenciaSecex { get; set; }

		[Column("MOE_ID")]
		public int? IdMoeda { get; set; }

		[Column("INC_ID")]
		public int? IdInconterms { get; set; }
	
		[Column("RFB_ID_DESPACHO")]
		public int? IdUnidadeReceitaDespacho { get; set; }

		[Column("RTB_ID")]
		public int? IdRegimeTributario { get; set; }

		[Column("FLE_ID")]
		public int? IdFundamentoLegal { get; set; }

		[Column("SPM_VL_CRA")]
		public decimal? ValorCRA { get; set; }

		[Column("SPM_TP_COBCAMBIAL")]
		public int? TipoCoberturaCambial { get; set; }

		[Column("SPM_NU_COBCAMBIAL_LIMITE_DIAS_PAGTO")]
		public int? LimiteDiasCoberturaCambial { get; set; }

		[Column("INF_ID")]
		public int? InstituicaoFinanceira { get; set; }

		[Column("MOT_ID")]
		public int? IdMotivo { get; set; }

		[Column("MOP_ID")]
		public int? IdModalidadePagamento { get; set; }

		[Column("SPM_TP_ACORDO_TARIFARIO")]
		public byte? TipoAcordoTarifario { get; set; }

		[Column("SPM_DS_INFORMACAO_COMPLEMENTAR")]
		[StringLength(4048)]
		public string InformacoesComplementares { get; set; }

		[Column("SPM_TP_BEM_ENCOMENDA")]
		public byte? TipoBemEncomenda { get; set; }

		[Column("SPM_TP_MATERIAL_USADO")]
		public byte? MaterialUsado { get; set; }

		[Required]
		[Column("SPL_ID")]
		[ForeignKey(nameof(SolicitacaoPli))]
		public long IdSolicitacaoPli { get; set; }

		[Column("SPR_CO_PRODUTO")]
		public short? CodigoProduto { get; set; }

		[Column("SPR_CO_TP_PRODUTO")]
		public short? CodigoTipoProduto { get; set; }

		[Column("SPR_CO_MODELO_PRODUTO")]
		public short? CodigoModeloProduto { get; set; }

		[Column("MER_CO_NCM_MERCADORIA")]
		[StringLength(8)]
		public string NCMMercadoria { get; set; }

		[Column("SPM_NU_NCM_DESTAQUE")]
		[StringLength(3)]
		public string NCMDestaque { get; set; }

		[Column("SPM_TP_FORNECEDOR")]
		public short? TipoFornecedor { get; set; }

		[Column("NLD_ID")]
		public int? IdNaladi { get; set; }

		[Column("ALA_ID")]
		public int? IdAladi { get; set; }

		[Column("MER_DS_NCM_MERCADORIA")]
		[StringLength(120)]
		public string DescricaoNCMMercadoria { get; set; }

		[Column("SPM_VL_TOTAL_CONDICAO_VENDA")]
		public decimal? ValorTotalCondicaoVenda { get; set; }

		[Column("PME_DS_PRODUTO")]
		[StringLength(500)]
		public string DescricaoProduto { get; set; }

		[Column("PAI_CO_ORIGEM_FABRICANTE")]
		[StringLength(3)]
		public string CodigoPaisOrigemFabricante { get; set; }

		[Column("PAI_DS_ORIGEM_FABRICANTE")]
		[StringLength(50)]
		public string DescricaoPaisOrigemFabricante { get; set; }

		[Column("PAI_CO_ORIGEM_MERCADORIA")]
		[StringLength(3)]
		public string CodigoPaisOrigemMercadoria { get; set; }

		[Column("PAI_DS_ORIGEM_MERCADORIA")]
		[StringLength(50)]
		public string DescricaoPaisOrigemMercadoria { get; set; }

		[Column("MOE_CO")]
		public short? CodigoMoeda { get; set; }

		[Column("INC_CO")]
		public string CodigoIncoterms { get; set; }
		
		[Column("FLE_CO")]
		public short? CodigoFundamentoLegal { get; set; }

		[Column("INF_CO")]
		public short? CodigoInstituicaoFinanceira { get; set; }

		[Column("MOT_CO")]
		public short? CodigoMotivo { get; set; }

		[Column("MOP_CO")]
		public short? CodigoModalidadePagamento { get; set; }

		[Column("ALA_CO")]
		public short? CodigoAladi { get; set; }

		[Column("NLD_CO")]
		public int? CodigoNaladi { get; set; }

		[Column("RFB_CO_DESPACHO")]
		public int? CodigoUnidadeReceitaFederalDespacho { get; set; }

		[Column("RTB_CO")]
		public string CodigoRegimeTributario { get; set; }

		[Column("RFB_CO_ENTRADA")]
		public int? CodigoUnidadeReceitaFederalEntrada { get; set; }

		[Column("RFB_ID_ENTRADA")]
		public int? IdUnidadeReceitaFederalEntrada { get; set; }
	}
}