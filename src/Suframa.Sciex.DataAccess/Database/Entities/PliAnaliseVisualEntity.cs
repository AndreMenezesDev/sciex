using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PLI_ANALISE_VISUAL")]
	public partial class PliAnaliseVisualEntity : BaseEntity
	{
		public virtual PliEntity Pli { get; set; }
		public virtual CodigoUtilizacaoEntity CodigoUtilizacao { get; set; }
		public virtual CodigoContaEntity CodigoConta { get; set; }


		public PliAnaliseVisualEntity()
		{
			
		}

		[Key]
		[Column("PLI_ID")]
		[ForeignKey(nameof(Pli))]
		public long IdPLI { get; set; }

		[Column("PAV_ST_ANALISE")]
		public Decimal? StatusAnalise { get; set; }

		[StringLength(500)]
		[Column("PAV_DS_MOTIVO")]
		public string DescricaoMotivo { get; set; }

		[Column("CUT_ID")]
		[ForeignKey(nameof(CodigoUtilizacao))]
		public int? IdCodigoUtilizacao { get; set; }

		[Column("CCO_ID")]
		[ForeignKey(nameof(CodigoConta))]
		public int? IdCodigoConta { get; set; }

		[Column("PAV_DH_ANALISE")]
		public DateTime? DataAnalise { get; set; }

		[Column("PAV_DH_PENDENCIA")]
		public DateTime? DataPendencia { get; set; }

		[StringLength(500)]
		[Column("PAV_DS_RESPOSTA")]
		public string DescricaoResposta { get; set; }

		[StringLength(100)]
		[Column("PAV_NO_USER")]
		public string NomeAnalista { get; set; }
		
		[StringLength(14)]
		[Column("PAV_NU_USER")]
		public string CpfAnalista { get; set; }
	}
}