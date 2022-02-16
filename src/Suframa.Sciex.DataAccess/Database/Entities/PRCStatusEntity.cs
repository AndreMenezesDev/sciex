using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("SCIEX_PRC_STATUS")]
    public partial class PRCStatusEntity : BaseEntity
    {
		public virtual ProcessoEntity Processo { get; set; }
		public virtual PRCSolicitacaoAlteracaoEntity SolicitacaoAlteracao { get; set; }

		[Key]
		[Column("sta_id")]
		public int IdStatus { get; set; }		

		[ForeignKey(nameof(Processo))]
		[Column("prc_id")]
		public int IdProcesso { get; set; }

		[ForeignKey(nameof(SolicitacaoAlteracao))]
		[Column("soa_id")]
		public int? IdSolicitacaoAlteracao { get; set; }

		[StringLength(2)]
		[Column("sta_tp")]
		public string Tipo { get; set; }
		
		[Column("sta_dt")]
		public DateTime? Data { get; set; }

		[Column("sta_dt_validade")]
		public DateTime? DataValidade { get; set; }

		[StringLength(11)]
		[Column("sta_nu_cpf_responsavel")]
		public string CpfResponsavel { get; set; }

		[StringLength(100)]
		[Column("sta_no_responsavel")]
		public string NomeResponsavel { get; set; }

		[Column("sta_nu_plano")]
		public int? NumeroPlano { get; set; }
		
		[Column("sta_nu_ano_plano")]
		public int? AnoPlano { get; set; }
		[Column("sta_ds_observacao")]
		public string DescricaoObservacao { get; set; }

	}
}