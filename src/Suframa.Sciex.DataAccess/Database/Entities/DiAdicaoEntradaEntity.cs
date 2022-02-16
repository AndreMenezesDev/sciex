using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_DI_ADICAO_ENTRADA")]
	public partial class DiAdicaoEntradaEntity : BaseEntity
	{		
		[Key]
		[Column("DAD_ID")]
		public long Id { get; set; }

		[Column("DAD_QT_ADICAO")]
		public string QuantidadeAdicao { get; set; }

		[Column("DAD_NU_LI")]
		public string NumeroLi { get; set; }

		[Column("DAD_NU_DI")]
		public string NumeroDi { get; set; }

		[Column("DAD_CO_VIA_TRANSPORTE")]
		public string CodigoViaTransporte { get; set; }

		[Column("DAD_TP_MULTIMODAL")]
		public string TipoMultimodal { get; set; }

		[Column("DAD_VL_PESO_LIQUIDO")]
		public string ValorPesoLiquido { get; set; }

		[Column("DAD_VL_FRETE_MOEDA_NEGOCIADA")]
		public string ValorFreteMoedaNegociada { get; set; }

		[Column("DAD_VL_FRETE_DOLAR")]
		public string ValorFreteDolar { get; set; }

		[Column("DAD_VL_FRETE")]
		public string ValorFrete { get; set; }

		[Column("DAD_VL_SEGURO_MOEDA_NEGOCIADA")]
		public string ValorSeguroMoedaNegociada { get; set; }

		[Column("DAD_VL_SEGURO_DOLAR")]
		public string ValorSeguroDolar { get; set; }

		[Column("DAD_VL_SEGURO")]
		public string ValorSeguro { get; set; }

		[Column("DAD_CO_MOEDA_FRETE")]
		public string CodigoMoedaFrete { get; set; }

		[Column("DAD_MOEDA_SEGURO")]
		public string MoedaSeguro { get; set; }

		[Column("DAD_CO_FUNDAMENTO_LEGAL")]
		public string CodigoFundamentoLegal { get; set; }

		[Column("DAD_VL_MERCADORIA_DOLAR")]
		public string ValorMercadoriaDolar { get; set; }

		[Column("DAD_VL_MERCADORIA_MOEDA_NEGOCIADA")]
		public string ValorMercadoriaMoedaNegociada { get; set; }

		[Column("DAD_ST")]
		public int Situacao { get; set; }

		public virtual DiEntradaEntity DiEntrada { get; set; }

		[Column("DIE_ID")]
		[ForeignKey(nameof(DiEntrada))]
		public long IdDiEntrada { get; set; }
	}
}