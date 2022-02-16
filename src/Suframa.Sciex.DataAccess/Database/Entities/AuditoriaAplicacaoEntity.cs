using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_AUDITORIA_APLICACAO")]
	public partial class AuditoriaAplicacaoEntity : BaseEntity
	{
		public virtual ICollection<AuditoriaEntity> ListaAuditoria { get; set; }

		public AuditoriaAplicacaoEntity()
		{
			ListaAuditoria = new HashSet<AuditoriaEntity>();
		}

		[Key]
		[Column("AAP_ID")]
		public int IdAuditoriaAplicacao { get; set; }

		[Required]
		[Column("AAP_CO")]
		public byte CodigoAplicacao { get; set; }

		[Required]
		[Column("AAP_DS")]
		[StringLength(80)]
		public string Descricao { get; set; }
	}
}