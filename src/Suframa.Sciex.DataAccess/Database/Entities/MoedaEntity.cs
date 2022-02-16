using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_MOEDA")]
	public partial class MoedaEntity : BaseEntity
	{
		public virtual ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }
		public virtual ICollection<ParidadeValorEntity> ParidadeValor { get; set; }

		public MoedaEntity()
		{
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
			ParidadeValor = new HashSet<ParidadeValorEntity>();
		}

		[Key]
		[Column("MOE_ID")]
		public int IdMoeda { get; set; }

		[Required]
		[Column("MOE_CO")]		
		public short CodigoMoeda { get; set; }

		[Required]
		[StringLength(60)]
		[Column("MOE_DS")]
		public string Descricao { get; set; }

		[Required]
		[StringLength(10)]
		[Column("MOE_SG")]
		public string Sigla { get; set; }
	}
}