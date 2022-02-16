using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CADSUF_RECURSO")]
	public partial class RecursoEntity : BaseEntity, IData
	{
		public virtual ArquivoEntity Arquivo { get; set; }

		[Column("REC_DT_ALTERACAO")]
		public DateTime DataAlteracao { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[Column("REC_DT_INCLUSAO")]
		public DateTime DataInclusao { get; set; }

		[Column("ARQ_ID")]
		[ForeignKey(nameof(Arquivo))]
		public int? IdArquivo { get; set; }

		[Column("PAP_ID")]
		public int? IdPapel { get; set; }

		[Column("PRT_ID")]
		[ForeignKey(nameof(Protocolo))]
		public int IdProtocolo { get; set; }

		[Key]
		[Column("REC_ID")]
		public int IdRecurso { get; set; }

		[Column("USI_ID")]
		public int? IdUsuarioInterno { get; set; }

		[Column("REC_JUSTIFICATIVA")]
		public string Justificativa { get; set; }

		public virtual PapelEntity Papel { get; set; }

		public virtual ProtocoloEntity Protocolo { get; set; }

		public virtual UsuarioInternoEntity UsuarioInterno { get; set; }
	}
}