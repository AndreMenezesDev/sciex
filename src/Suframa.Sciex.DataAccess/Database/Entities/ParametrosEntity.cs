using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PARAMETROS")]
	public partial class ParametrosEntity : BaseEntity
	{		
		public virtual MoedaEntity Moeda { get; set; }
		public virtual IncotermsEntity Incoterms { get; set; }
		public virtual UnidadeReceitaFederalEntity UnidadeReceitaFederalEntrada { get; set; }
		public virtual UnidadeReceitaFederalEntity UnidadeReceitaFederalDespacho { get; set; }
		public virtual FornecedorEntity Fornecedor { get; set; }
		public virtual FabricanteEntity Fabricante { get; set; }
		public virtual AladiEntity Aladi { get; set; }
		public virtual NaladiEntity Naladi { get; set; }
		public virtual RegimeTributarioEntity RegimeTributario { get; set; }
		public virtual FundamentoLegalEntity FundamentoLegal { get; set; }
		public virtual ModalidadePagamentoEntity ModalidadePagamento { get; set; }
		public virtual InstituicaoFinanceiraEntity InstituicaoFinanceira { get; set; }
		public virtual MotivoEntity Motivo { get; set; }

		[Key]
		[Column("PRM_ID")]
		public int IdParametro { get; set; }

		[Column("MOE_ID")]
		[ForeignKey(nameof(Moeda))]
		public int? IdMoeda { get; set; }

		[Column("INC_ID")]
		[ForeignKey(nameof(Incoterms))]
		public int? IdIncoterms { get; set; }

		[Column("URF_ID_ENTRADA")]
		[ForeignKey(nameof(UnidadeReceitaFederalEntrada))]
		public int? IdUnidadeReceitaFederalEntrada { get; set; }

		[Column("URF_ID_DESPACHO")]
		[ForeignKey(nameof(UnidadeReceitaFederalDespacho))]
		public int? IdUnidadeReceitaFederalDespacho { get; set; }

		[Column("FOR_ID")]
		[ForeignKey(nameof(Fornecedor))]
		public int? IdFornecedor { get; set; }

		[Column("FAB_ID")]
		[ForeignKey(nameof(Fabricante))]
		public int? IdFabricante { get; set; }

		[Column("ALA_ID")]
		[ForeignKey(nameof(Aladi))]
		public int? IdAladi { get; set; }

		[Column("NLD_ID")]
		[ForeignKey(nameof(Naladi))]
		public int? IdNaladi { get; set; }

		[Column("RTB_ID")]
		[ForeignKey(nameof(RegimeTributario))]
		public int? IdRegimeTributario { get; set; }

		[Column("FLE_ID")]
		[ForeignKey(nameof(FundamentoLegal))]
		public int? IdFundamentoLegal { get; set; }

		[Column("MOP_ID")]
		[ForeignKey(nameof(ModalidadePagamento))]
		public int? IdModalidadePagamento { get; set; }

		[Column("MOT_ID")]
		[ForeignKey(nameof(Motivo))]
		public int? IdMotivo { get; set; }

		[Column("INF_ID")]
		[ForeignKey(nameof(InstituicaoFinanceira))]
		public int? IdInstituicaoFinanceira { get; set; }

		[Required]
		[StringLength(120)]
		[Column("PRM_DS")]
		public string Descricao { get; set; }

		[Column("PRM_TP_COBERTURA_CAMBIAL")]
		public int? TipoCorbeturaCambial { get; set; }

		[Column("PRM_QT_DIA_LIMITE")]
		public int? QuantidadeDiaLimite { get; set; }

		[Column("PRM_TP_ACORDO_TARIFARIO")]
		public byte? TipoAcordoTarifario { get; set; }

		[Column("PRM_TP_FORNECEDOR")] 
		public short? TipoFornecedor { get; set; }

		[StringLength(3)]
		[Column("PAI_CO_MERCADORIA")]
		public string CodigoPaiMercadoria { get; set; }

		[StringLength(50)]
		[Column("PAI_DS_MERCADORIA")]
		public string DescricaoPaiMercadoria { get; set; }

		[StringLength(3)]
		[Column("PAI_CO_ORIGEM_FABRICANTE")]
		public string CodigoPaisOrigemFabricante { get; set; }

		[StringLength(50)]
		[Column("PAI_DS_ORIGEM_FABRICANTE")]
		public string DescricaoPaisOrigemFabricante { get; set; }

		[StringLength(14)]
		[Column("IMP_NU_CNPJ")]
		public string CPNJImportador { get; set; }

		[Column("PRM_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

		[Required]
		[Column("PRM_CO")]
		public int Codigo { get; set; }

	}
}