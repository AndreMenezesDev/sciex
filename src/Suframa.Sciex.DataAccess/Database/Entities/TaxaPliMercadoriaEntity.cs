using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_TAXA_PLI_MERCADORIA")]
	public partial class TaxaPliMercadoriaEntity : BaseEntity
	{		
		public virtual PliMercadoriaEntity PliMercadoria { get; set; }
		public virtual FundamentoLegalEntity FundamentoLegal { get; set; }
		public virtual RegimeTributarioEntity RegimeTributario { get; set; }
		public virtual PliEntity Pli { get; set; }		
		public virtual ParidadeValorEntity ParidadeValor { get; set; }				

		[Key]
		[ForeignKey(nameof(PliMercadoria))]
		[Column("PME_ID")]
		public long IdPliMercadoria { get; set; }
		
		[ForeignKey(nameof(FundamentoLegal))]
		[Column("FLE_ID")]
		public int? IdFundamentoLegal { get; set; }

		[ForeignKey(nameof(RegimeTributario))]
		[Column("RTB_ID")]
		public int? IdRegimeTributario { get; set; }

		[ForeignKey(nameof(Pli))]
		[Column("PLI_ID")]
		public long? IdPli { get; set; }

		[Column("TGB_ID_ISENCAO")]
		public int? Isencao { get; set; }

		[Column("TGB_ID_REDUCAO")]
		public int? Reducao { get; set; }

		[ForeignKey(nameof(ParidadeValor))]
		[Column("PVA_ID_MOEDA_NEGOCIADA")]
		public int? IdMoedaNegociada { get; set; }

		[Column("PME_VL_PERC_REDUCAO")]
		public decimal? ValorPercentualReducao { get; set; }

		[Column("PME_VL_MERC_MOEDA_NEGOCIADA")]
		public decimal? ValorMercadoriaMoedaNegociada { get; set; }

		[Column("PME_VL_MERC_REAIS")]
		public decimal? ValorMercadoriaReais { get; set; }

		[Column("PME_QT_ITENS")]
		public short? QtdItens { get; set; }

		[Column("PME_DH_CADASTRO")]
		public DateTime? DataCadastro { get; set; }

		[Column("PCA_DT_PARIDADE")]
		public DateTime? DataParidade { get; set; }

		[StringLength(8)]
		[Column("MER_CO_NCM_MERCADORIA")]
		public string CodigoNCM { get; set; }
	}
}