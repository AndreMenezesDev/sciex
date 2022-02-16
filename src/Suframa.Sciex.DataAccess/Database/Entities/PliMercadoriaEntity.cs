using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PLI_MERCADORIA")]
	public partial class PliMercadoriaEntity : BaseEntity
	{
		public virtual MoedaEntity Moeda { get; set; }
		public virtual IncotermsEntity Incoterms { get; set; }
		public virtual RegimeTributarioEntity RegimeTributario { get; set; }
		public virtual FabricanteEntity Fabricante { get; set; }
		public virtual FornecedorEntity Fornecedor { get; set; }
		public virtual FundamentoLegalEntity FundamentoLegal { get; set; }
		public virtual InstituicaoFinanceiraEntity InstituicaoFinanceira { get; set; }
		public virtual MotivoEntity Motivo { get; set; }
		public virtual ModalidadePagamentoEntity ModalidadePagamento { get; set; }
		public virtual AladiEntity Aladi { get; set; }
		public virtual NaladiEntity Naladi { get; set; }
		public virtual UnidadeReceitaFederalEntity UnidadeReceitaFederalEntrada { get; set; }
		public virtual UnidadeReceitaFederalEntity UnidadeReceitaFederalDespacho { get; set; }
		public virtual PliEntity Pli { get; set; }
		public virtual PliProdutoEntity PliProduto { get; set; }
		public virtual CodigoContaEntity CodigoConta { get; set; }
		public virtual CodigoUtilizacaoEntity CodigoUtilizacao { get; set; }

		// tabelas com relacionamento "1:1"
		public virtual TaxaPliMercadoriaEntity TaxaPliMercadoria { get; set; }
		public virtual AliEntity Ali { get; set; }
		public virtual LiEntity Li { get; set; }
		public virtual PliFornecedorFabricanteEntity PliFornecedorFabricante { get; set; }
		public virtual ICollection<LancamentoEntity> Lancamento { get; set; }
		public virtual ICollection<PliDetalheMercadoriaEntity> PliDetalheMercadoria { get; set; }
		public virtual ICollection<PliProcessoAnuenteEntity> PliProcessoAnuente { get; set; }
		public virtual ICollection<AliHistoricoEntity> AliHistorico { get; set; }
		public virtual ICollection<AliEntradaArquivoEntity> AliEntradaArquivo { get; set; }
		//public virtual ICollection<AliEntity> Ali { get; set; }
		//public virtual ICollection<LiEntity> Li { get; set; }

		public PliMercadoriaEntity()
		{
			PliDetalheMercadoria = new HashSet<PliDetalheMercadoriaEntity>();
			PliProcessoAnuente = new HashSet<PliProcessoAnuenteEntity>();
			Lancamento = new HashSet<LancamentoEntity>();
			AliHistorico = new HashSet<AliHistoricoEntity>();
			AliEntradaArquivo = new HashSet<AliEntradaArquivoEntity>();
			//Ali = new HashSet<AliEntity>();
			//Li = new HashSet<LiEntity>();
		}		

		[Key]
		[Column("PME_ID")]
		public long IdPliMercadoria { get; set; }

		[Required]
		[Column("PLI_ID")]
		[ForeignKey(nameof(Pli))]
		public long IdPLI { get; set; }
		
		[Column("MOE_ID")]
		[ForeignKey(nameof(Moeda))]
		public int? IdMoeda { get; set; }

		[Column("INC_ID")]
		[ForeignKey(nameof(Incoterms))]
		public int? IdIncoterms { get; set; }
		
		[Column("RTB_ID")]
		[ForeignKey(nameof(RegimeTributario))]
		public int? IdRegimeTributario { get; set; }

		[Column("FLE_ID")]
		[ForeignKey(nameof(FundamentoLegal))]
		public int? IdFundamentoLegal { get; set; }

		[Column("FAB_ID")]
		[ForeignKey(nameof(Fabricante))]
		public int? IdFabricante { get; set; }

		[Column("FOR_ID")]
		[ForeignKey(nameof(Fornecedor))]
		public int? IdFornecedor { get; set; }

		[Column("INF_ID")]
		[ForeignKey(nameof(InstituicaoFinanceira))]
		public int? IdInstituicaoFinanceira { get; set; }

		[Column("MOT_ID")]
		[ForeignKey(nameof(Motivo))]
		public int? IdMotivo { get; set; }

		[Column("MOP_ID")]
		[ForeignKey(nameof(ModalidadePagamento))]
		public int? IdModalidadePagamento { get; set; }

		[Column("ALA_ID")]
		[ForeignKey(nameof(Aladi))]
		public int? IdAladi { get; set; }

		[Column("NLD_ID")]
		[ForeignKey(nameof(Naladi))]
		public int? IdNaladi { get; set; }

		[Column("PPR_ID")]
		[ForeignKey(nameof(PliProduto))]
		public long? IdPliProduto { get; set; }

		[Column("CUT_ID")]
		[ForeignKey(nameof(CodigoUtilizacao))]
		public int? IdCodigoUtilizacao { get; set; }

		[Column("CCO_ID")]
		[ForeignKey(nameof(CodigoConta))]
		public int? IdCodigoConta { get; set; }

		[Column("RFB_ID_ENTRADA")]
		[ForeignKey(nameof(UnidadeReceitaFederalEntrada))]
		public int? IdURFEntrada { get; set; }

		[Column("RFB_ID_DESPACHO")]
		[ForeignKey(nameof(UnidadeReceitaFederalDespacho))]
		public int? IdURFDespacho { get; set; }

		[StringLength(3)]
		[Column("PAI_CO_MERCADORIA")]
		public string CodigoPais { get; set; }

		[StringLength(50)]
		[Column("PAI_DS_MERCADORIA")]
		public string DescricaoPais { get; set; }

		[StringLength(3)]
		[Column("PAI_CO_ORIGEM_FABRICANTE")]
		public string CodigoPaisOrigemFabricante { get; set; }

		[StringLength(50)]
		[Column("PAI_DS_ORIGEM_FABRICANTE")]
		public string DescricaoPaisOrigemFabricante { get; set; }

		[Column("PME_NU_PESO_LIQUIDO")]
		public decimal? PesoLiquido { get; set; }

		[Column("PME_QT_UNID_MEDIDA_ESTATISTICA")]
		public decimal? QuantidadeUnidadeMedidaEstatistica { get; set; }

		[StringLength(13)]
		[Column("PME_NU_COMUNICADO_COMPRA")]
		public string NumeroComunicadoCompra { get; set; }

		[StringLength(13)]
		[Column("PME_NU_ATO_DRAWBACK")]
		public string NumeroAtoDrawback { get; set; }

		[StringLength(5)]
		[Column("PME_NU_AGENCIA_SECEX")]
		public string NumeroAgenciaSecex { get; set; }

		[Column("PME_VL_CRA")]
		public decimal? ValorCRA { get; set; }

		[Column("PME_TP_COBCAMBIAL")]
		public int? TipoCOBCambial { get; set; }

		[Column("PME_NU_COBCAMBIAL_LIMITE_DIAS_PAGTO")]
		public int? NumeroCOBCambialLimiteDiasPagamento{ get; set; }

		[Column("PME_TP_ACORDO_TARIFARIO")]
		public byte? TipoAcordoTarifario { get; set; }

		[StringLength(4048)]
		[Column("PME_DS_INFORMACAO_COMPLEMENTAR")]
		public string DescricaoInformacaoComplementar { get; set; }

		[Column("PME_TP_BEM_ENCOMENDA")]
		public byte? TipoBemEncomenda { get; set; }

		[Column("PME_TP_MATERIAL_USADO")]
		public byte? TipoMaterialUsado { get; set; }

		[StringLength(3)]
		[Column("PME_NU_NCM_DESTAQUE")]
		public string NumeroNCMDestaque { get; set; }

		[Column("PME_CO_PRODUTO")]
		public short? CodigoProduto { get; set; }

		[StringLength(500)]
		[Column("PME_DS_PRODUTO")]
		public string DescricaoProduto { get; set; }

		[Column("PME_CO_TP_PRODUTO")]
		public short? CodigoTipoProduto { get; set; }

		[Column("PME_CO_MODELO_PRODUTO")]
		public short? CodigoModeloProduto { get; set; }

		[StringLength(8)]
		[Column("MER_CO_NCM_MERCADORIA")]
		public string CodigoNCMMercadoria { get; set; }

		[StringLength(120)]
		[Column("MER_DS_NCM_MERCADORIA")]
		public string DescricaoNCMMercadoria { get; set; }

		[Column("PME_TP_FORNECEDOR")]
		public short? TipoFornecedor { get; set; }
			
		[Column("PME_VL_TOTAL_CONDICAO_VENDA")]
		public decimal? ValorTotalCondicaoVenda { get; set; }

		[Column("PME_VL_TOTAL_CONDICAO_VENDA_REAL")]
		public decimal? ValorTotalCondicaoVendaReal { get; set; }

		[Column("PME_VL_TOTAL_CONDICAO_VENDA_DOLAR")]
		public decimal? ValorTotalCondicaoVendaDolar { get; set; }

		[Column("PME_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

		[Column("pme_nu_li_retificador")]
		public int? NumeroLiRetificador { get; set; }


		public PliMercadoriaEntity ShallowCopy()
		{
			return (PliMercadoriaEntity)this.MemberwiseClone();
		}
	}
}