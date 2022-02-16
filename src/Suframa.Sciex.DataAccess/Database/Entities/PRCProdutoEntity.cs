using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PRC_PRODUTO")]
	public partial class PRCProdutoEntity : BaseEntity
	{
		public virtual ProcessoEntity Processo { get; set; }
		public virtual ICollection<PRCInsumoEntity> ListaInsumos { get; set; }
		public virtual ICollection<PRCProdutoPaisEntity> ListaProdutoPais { get; set; }
		public virtual ICollection<PRCHistoricoInsumoEntity> ListaHistoricoInsumo { get; set; }

		public PRCProdutoEntity()
		{
			ListaInsumos = new HashSet<PRCInsumoEntity>();
			ListaProdutoPais = new HashSet<PRCProdutoPaisEntity>();
			ListaHistoricoInsumo = new HashSet<PRCHistoricoInsumoEntity>();
		}

		[Key]
		[Column("pro_id")]
		public int IdProduto { get; set; }	
		
		[ForeignKey(nameof(Processo))]
		[Column("prc_id")]
		public int IdProcesso { get; set; }		

		[Column("pro_co_produto_exportacao")]
		public int? CodigoProdutoExportacao { get; set; }
		
		[Column("pro_co_produto_suframa")]
		public int? CodigoProdutoSuframa { get; set; }

		[StringLength(8)]
		[Column("pro_co_ncm")]
		public string CodigoNCM { get; set; }

		[Column("pro_co_tp_produto")]
		public int? TipoProduto { get; set; }

		[StringLength(255)]
		[Column("pro_ds_modelo")]
		public string DescricaoModelo { get; set; }

		[Column("pro_qt_aprov")]
		public decimal? QuantidadeAprovado { get; set; }
		
		[Column("pro_co_unidade")]
		public int? CodigoUnidade { get; set; }
		
		[Column("pro_vl_dolar_aprov")]
		public decimal? ValorDolarAprovado { get; set; }
		
		[Column("pro_vl_fluxo_caixa")]
		public decimal? ValorFluxoCaixa { get; set; }

		[Column("pro_qt_comp")]
		public decimal? QuantidadeComprovado { get; set; }

		[Column("pro_vl_dolar_comp")]
		public decimal? ValorDolarComprovado { get; set; }

		[Column("pro_vl_nacional_comp")]
		public decimal? ValorNacionalComprovado { get; set; }

	}
}