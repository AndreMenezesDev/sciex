using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_PRJ_NCM")]
	public partial class ViewNcmEntity : BaseEntity
	{
		[Key]
		[Column("NCM_ID")]
		public int IdNcm { get; set; }

		[Required]
		[StringLength(8)]
		[Column("NCM_CO")]
		public string CodigoNCM { get; set; }

		[Required]
		[StringLength(120)]
		[Column("NCM_DS")]
		public string Descricao { get; set; }

		[Required]
		[Column("NCM_ID_UME")]
		public short idNcmUnidadeMedida { get; set; }

		[Required]
		[Column("NCM_ST")]
		public short Status { get; set; }
	}
}