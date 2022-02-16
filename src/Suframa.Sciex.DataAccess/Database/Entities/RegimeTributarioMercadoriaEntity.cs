using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_REGIME_TRIBUTARIO_MERCADORIA")]
	public partial class RegimeTributarioMercadoriaEntity : BaseEntity
	{
		public virtual RegimeTributarioEntity RegimeTributario { get; set; }
		public virtual FundamentoLegalEntity FundamentoLegal { get; set; }

		[Key]
		[Column("RTM_ID")]
		public int IdRegimeTributarioMercadoria { get; set; }

		[Required]
		[Column("RTM_ST")]
		public byte Status { get; set; }

		[Required]
		[Column("RTB_ID")]
		[ForeignKey(nameof(RegimeTributario))]
		public int IdRegimeTributario { get; set; }

		[Required]
		[Column("FLE_ID")]
		[ForeignKey(nameof(FundamentoLegal))]
		public int IdFundamentoLegal { get; set; }

		[Required]
		[Column("RTM_CO_MUNICIPIO")]
		public int CodigoMunicipio { get; set; }

		[Required]
		[StringLength(100)]
		[Column("RTM_DS_MUNICIPIO")]
		public string DescricaoMunicipio { get; set; }

		[Required]
		[StringLength(2)]
		[Column("RTM_SG_UF")]
		public string UF { get; set; }

		[Required]
		[Column("RTM_DT_INICIO_VIGENCIA")]
		public DateTime DataInicioVigencia { get; set; }

		[Column("RTM_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

	}
}