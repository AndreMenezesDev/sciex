using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PRC_DUE")]
	public class PRCDueEntity : BaseEntity
	{
		public virtual PRCProdutoPaisEntity PRCProdutoPais { get; set; }

		[Key]
		[Column("due_id")]
		public int IdDue { get; set; }

		[ForeignKey(nameof(PRCProdutoPais))]
		[Column("prp_id")]
		public int? IdPRCProdutoPais { get; set; }

		[Column("due_nu")]
		[StringLength(15, MinimumLength = 1)]
		public string Numero { get; set; }

		[Column("due_dt_averbacao")]
		public DateTime DataAverbacao { get; set; }

		[Column("due_vl_dolar")]
		public decimal ValorDolar { get; set; }

		[Column("due_qt")]
		public decimal Quantidade { get; set; }

		[Column("pai_co")]
		public int CodigoPais { get; set; }


	}
}