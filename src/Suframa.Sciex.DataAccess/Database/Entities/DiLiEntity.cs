using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_DI_LI")]
	public partial class DiLiEntity : BaseEntity
	{		

		[Key]
		[Column("DLI_ID")]
		public long Id { get; set; }

		[Column("DLI_NU_LI")]
		public int NumeroLi { get; set; }

		[Column("DLI_TP_MULTIMODAL")]
		public decimal TipoMultimodal { get; set; }

		[Column("DLI_VL_PESO_LIQUIDO")]
		public decimal ValorPesoLiquido { get; set; }

		[Column("DLI_VL_FRETE_MOEDA_NEGOCIADA")]
		public decimal ValorFreteMoedaNegociada { get; set; }

		[Column("DLI_VL_FRETE_DOLAR")]
		public decimal ValorFreteDolar { get; set; }

		[Column("DLI_VL_FRETE")]
		public decimal ValorFrete { get; set; }

		[Column("DLI_VL_SEGURO_MOEDA_NEGOCIADA")]
		public decimal ValorSeguroMoedaNegociada { get; set; }

		[Column("DLI_VL_SEGURO_DOLAR")]
		public decimal ValorSeguroDolar { get; set; }

		[Column("DLI_VL_SEGURO")]
		public decimal ValorSeguro { get; set; }

		[Column("DLI_VL_MERCADORIA_DOLAR")]
		public decimal ValorMercadoriaDolar { get; set; }

		[Column("DLI_VL_MERCADORIA_MOEDA_NEGOCIADA")]
		public decimal ValorMercadoriaMoedaNegociada { get; set; }

		[Column("DLI_CO_VIA_TRANSPORTE")]
		public int CodigoViaTransporte { get; set; }

		public virtual DiEntity Di { get; set; }

		[Column("DI_ID")]
		[ForeignKey(nameof(Di))]
		public long IdDi { get; set; }

		public virtual MoedaEntity MoedaFrete { get; set; }

		[Column("MOE_ID_FRETE")]
		[ForeignKey(nameof(MoedaFrete))]
		public int IdMoedaFrete { get; set; }

		public virtual MoedaEntity MoedaSeguro { get; set; }

		[Column("MOE_ID_SEGURO")]
		[ForeignKey(nameof(MoedaSeguro))]
		public int IdMoedaSeguro { get; set; }

		public virtual FundamentoLegalEntity FundamentoLegal { get; set; }

		[Column("FLE_ID")]
		[ForeignKey(nameof(FundamentoLegal))]
		public int IdFundamentoLegal { get; set; }
	}
}