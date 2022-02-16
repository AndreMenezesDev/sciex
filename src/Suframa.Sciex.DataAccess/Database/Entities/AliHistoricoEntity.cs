using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ALI_HISTORICO")]
	public partial class AliHistoricoEntity : BaseEntity
	{

		public virtual PliMercadoriaEntity PliMercadoria { get; set; }


		[Key]
		[Column("AHI_ID")]
		public long IdAliHistorico { get; set; }

		[Column("PME_ID")]
		[ForeignKey(nameof(PliMercadoria))]
		public long IdPliMercadoria { get; set; }

		[Required]
		[Column("AHI_ST_ALI_ANTERIOR")]		
		public byte StatusAliAnterior { get; set; }

		[Column("AHI_ST_LI_ANTERIOR")]
		public byte? StatusLiAnterior { get; set; }

		[Required]
		[Column("AHI_DH_OPERACAO")]
		public DateTime DataOperacao { get; set; }

		[Required]
		[StringLength(14)]
		[Column("AHI_NU_CPFCNPJ_RESPONSAVEL")]
		public string CPFCNPJResponsavel { get; set; }

		[Required]
		[StringLength(100)]
		[Column("AHI_NO_RESPONSAVEL")]
		public string NomeResponsavel { get; set; }
		
		[StringLength(255)]
		[Column("AHI_DS_OBSERVACAO")]
		public string Observacao { get; set; }
	}
}