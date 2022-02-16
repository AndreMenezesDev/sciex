using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_UNID_RFB")]
	public partial class UnidadeReceitaFederalEntity : BaseEntity
	{
		public virtual ICollection<ParametrosEntity> ParametrosUnidadeEntrada { get; set; }
		public virtual ICollection<ParametrosEntity> ParametrosUnidadeDespacho { get; set; }
		public virtual ICollection<PliMercadoriaEntity> PliMercadoriaUnidadeEntrada { get; set; }
		public virtual ICollection<PliMercadoriaEntity> PliMercadoriaUnidadeDespacho { get; set; }

		[Key]
		[Column("RFB_ID")]
		public int IdUnidadeReceitaFederal { get; set; }

		[Required]
		[Column("RFB_CO")]
		public int Codigo { get; set; }

		[Required]
		[StringLength(120)]
		[Column("RFB_DS")]
		public string Descricao { get; set; }

		[Column("RFB_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

		public UnidadeReceitaFederalEntity()
		{
			ParametrosUnidadeEntrada = new HashSet<ParametrosEntity>();
			ParametrosUnidadeDespacho = new HashSet<ParametrosEntity>();

			PliMercadoriaUnidadeEntrada = new HashSet<PliMercadoriaEntity>();
			PliMercadoriaUnidadeDespacho = new HashSet<PliMercadoriaEntity>();
		}
	}
}