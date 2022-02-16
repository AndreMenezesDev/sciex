using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_PRJ_MERCADORIA")]
	public partial class ViewMercadoriaEntity : BaseEntity
	{

		[Key]
		[Column("MER_ID")]
		public int IdMercadoria { get; set; }

		[Column("MER_CO_PRODUTO")]
		public short CodigoProdutoMercadoria { get; set; }
	
		[StringLength(8)]
		[Column("MER_CO_NCM_MERCADORIA")]
		public string CodigoNCMMercadoria { get; set; }

		[StringLength(120)]
		[Column("MER_DS_NCM_MERCADORIA")]
		public string Descricao { get; set; }

		[Column("MER_ST_MERCADORIA")]
		public short StatusMercadoria { get; set; }

		

	}
}