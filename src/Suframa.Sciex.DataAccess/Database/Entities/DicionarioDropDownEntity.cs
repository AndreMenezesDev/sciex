using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_DICIONARIO_DROPDOWN")]
	public partial class DicionarioDropDownEntity : BaseEntity
	{
		[Key]
		[Column(Order = 1)]
		[StringLength(24)]
		public string Campo { get; set; }

		[StringLength(500)]
		public string Descricao { get; set; }

		[Key]
		[Column(Order = 0)]
		[StringLength(29)]
		public string Secao { get; set; }

		[Key]
		[Column(Order = 2, TypeName = "numeric")]
		public decimal Valor { get; set; }
	}
}