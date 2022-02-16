using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PRC_SOLIC_PRORROGACAO")]
	public partial class PRCSolicProrrogacaoEntity : BaseEntity
	{

		public virtual ProcessoEntity Processo { get; set; }


		[Column("prc_id")]
		[ForeignKey(nameof(Processo))]
		public int IdProcesso { get; set; }
		
		[Key]
		[Column("sop_id")]
		public int IdSolicProrrogacao { get; set; }
		[StringLength(500)]
		[Column("sop_ds_justificativa_reprovado")]
		public string JustificativaReprovado { get; set; }

		[Column("sop_dt")]
		public DateTime? Data { get; set; }
		
		[Column("sop_st")]
		public int? Status { get; set; }
		
		[StringLength(11)]
		[Column("sop_nu_cpf_responsavel")]
		public string CpfResponsavel { get; set; }

		[StringLength(100)]
		[Column("sop_no_responsavel")]
		public string NumeroResponsavel { get; set; }

		[StringLength(500)]
		[Column("sop_ds_justificativa")]
		public string Justificativa { get; set; }

		



	}
}