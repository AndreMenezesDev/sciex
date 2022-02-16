using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_INSTITUICAO_FINANCEIRA")]
	public partial class InstituicaoFinanceiraEntity : BaseEntity
	{
		public virtual ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }

		public InstituicaoFinanceiraEntity()
		{
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
		}

		[Key]
		[Column("INF_ID")]
		public int IdInstituicaoFinanceira { get; set; }

		[Required]
		[Column("INF_CO")]
		public short Codigo { get; set; }

		[Required]
		[StringLength(120)]
		[Column("INF_DS")]
		public string Descricao { get; set; }
	}
}