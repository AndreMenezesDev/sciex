using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CADSUF_WORKFLOW_PROTOCOLO")]
	public partial class WorkflowProtocoloEntity : BaseEntity
	{
		public virtual ICollection<ConferenciaDocumentoEntity> ConferenciaDocumento { get; set; }

		[Column("WPR_DT")]
		public DateTime Data { get; set; }

		[Column("WPR_DT_NOTIFICACAO")]
		public DateTime? DataNotificacao { get; set; }

		[Column("PRT_ID")]
		[ForeignKey(nameof(Protocolo))]
		public int IdProtocolo { get; set; }

		[Column("SPR_ID")]
		[ForeignKey(nameof(StatusProtocolo))]
		public int IdStatusProtocolo { get; set; }

		[Column("USI_ID")]
		[ForeignKey(nameof(UsuarioInterno))]
		public int? IdUsuarioInterno { get; set; }

		[Key]
		[Column("WPR_ID")]
		public int IdWorkflowProtocolo { get; set; }

		[Column("WPR_DS_JUSTIFICATIVA")]
		public string Justificativa { get; set; }

		public virtual ICollection<JustificativaProtocoloEntity> JustificativaProtocolo { get; set; }

		public virtual ProtocoloEntity Protocolo { get; set; }

		public virtual StatusProtocoloEntity StatusProtocolo { get; set; }

		public virtual UsuarioInternoEntity UsuarioInterno { get; set; }

		public virtual ICollection<WorkflowMensagemPadraoEntity> WorkflowMensagemPadrao { get; set; }

		public WorkflowProtocoloEntity()
		{
			ConferenciaDocumento = new HashSet<ConferenciaDocumentoEntity>();
			JustificativaProtocolo = new HashSet<JustificativaProtocoloEntity>();
			WorkflowMensagemPadrao = new HashSet<WorkflowMensagemPadraoEntity>();
		}
	}
}