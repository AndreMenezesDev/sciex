using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_LI_HISTORICO_ERRO")]
	public partial class LiHistoricoErroEntity : BaseEntity
	{

		public virtual PliMercadoriaEntity PliMercadoria { get; set; }		
		public virtual LiArquivoRetornoEntity LiArquivoRetorno { get; set; }

		[Key]
		[Required]
		[Column("LHE_ID")]
		public long IdLIHistoricoErro { get; set; }

		[Column("LHE_TP_ERRO")]
		public byte? TipoErroLiH { get; set; }

		[Column("PME_ID")]
		[ForeignKey(nameof(PliMercadoria))]
		public long? IdPliMercadoria { get; set; }

		[Column("LAR_ID")]
		[ForeignKey(nameof(LiArquivoRetorno))]
		public long? IdLIArquivoRetorno { get; set; }

		[Column("LI_NU")]
		public long? NumeroLI { get; set; }

		[Column("LI_NU_LI_PROTOCOLADA")]
		public long? NumeroLIProtocolo { get; set; }

		[Column("LI_DT_GERACAO")]
		public DateTime? DataGeração { get; set; }

		[Column("LI_DS_MENSAGEM")]
		public string MensagemErro { get; set; }

		[Column("LI_NU_DIAGNOSTICO_ERRO")]
		public byte? CodigoDiagnosticoErro { get; set; }

		[Column("LI_ST")]
		public byte? Status { get; set; }

		[Column("LI_DH_CADASTRO")]
		public DateTime? DataCadastro { get; set; }

		[Column("LI_DH_CANCELAMENTO")]
		public DateTime? DataCancelamento { get; set; }

	}
}