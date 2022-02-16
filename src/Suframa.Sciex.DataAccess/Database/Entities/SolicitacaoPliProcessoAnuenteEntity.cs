using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SOLIC_PLI_PROCESSO_ANUENTE")]
	public partial class SolicitacaoPliProcessoAnuenteEntity : BaseEntity
	{
		public virtual SolicitacaoPliMercadoriaEntity SolicitacaoPLIMercadoria { get; set; }
		public virtual OrgaoAnuenteEntity OrgaoAnuente { get; set; }

		[Key]
		[Required]
		[Column("SPP_ID")]
		public long IdSolicitacaoProcessoAnuente { get; set; }

		[Required]
		[Column("SPM_ID")]
		[ForeignKey(nameof(SolicitacaoPLIMercadoria))]
		public long IdSolicitacaoPliMercadoria { get; set; }

		[Required]
		[ForeignKey(nameof(OrgaoAnuente))]
		[Column("OAN_ID")]
		public int IdOrgaoAnuente { get; set; }
	
		[Required]
		[StringLength(20)]
		[Column("SPP_NU_PROCESSO")]
		public string NumeroProcesso { get; set; }

		[StringLength(10)]
		[Column("SSP_SG_ORGAO_ANUENTE")]
		public string SiglaOrgaoAnuente { get; set; }

	}
}