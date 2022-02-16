using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PLI_PRODUTO")]
	public partial class PliProdutoEntity : BaseEntity
	{
		public virtual ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }
		public virtual PliEntity Pli { get; set; }

		public PliProdutoEntity()
		{
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
		}

		[Key]
		[Column("PPR_ID")]
		public long IdPliProduto { get; set; }

		[Required]
		[Column("PLI_ID")]
		[ForeignKey(nameof(Pli))]
		public long IdPLI { get; set; }

		[Required]
		[Column("PPR_CO_PRODUTO")]
		public short CodigoProduto { get; set; }

		[Required]
		[Column("PPR_CO_TP_PRODUTO")]
		public short CodigoTipoProduto { get; set; }

		[Required]
		[Column("PPR_CO_MODELO_PRODUTO")]
		public short CodigoModeloProduto { get; set; }

		[StringLength(400)]
		[Column("PPR_DS_PRODUTO")]
		public string Descricao { get; set; }

		[Column("PPR_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }
	}
}