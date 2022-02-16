using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CADSUF_CONTROLE_EXEC_JOB")]
	public partial class ControleExecucaoJobEntity : BaseEntity
	{
		[Column("CEJ_DS_CPFCNPJ_INSC_CRED")]
		[StringLength(8000)]
		public string CpfCnpjInscricaoCredenciamento { get; set; }

		[Column("CEJ_DH_EXECUCAO_FIM")]
		public DateTime? DataExecucaoFim { get; set; }

		[Column("CEJ_DH_EXECUCAO_INICIO")]
		public DateTime? DataExecucaoInicio { get; set; }

		[Key]
		[Column("CEJ_ID")]
		public int IdControleExecucaoJob { get; set; }

		[Column("LSE_ID")]
		[ForeignKey(nameof(ListaServico))]
		public int IdListaServico { get; set; }

		public virtual ListaServicoEntity ListaServico { get; set; }

		[Column("CEJ_DS_NOME_INSC_CRED")]
		[StringLength(8000)]
		public string NomeRazaoInscricaoCredenciamento { get; set; }

		[Column("CEJ_QT_REGISTRO_AFETADO")]
		public int? QuantidadeRegistrosAfetados { get; set; }

		[Column("CEJ_ST_EXECUCAO")]
		public int? Status { get; set; }
	}
}