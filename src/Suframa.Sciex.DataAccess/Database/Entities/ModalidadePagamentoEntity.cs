using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_MODALIDADE_PAGTO")]
	public partial class ModalidadePagamentoEntity : BaseEntity
	{
		public virtual ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }

		public ModalidadePagamentoEntity()
		{
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
		}


		[Key]
		[Column("MOP_ID")]
		public int IdModalidadePagamento { get; set; }

		[Required]
		[Column("MOP_CO")]
		public short Codigo { get; set; }

		[Required]
		[StringLength(120)]
		[Column("MOP_DS")]
		public string Descricao { get; set; }
	}
}