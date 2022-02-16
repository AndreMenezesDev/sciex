using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	
	[Table("SCIEX_SOLIC_PE_PRODUTO")]
	public class SolicitacaoPEProdutoEntity : BaseEntity
	{
		public virtual SolicitacaoPELoteEntity SolicitacaoPELote { get; set; }
		public virtual ICollection<SolicitacaoPaisProdutoEntity> PaisProduto { get; set; }
		public virtual ICollection<SolicitacaoPEInsumoEntity> Insumos { get; set; }
		public SolicitacaoPEProdutoEntity()
		{
			PaisProduto = new HashSet<SolicitacaoPaisProdutoEntity>();
			Insumos = new HashSet<SolicitacaoPEInsumoEntity>();
		}

		[Key]
		[Column("spp_id")]
		public int Id { get; set; }
		[Column("slo_id")]
		[ForeignKey(nameof(SolicitacaoPELote))]
		public int LoteId { get; set; }
		[Column("spp_co_produto_exp")]
		public int CodigoProdutoPexPam { get; set; }
		[Column("spp_co_produto_suframa")]
		public string CodigoProdutoSuframa { get; set; }
		[Column("spp_co_ncm")]
		public string CodigoNCM { get; set; }
		[Column("spp_co_tp_produto")]
		public string CodigoTipoProduto { get; set; }
		[Column("spp_ds_modelo")]
		public string DescricaoModelo { get; set; }
		[Column("spp_qt")]
		public decimal? Quantidade { get; set; }
		[Column("spp_vl_dolar")]
		public decimal? ValorDolar { get; set; }
		[Column("spp_vl_nacional")]
		public decimal? ValorNacional { get; set; }
		[Column("spp_co_unidade")]
		public string CodigoUnidade { get; set; }
		[Column("spp_nu_lote")]
		public int? NumeroLote { get; set; }
		[Column("spp_nu_ano_lote")]
		public int? AnoLote { get; set; }
		[Column("spp_nu_inscricao_cadastral")]
		public string InscricaoCadastral { get; set; }
		[Column("spp_st_validacao")]
		public int? SituacaoValidacao { get; set; }

	}
}
