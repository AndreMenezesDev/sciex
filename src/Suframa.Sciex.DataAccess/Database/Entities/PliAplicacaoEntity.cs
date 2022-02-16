using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PLI_APLICACAO")]
	public partial class PliAplicacaoEntity : BaseEntity
	{
		public virtual ICollection<PliEntity> PliEntity { get; set; }
		public virtual ICollection<ControleImportacaoEntity> ControleImportacao { get; set; }

		public PliAplicacaoEntity()
		{
			PliEntity = new HashSet<PliEntity>();
			ControleImportacao = new HashSet<ControleImportacaoEntity>();
		}

		[Key]
		[Column("PAP_ID")]
		public int IdPliAplicacao { get; set; }

		[Required]
		[Column("PAP_CO")]
		public short Codigo { get; set; }

		[StringLength(120)]
		[Column("PAP_DS")]
		public string Descricao { get; set; }

	}
}