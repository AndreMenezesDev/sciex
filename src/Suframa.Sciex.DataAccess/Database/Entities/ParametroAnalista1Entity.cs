using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PARAMETRO_ANALISTA")]
	public partial class ParametroAnalista1Entity : BaseEntity
	{
		public virtual AnalistaEntity Analista { get; set; }

		[Key]
		[Column("PAN_ID")]
		public int IdParametroAnalista { get; set; }

		[ForeignKey(nameof(Analista))]
		[Column("ANA_ID")]
		public int IdAnalista { get; set; }

		[Required]
		[Column("PAN_ST_VISUAL")]
		public int StatusAnaliseVisual { get; set; }

		[Required]
		[Column("PAN_DH_VISUAL_INICIO")]
		public DateTime? DataAnaliseVisualInicio { get; set; }

		[Column("PAN_HR_VISUAL_INICIO")]
		public TimeSpan? HoraAnaliseVisualInicio { get; set; }

		[Column("PAN_HR_VISUAL_FIM")]
		public TimeSpan? HoraAnaliseVisualFim { get; set; }

		[Required]
		[Column("PAN_ST_VISUAL_BLOQUEIO")]
		public int StatusAnaliseVisualBloqueio { get; set; }

		[Column("PAN_DH_VISUAL_BLOQUEIO_INICIO")]
		public DateTime? DataAnaliseVisualBloqueioInicio { get; set; }

		[Column("PAN_HR_VISUAL_BLOQ_INICIO")]
		public TimeSpan? HoraAnaliseVisualBloqueioInicio { get; set; }

		[Column("PAN_HR_VISUAL_BLOQ_FIM")]
		public TimeSpan? HoraAnaliseVisualBloqueioFim { get; set; }

		[StringLength(120)]
		[Column("PAN_DS_VISUAL_BLOQ_PLI")]
		public string DescricaoAnaliseVisualBloqueioFim { get; set; }

		[Required]
		[Column("PAN_ST_LOTE_LISTAGEM")]
		public int StatusAnaliseLoteListagem { get; set; }

		[Column("PAN_DH_LOTE_INICIO")]
		public DateTime? DataAnaliseLoteListagemInicio { get; set; }

		[Column("PAN_HR_LOTE_INICIO")]
		public TimeSpan? HoraAnaliseLoteListagemInicio { get; set; }

		[Column("PAN_HR_LOTE_FIM")]
		public TimeSpan? HoraAnaliseLoteListagemFim { get; set; }

		[Required]
		[Column("PAN_ST_LOTE_BLOQUEIO")]
		public int StatusAnaliseLoteListagemBloqueio { get; set; }

		[Column("PAN_DH_LOTE_BLOQUEIO_INICIO")]
		public DateTime? DataAnaliseListagemLoteBloqueioInicio { get; set; }

		[Column("PAN_HR_LOTE_BLOQUEIO_INICIO")]
		public TimeSpan? HoraAnaliseLoteListagemBloqueioInicio { get; set; }

		[Column("PAN_HR_LOTE_BLOQUEIO_FIM")]
		public TimeSpan? HoraAnaliseLoteListagemBloqueioFim { get; set; }

		[StringLength(60)]
		[Column("PAN_DS_LOTE_BLOQ_PLI")]
		public string DescricaoAnaliseLoteListagemBloqueioFim { get; set; }
	}
}