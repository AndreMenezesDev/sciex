using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ANALISTA")]
	public partial class AnalistaEntity : BaseEntity
	{
		public AnalistaEntity()
		{

		}

		[Key]
		[Column("ANA_ID")]
		public int IdAnalista { get; set; }

		[Required]
		[StringLength(11)]
		[Column("ANA_NU_CPF")]
		public string CPF { get; set; }

		[Required]
		[StringLength(120)]
		[Column("ANA_NO")]
		public string Nome { get; set; }

		[Required]
		[StringLength(20)]
		[Column("ANA_SG_SETOR")]
		public string SiglaSetor { get; set; }

		[Required]
		[StringLength(120)]
		[Column("ANA_DS_SETOR")]
		public string DescricaoSetor { get; set; }

		[Required]
		[Column("ANA_DH_SINCRONIZACAO")]
		public DateTime DataHoraSincronizacao { get; set; }

		[Column("ANA_ST_VISUAL")]
		public byte? SituacaoVisual { get; set; }

		[Column("ANA_ST_VISUAL_SETADO")]
		public byte? SituacaoVisualSetada { get; set; }

		[Column("ANA_ST_LE")]
		public byte? SituacaoLE { get; set; }

		[Column("ANA_ST_LE_SETADO")]
		public byte? SituacaoLESetado { get; set; }

		[Column("ANA_ST_PLANO")]
		public byte? SituacaoPlano { get; set; }

		[Column("ANA_ST_PLANO_SETADO")]
		public byte? SituacaoPlanoSetado { get; set; }

		[Column("ANA_ST_SOLICITACAO")]
		public byte? Solicitacao { get; set; }

		[Column("ANA_ST_SOLICITACAO_SETADO")]
		public byte? SituacaoSolicitacaoSetado { get; set; }
	}
}