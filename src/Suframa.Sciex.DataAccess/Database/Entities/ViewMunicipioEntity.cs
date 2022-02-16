using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_SCIEX_MUNICIPIO")]
	public partial class ViewMunicipioEntity : BaseEntity
	{
		[Key]
		[Column("MUN_ID")]
		public int IdMunicipio { get; set; }

		[Required]
		[Column("MUN_CO")]
		public decimal CodigoMunicipio { get; set; }

		[Required]
		[StringLength(100)]
		[Column("MUN_DS")]
		public string Descricao { get; set; }

		[Required]
		[StringLength(2)]
		[Column("MUN_SG_UF")]
		public string SiglaUF { get; set; }

		[Column("MUN_CO_UF")]
		public int CodigoUF { get; set; }

	}
}