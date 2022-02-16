using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_AUDITORIA")]
	public partial class AuditoriaEntity : BaseEntity
	{
		public virtual AuditoriaAplicacaoEntity AuditoriaAplicacao { get; set; }

		[Key]
		[Column("AUD_ID")]
		public long IdAuditoria { get; set; }
		
		[Column("AAP_ID")]
		[ForeignKey(nameof(AuditoriaAplicacao))]
		public int IdAuditoriaAplicacao { get; set; }

		[Required]
		[Column("AUD_NU_CPFCNPJ_RESPONSAVEL")]
		[StringLength(14)]
		public string CpfCnpjResponsavel { get; set; }

		[Required]
		[Column("AUD_NO_RESPONSAVEL")]
		public string NomeResponsavel { get; set; }

		[Required]
		[Column("AUD_TP_ACAO")]
		public byte TipoAcao { get; set; }

		[Required]
		[Column("AUD_DH_ACAO",TypeName ="datetime2")]
		public DateTime DataHoraAcao { get; set; }

		[Required]
		[Column("AUD_DS_ACAO")]
		public string DescricaoAcao { get; set; }
		
		[StringLength(500)]
		[Column("AUD_DS_JUSTIFICATIVA")]
		public string Justificativa { get; set; }

		[Required]
		[Column("AUD_ID_REFERENCIA")]
		public long IdReferencia { get; set; }
	}
}