using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SETOR_ARMAZENAMENTO")]
	public class SetorArmazenamentoEntity : BaseEntity
	{
		public SetorArmazenamentoEntity()
		{

		}

		[Key]
		[Column("SAR_ID")]
		public int? Id { get; set; }

		[Required]
		[StringLength(200)]
		[Column("SAR_DS")]
		public string Descricao { get; set; }

		[Column("SAR_ST")]
		public byte Status { get; set; }

		[Column("SAR_CO")]
		public int Codigo { get; set; }

		[Column("SAR_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

		public virtual RecintoAlfandegaEntity RecintoAlfandega { get; set; }
		[Column("RAL_ID")]
		[ForeignKey(nameof(RecintoAlfandega))]
		public int IdRecintoAlfandega { get; set; }
	}
}
