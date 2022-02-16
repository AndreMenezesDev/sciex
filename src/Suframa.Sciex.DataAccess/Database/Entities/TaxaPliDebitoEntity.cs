using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_TAXA_PLI_DEBITO")]
	public partial class TaxaPliDebitoEntity : BaseEntity
	{
		public virtual TaxaPliEntity TaxaPli { get; set; }

		public TaxaPliDebitoEntity()
		{

		}

		[Key]
		[Column("tpd_id")]
		public int IdDebito { get; set; }

		[Column("pli_id")]
		[ForeignKey(nameof(TaxaPli))]
		public long? IdPli { get; set; }
		
		[Column("tpd_nu_controle_cobranca_tcif")]
		public short? NumeroControleCobrancaTCIF { get; set; }

		[Column("tpd_nu_debito")]
		public int? NumeroDebito { get; set; }

		[Column("tpd_nu_debito_ano")]
		public short? AnoDebito { get; set; }

		[Column("tpd_dt_debito_vencto")]
		public DateTime? DataDebitoVencimento { get; set; }

		[Column("tpd_dt_debito_pagto")]
		public DateTime? DataDebitoPagamento { get; set; }

		[Column("tpd_dt_debito_cancelamento")]
		public DateTime? DataDebitoCancelamento { get; set; }

		[Column("tpd_dt_debito_fim_acao")]
		public DateTime? DataDebitoFimAcao { get; set; }
	}
}