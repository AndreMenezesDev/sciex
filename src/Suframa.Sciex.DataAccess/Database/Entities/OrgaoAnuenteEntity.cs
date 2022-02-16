using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ORGAO_ANUENTE")]
	public partial class OrgaoAnuenteEntity : BaseEntity
	{
		public virtual ICollection<PliProcessoAnuenteEntity> PliProcessoAnuenteEntity { get; set; }

		public OrgaoAnuenteEntity()
		{
			PliProcessoAnuenteEntity = new HashSet<PliProcessoAnuenteEntity>();
		}

		[Key]
		[Column("OAN_ID")]
		public int IdOrgaoAnuente { get; set; }

		[Required]
		[StringLength(10)]
		[Column("OAN_SG")]
		public string OrgaoSigla { get; set; }

		[Required]
		[StringLength(120)]
		[Column("OAN_DS")]
		public string Descricao { get; set; }

		[StringLength(14)]
		[Column("OAN_CO_CNPJ")]
		public string Cnpj { get; set; }

	}
}