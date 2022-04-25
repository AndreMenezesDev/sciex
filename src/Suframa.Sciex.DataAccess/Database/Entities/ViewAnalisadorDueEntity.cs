using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("[VW_SCIEX_EMITIR_RELATORIO_ANALISADOR_DUE]")]
	public partial class ViewEmitirRelatorioAnalisadorDueEntity : BaseEntity
	{
		[Key]
		[Column("nu_inscricao_cadastral")]
		public int NumeroIncricaoCadastral { get; set; }

		[Column("razao_social")]
		public string RazaoSocial { get; set; }

		[Column("nu_plano")]
		public int NumeroPlano { get; set; }

		[Column("nu_ano_plano")]
		public int AnoPlano { get; set; }

		[Column("nu_ano_processo")]
		public int NuAnoProcesso { get; set; }

		[Column("nu_processo")]
		public int NumeroProcesso { get; set; }

		[Column("ano_processo")]
		public int AnoProcesso { get; set; }

		[Column("status_plano")]
		public int StatusPlano { get; set; }

		[Column("dt_status")]
		public DateTime DataStatus { get; set; }

		[Column("due_nu")]
		public string NumeroDue { get; set; }

		[Column("due_vl_dolar")]
		public decimal ValorDolar { get; set; }

		[Column("qt_due")]
		public decimal QuantidadeDue { get; set; }


	}
}