using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PRC_PRODUTO_PAIS")]
	public partial class PRCProdutoPaisEntity : BaseEntity
	{
		public virtual PRCProdutoEntity PrcProduto { get; set; }
		public virtual ICollection<PRCDueEntity> PrcDue { get; set; }

		public PRCProdutoPaisEntity()
		{
			PrcDue = new HashSet<PRCDueEntity>();
		}

		[Key]
		[Column("prp_id")]
		public int IdProdutoPais { get; set; }	
		
		[ForeignKey(nameof(PrcProduto))]
		[Column("pro_id")]
		public int IdPrcProduto { get; set; }	

		[Column("prp_qt_aprov")]
		public decimal? QuantidadeAprovado { get; set; }
		
		[Column("prp_vl_dolar_aprov")]
		public decimal? ValorDolarAprovado { get; set; }

		[Column("pai_co")]
		public int? CodigoPais { get; set; }

		[Column("prp_qt_comp")]
		public decimal? QuantidadeComprovado { get; set; }
		
		[Column("prp_vl_dolar_comp")]
		public decimal? ValorDolarComprovado { get; set; }

	}
}