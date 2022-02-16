using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	
	[Table("SCIEX_SOLIC_PE_INSUMO")]
	public class SolicitacaoPEInsumoEntity : BaseEntity
	{
		public virtual ICollection<SolicitacaoPEDetalheEntity> Detalhes { get; set; }

		public virtual SolicitacaoPEProdutoEntity SolicitacaoPEProdutoEntity { get; set; }

		public SolicitacaoPEInsumoEntity()
		{
			Detalhes = new HashSet<SolicitacaoPEDetalheEntity>();
		}

		[Key]
		[Column("spi_id")]
		public int Id { get; set; }
		[Column("spp_id")]
		[ForeignKey(nameof(SolicitacaoPEProdutoEntity))]
		public int ProdutoId { get; set; }
		[Column("spi_co_insumo")]
		public int CodigoInsumo { get; set; }
		[Column("spi_co_unidade")]
		public string CodigoUnidade { get; set; }
		[Column("spi_tp_insumo")]
		public string CodigoTipoInsumo { get; set; }
		[Column("spi_co_ncm")]
		public string CodigoNCM { get; set; }
		[Column("spi_vl_percentual_perda")]
		public decimal? ValorPctPerda { get; set; }
		[Column("spi_co_detalhe")]
		public string CodigoDetalhe { get; set; }
		[Column("spi_ds_insumo")]
		public string DescricaoInsumo { get; set; }
		[Column("spi_ds_part_number")]
		public string DescricaoPartNumber { get; set; }
		[Column("spi_ds_especificacao_tecnica")]
		public string DescricaoEspTecnica { get; set; }
		[Column("spi_vl_coeficiente_tecnico")]
		public decimal? ValorCoeficienteTecnico { get; set; }
		[Column("spi_co_produto_exp")]
		public int CodigoProdutoPexPam { get; set; }
		[Column("spi_nu_lote")]
		public int? NumeroLote { get; set; }
		[Column("spi_nu_ano_lote")]
		public int? AnoLote { get; set; }
		[Column("spi_nu_inscricao_suframa")]
		public string InscricaoCadastral { get; set; }
	}
}
