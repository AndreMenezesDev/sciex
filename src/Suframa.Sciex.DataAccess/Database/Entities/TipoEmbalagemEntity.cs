using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_TIPO_EMBALAGEM")]
	public class TipoEmbalagemEntity : BaseEntity
	{
		public TipoEmbalagemEntity()
		{

		}

		[Key]
		[Column("TEM_ID")]
		public int? Id { get; set; }

		[Required]
		[StringLength(200)]
		[Column("TEM_DS")]
		public string Descricao { get; set; }

		[Column("TEM_ST")]
		public byte Status { get; set; }

		[Column("TEM_CO")]
		public short Codigo { get; set; }

		[Column("TEM_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

	}
}
