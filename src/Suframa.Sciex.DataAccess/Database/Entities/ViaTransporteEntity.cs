
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_VIA_TRANSPORTE")]
	public partial class ViaTransporteEntity: BaseEntity
	{
		public ViaTransporteEntity()
		{

		}

		[Key]
		[Column("vtr_id")]
		public int IdViaTransporte { get; set; }

		[Required]
		[Column("vtr_ds")]
		[StringLength(200)]
		public string Descricao { get; set; }

		[Required]
		[Column("vtr_st")]
		public byte Status { get; set; }

		[Required]
		[Column("vtr_co")]
		public short Codigo { get; set; }

		[Column("vtr_col_row_version")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

	}
}
