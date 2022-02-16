using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("PRJ_UNIDADE_MEDIDA")]
    public partial class PRJUnidadeMedidaEntity : BaseEntity
    {
		[Key]
		[Column("UME_ID")]
		public int IdUnidadeMedida { get; set; }		

		[Column("UME_CO")]
		public int? Codigo { get; set; }

		[StringLength(40)]
		[Column("UME_DS")]
		public string Descricao { get; set; }

		[StringLength(5)]
		[Column("UME_SG")]
		public string Sigla { get; set; }

	}
}