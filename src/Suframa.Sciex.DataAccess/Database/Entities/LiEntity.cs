using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_LI")]
	public partial class LiEntity : BaseEntity
	{

		public virtual PliMercadoriaEntity PliMercadoria { get; set; }
		public virtual DiEntity Di { get; set; }
		public virtual LiArquivoRetornoEntity LiArquivoRetorno { get; set; }

		[Key]
		[Column("PME_ID")]
		[ForeignKey(nameof(PliMercadoria))]
		public long IdPliMercadoria { get; set; }

		[Column("DI_ID")]
		[ForeignKey(nameof(Di))]
		public long? IdDI { get; set; }

		[Column("LAR_ID")]
		[ForeignKey(nameof(LiArquivoRetorno))]
		public long? IdLiArquivoRetorno { get; set; }

		[Column("LI_NU")]
		public long? NumeroLi { get; set; }

		[Required]
		[Column("LI_ST")]
		public byte Status { get; set; }

		[Required]
		[Column("LI_TP")]
		public byte TipoLi { get; set; }

		[Required]
		[Column("LI_DH_CADASTRO",TypeName = "datetime")]
		public DateTime DataCadastro { get; set; }

		[Column("LI_DH_CANCELAMENTO",TypeName = "datetime2")]
		public DateTime? DataCancelamento { get; set; }

		[Column("LI_NU_LI_PROTOCOLADA")]
		public long? NumeroProtocoloLI { get; set; }

		[Column("LI_DT_GERACAO")]
		public DateTime? DataGeracaoLI { get; set; }

		[Column("LI_NU_DIAGNOSTICO_ERRO")]
		public byte? CodigoErroDiagnostico { get; set; }

		[Column("LI_DS_MENSAGEM")]
		[StringLength(255)]
		public string MensagemErroLI { get; set; }

	}
}