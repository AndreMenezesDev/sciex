using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CADSUF_REQUERIMENTO_DOCUMENTO")]
	public partial class RequerimentoDocumentoEntity : BaseEntity
	{
		public virtual ArquivoEntity Arquivo { get; set; }

		[Column("RDO_DT_EXPEDICAO")]
		public DateTime? DataExpedicao { get; set; }

		[Column("RDO_DT_VENCIMENTO")]
		public DateTime? DataVencimento { get; set; }

		[Column("ARQ_ID")]
		[ForeignKey(nameof(Arquivo))]
		public int? IdArquivo { get; set; }

		[Column("REQ_ID")]
		[ForeignKey(nameof(Requerimento))]
		public int IdRequerimento { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("RDO_ID")]
		public int IdRequerimentoDocumento { get; set; }

		[Column("TDO_ID")]
		[ForeignKey(nameof(TipoDocumento))]
		public int IdTipoDocumento { get; set; }

		[StringLength(50)]
		[Column("RDO_NU_CERTIDAO")]
		public string NumeroCertidao { get; set; }

		public virtual RequerimentoEntity Requerimento { get; set; }

		[Required]
		[StringLength(1)]
		[Column("RDO_ST")]
		public string Status { get; set; }

		public virtual TipoDocumentoEntity TipoDocumento { get; set; }

		[Column("RDO_TP_ORIGEM")]
		public int TipoOrigem { get; set; }
	}
}