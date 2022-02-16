using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_CODIGO_UTILIZACAO")]
	public partial class CodigoUtilizacaoEntity : BaseEntity
	{
		public ICollection<ControleImportacaoEntity> ControleImportacao { get; set; }
		public ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }

		public CodigoUtilizacaoEntity()
		{
			ControleImportacao = new HashSet<ControleImportacaoEntity>();
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
		}

		[Key]
		[Column("CUT_ID")]
		public int IdCodigoUtilizacao { get; set; }

		[Required]
		[StringLength(255)]
		[Column("CUT_DS")]
		public string Descricao { get; set; }

		[Required]
		[Column("CUT_ST")]
		public byte Status { get; set; }

		[Required]
		[Column("CUT_CO")]
		public short Codigo { get; set; }

		[Column("CUT_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }
	}
}