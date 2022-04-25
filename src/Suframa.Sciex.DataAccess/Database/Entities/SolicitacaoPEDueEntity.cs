using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	
	[Table("SCIEX_SOLIC_PE_DUE")]
	public class SolicitacaoPEDueEntity : BaseEntity
	{
		public virtual SolicitacaoPEProdutoPaisEntity SolicitacaoPEProdutoPais { get; set; }

		[Key]
		[Column("sdu_id")]
		public int IdDue { get; set; }

		[ForeignKey(nameof(SolicitacaoPEProdutoPais))]
		[Column("prp_id")]
		public int? IdProdutoPais { get; set; }

		[Column("sdu_nu")]
		[StringLength(15)]
		public string Numero{ get; set; }
		
		[Column("sdu_dt_averbacao")]
		public DateTime? DataAverbacao { get; set; }

		[Column("sdu_valor_dolar")]
		public decimal? ValorDolar { get; set; }

		[Column("sdu_qt")]
		public decimal? Quantidade { get; set; }

		[Column("sdu_co_pais")]
		public int? CodigoPais { get; set; }

		[Column("sdu_nu_lote")]
		public int? NumeroLote { get; set; }

		[Column("sdu_nu_ano_lote")]
		public int? NumeroAnoLote { get; set; }

		[Column("sdu_co_produto_exp")]
		public int? CodigoProdutoExportacao { get; set; }

		[Column("sdu_nu_inscricao_cadastral")]
		[StringLength(9)]
		public string InscricaoCadastral { get; set; }

	}
}
