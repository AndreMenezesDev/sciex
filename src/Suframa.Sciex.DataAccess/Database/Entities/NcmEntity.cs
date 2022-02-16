using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_NCM")]
	public partial class NcmEntity : BaseEntity
	{
		[Key]
		[Column("NCM_ID")]
		public int IdNcm { get; set; }

		[Required]
		[StringLength(8)]
		[Column("NCM_CO")]
		public string CodigoNCM { get; set; }

		[Required]
		[StringLength(150)]
		[Column("NCM_DS")]
		public string Descricao { get; set; }

		[Required]
		[Column("NCM_ST")]
		public byte Status { get; set; }

		[Column("NCM_ST_AMAZ_OCIDENTAL")]
		public byte? IsAmazoniaOcidental { get; set; }
	}
}