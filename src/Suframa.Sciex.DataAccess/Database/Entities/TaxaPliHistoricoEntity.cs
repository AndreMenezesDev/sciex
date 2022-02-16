using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_TAXA_PLI_HISTORICO")]
	public partial class TaxaPliHistoricoEntity : BaseEntity
	{
		public virtual PliEntity Pli { get; set; }

		[Key]
		[Column("TPH_ID")]
		public long IdTaxaPliHistorico { get; set; }

		[Required]
		[ForeignKey(nameof(Pli))]
		[Column("PLI_ID")]
		public long IdPli { get; set; }

		[Required]
		[Column("TPL_ST_TAXA")]
		public short StatusTaxa { get; set; }

		[Required]		
		[Column("TPH_DH_EVENTO")]
		public DateTime DataEvento { get; set; }

		[MaxLength]
		[Column("TPH_ME_RETORNO_SAC")]
		public string RetornoSac { get; set; }

		[StringLength(1000)]
		[Column("TPH_DS_OBSERVACAO")]
		public string Observacao { get; set; }		



	}
}