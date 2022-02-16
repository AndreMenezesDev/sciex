using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PRC_SOLIC_DETALHE")]
	public class PRCSolicDetalheEntity : BaseEntity
	{
		public virtual PRCInsumoEntity PrcInsumo { get; set; }
		public virtual PRCDetalheInsumoEntity PRCDetalheInsumo { get; set; }
		public virtual PRCSolicitacaoAlteracaoEntity PRCSolicitacaoAlteracao { get; set; }
		public virtual TipoSolicAlteracaoEntity TipoSolicAlteracao { get; set; }

		[Key]
		[Column("SOD_ID")]
		public int Id { get; set; }

		[Column("SOD_ST")]
		public int Status { get; set; }

		[Column("SOD_DS_DE")]
		public string DescricaoDe { get; set; }

		[Column("SOD_DS_PARA")]
		public string DescricaoPara { get; set; }
		
		[Column("SOD_DS_JUSTIFICATIVA")]
		public string Justificativa { get; set; }

		[ForeignKey(nameof(PrcInsumo))]
		[Column("INS_ID")]
		public int IdInsumo { get; set; }

		[ForeignKey(nameof(PRCDetalheInsumo))]
		[Column("DET_ID")]
		public int? IdDetalheInsumo { get; set; }

		[ForeignKey(nameof(PRCSolicitacaoAlteracao))]
		[Column("SOA_ID")]
		public int IdSolicitacaoAlteracao { get; set; }

		[ForeignKey(nameof(TipoSolicAlteracao))]
		[Column("TSO_ID")]
		public int IdTipoSolicitacao { get; set; }

	}
}
