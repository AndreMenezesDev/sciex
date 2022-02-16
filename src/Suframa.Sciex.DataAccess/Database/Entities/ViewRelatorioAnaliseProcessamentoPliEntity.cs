using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("VW_SCIEX_RELATORIO_ANALISE_PROCESSAMENTO_PLI")]
    public partial class ViewRelatorioAnaliseProcessamentoPliEntity : BaseEntity
    {
		[Key]
		[Column("PLI_ID")]
		public long IdPli { get; set; }

		[StringLength(8000)]
		[Column("NUMPLI")]
		public string NumeroPli { get; set; }

		[Column("NUMALI")]
        public long? NumeroAli { get; set; }

		[Required]
		[Column("PLIMERCADORIA")]
		public long IdPliMercadoria { get; set; }

		[StringLength(8)]
		[Column("NCM")]
		public string NomenclaturaComumMercosul { get; set; }
	
		[StringLength(8000)]
		[Column("CODIGOPRODUTO")]
		public string CodigoProduto { get; set; }

		[StringLength(8000)]
		[Column("TIPOPRODUTO")]
		public string TipoProduto { get; set; }

		[StringLength(8000)]
		[Column("MODELOPRODUTO")]
		public string ModeloProduto { get; set; }

		[Column("ITEM")]
		public int? Item { get; set; }

		[StringLength(500)]
		[Column("ERRO_MENSAGEM")]
		public string DescricaoErroProcessamento { get; set; }

		[StringLength(500)]
		[Column("ORIGEM")]
		public string Origem { get; set; }

		[Column("QTD_ERRO")]
		public int? QuantidadeErro { get; set; }

		[StringLength(27)]
		[Column("STATUS")]
		public string Status { get; set; }

	}
}