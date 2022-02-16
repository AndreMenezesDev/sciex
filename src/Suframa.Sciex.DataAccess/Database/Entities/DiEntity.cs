using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_DI")]
	public partial class DiEntity : BaseEntity
	{		

		[Key]
		[Column("DI_ID")]
		public long IdDi { get; set; }
		
		[Required]
		[Column("DI_NU")]
		public long NumeroDi { get; set; }

		[Column("DI_QT_ADICAO")]
		public int QtdAdicao { get; set; }

		[Column("DI_DT_DESEMBARACO")]
		public DateTime? DataDesembaraco { get; set; }

		[Column("DI_DT_REGISTRO")]
		public DateTime? DataRegistro { get; set; }

		[Column("DI_CO_DECLARACAO")]
		public int TipoDeclaracaoCodigo { get; set; }

		[Column("DI_NU_CNPJ")]
		[StringLength(14)]
		public string Cnpj { get; set; }

		[Column("DI_CO_RECINTO_ALFANDEGA")]
		public int? LocalAlfandega { get; set; }

		[Column("DI_CO_VIA_TRANSP_CARGA")]
		public int ViaTransporteCodigo { get; set; }

		[Column("DI_TP_MULTIMODAL")]
		public decimal TipoMultimodal { get; set; }

		[Column("DI_VL_TOTAL_MLE_DOLAR")]
		public decimal? ValorTotalDolar { get; set; }

		[Column("DI_CO_SETOR_ARMAZENAMENTO")]
		public int SetorArmazenamento { get; set; }

		[Column("DI_VL_TOTAL_MLE_MN")]
		public decimal? ValorTotalMn { get; set; }

		[Column("DI_DH_PROCESSAMENTO")]
		public DateTime? DataProcessamento { get; set; }

		public virtual UnidadeReceitaFederalEntity UrfEntrada { get; set; }

		[Column("RFB_ID_ENTRADA")]
		[ForeignKey(nameof(UrfEntrada))]
		public int IdUrfEntrada { get; set; }

		public virtual UnidadeReceitaFederalEntity UrfDespacho { get; set; }

		[Column("RFB_ID_DESPACHO")]
		[ForeignKey(nameof(UrfDespacho))]
		public int IdUrfDespacho { get; set; }
	}
}