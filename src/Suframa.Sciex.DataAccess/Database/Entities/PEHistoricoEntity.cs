
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PE_HISTORICO")]
	public partial class PEHistoricoEntity : BaseEntity
	{
		public virtual PlanoExportacaoEntity PlanoExportacao { get; set; }
		public PEHistoricoEntity()
		{
		}

		[Key]
		[Column("PEH_ID")]
		public int IdPEHistorico { get; set; }

		[Column("PEX_ID")]
		[ForeignKey(nameof(PlanoExportacao))]
		public int? IdPlanoExportacao { get; set; }

		[Column("PEH_DT")]
		public DateTime? Data { get; set; }

		[Column("PEH_NU_CPF_RESPONSAVEL")]
		[StringLength(11)]
		public string CpfResponsavel { get; set; }

		[Column("PEH_NO_RESPONSAVEL")]
		[StringLength(100)]
		public string NomeResponsavel { get; set; }

		[Column("PEH_DS_OBSERVACAO")]
		[StringLength(1000)]
		public string DescricaoObservacao { get; set; }

		[Column("PEH_ST_PLANO")]
		public int SituacaoPlano { get; set; }

	}
}