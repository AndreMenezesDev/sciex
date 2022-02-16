using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_FUNDAMENTO_LEGAL")]
	public partial class FundamentoLegalEntity : BaseEntity
	{
		public virtual ICollection<ParametrosEntity> Parametros { get; set; }
		public virtual ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }

		[Key]
		[Column("FLE_ID")]
		public int IdFundamentoLegal { get; set; }

		[Required]
		[Column("FLE_CO")]
		public short Codigo { get; set; }

		[Required]
		[StringLength(120)]
		[Column("FLE_DS")]
		public string Descricao { get; set; }

		[Column("FLE_TP_AREA_BENEFICIO")]
		public short? TipoAreaBeneficio { get; set; }

		[Column("FLE_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

		public FundamentoLegalEntity()
		{
			Parametros = new HashSet<ParametrosEntity>();
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
		}
	}
}