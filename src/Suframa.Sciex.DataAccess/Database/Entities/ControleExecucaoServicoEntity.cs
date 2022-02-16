using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_CONTROLE_EXEC_SERVICO")]
	public partial class ControleExecucaoServicoEntity : BaseEntity
	{

		public virtual ListaServicoEntity ListaServico { get; set; }

		[Key]
		[Column("CES_ID")]
		public int IdControleExecucaoServico { get; set; }

		[Required]
		[ForeignKey(nameof(ListaServico))]
		[Column("LSE_ID")]
		public int IdListaServico { get; set; }

		[Required]
		[Column("CES_DH_EXECUCAO_INICIO")]
		public DateTime DataHoraExecucaoInicio { get; set; }
		
		[Column("CES_DH_EXECUCAO_FIM")]
		public DateTime? DataHoraExecucaoFim { get; set; }

		[MaxLength]
		[Column("CES_ME_OBJETO_ENVIO")]
		public string MemoObjetoEnvio { get; set; }

		[MaxLength]
		[Column("CES_ME_OBJETO_RETORNO")]
		public string MemoObjetoRetorno { get; set; }

		[Required]
		[Column("CES_ST_EXECUCAO")]
		public int StatusExecucao { get; set; }

		[Required]
		[StringLength(20)]
		[Column("CES_NU_CPF_CNPJ_INTERESSADO")]
		public string NumeroCPFCNPJInteressado { get; set; }

		[Required]
		[StringLength(120)]
		[Column("CES_NO_CPF_CNPJ_INTERESSADO")]
		public string NomeCPFCNPJInteressado { get; set; }


	}
}