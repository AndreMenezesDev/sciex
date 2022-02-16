using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_CODIGO_CONTA")]
	public partial class CodigoContaEntity : BaseEntity
	{
		public ICollection<ControleImportacaoEntity> ControleImportacao { get; set; }
		public ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }

		public CodigoContaEntity()
		{
			ControleImportacao = new HashSet<ControleImportacaoEntity>();
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
		}

		[Key]
		[Column("CCO_ID")]
		public int IdCodigoConta { get; set; }

		[Required]
		[StringLength(255)]
		[Column("CCO_DS")]
		public string Descricao { get; set; }

		[Required]
		[Column("CCO_ST")]
		public byte Status { get; set; }

		[Required]
		[Column("CCO_CO")]
		public short Codigo { get; set; }

		[Column("CCO_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }
	}
}