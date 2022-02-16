using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SOLIC_PE_DETALHE_INS")]
	public class SolicitacaoPEDetalheEntity : BaseEntity
	{
		public virtual SolicitacaoPEInsumoEntity SolicitacaoPEInsumoEntity { get; set; }
		[Key]
		[Column("sdi_id")]
		public int Id { get; set; }
		[Column("spi_id")]
		[ForeignKey(nameof(SolicitacaoPEInsumoEntity))]
		public int InsumoId { get; set; }
		[Column("sdi_nu_seq")]
		public int Sequencial { get; set; }
		[Column("sdi_co_pais")]
		public string CodigoPais { get; set; }
		[Column("sdi_qt")]
		public decimal? Quantidade { get; set; }
		[Column("sdi_vl_unitario")]
		public decimal? ValorUnitario { get; set; }
		[Column("sdi_vl_frete")]
		public decimal? ValorFrete { get; set; }
		[Column("sdi_co_moeda")]
		public string CodigoMoeda { get; set; }
		[Column("sdi_nu_documento")]
		public string NumeroDocumento { get; set; }
		[Column("sdi_nu_cnpj_remetente")]
		public string CnpjRemetente { get; set; }
		[Column("sdi_dt_emissao_nf")]
		public DateTime? DataEmissaoNF { get; set; }
		[Column("sdi_nu_lote")]
		public int? NumeroLote { get; set; }
		[Column("sdi_nu_ano_lote")]
		public int? AnoLote { get; set; }
		[Column("sdi_nu_inscricao_cadastral")]
		public string InscricaoCadastral { get; set; }
		[Column("sdi_co_produto_exp")]
		public int CodigoProdutoPexPam { get; set; }
		[Column("sdi_co_insumo")]
		public int CodigoInsumo { get; set; }

	}
}
