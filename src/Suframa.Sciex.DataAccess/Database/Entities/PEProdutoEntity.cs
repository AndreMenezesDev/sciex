
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PE_PRODUTO")]
	public partial class PEProdutoEntity : BaseEntity
	{
		public virtual PlanoExportacaoEntity PlanoExportacao { get; set; }
		public virtual ICollection<PEProdutoPaisEntity> ListaPEProdutoPais { get; set; }
		public virtual ICollection<PEInsumoEntity> ListaPEInsumo{ get; set; }
		public PEProdutoEntity()
		{
			ListaPEProdutoPais = new HashSet<PEProdutoPaisEntity>();
			ListaPEInsumo = new HashSet<PEInsumoEntity>();
		}

		[Key]
		[Column("PRO_ID")]
		public int IdPEProduto { get; set; }

		[Column("PEX_ID")]
		[ForeignKey(nameof(PlanoExportacao))]
		public int IdPlanoExportacao { get; set; }

		[Column("PRO_CO_PRODUTO_EXPORTACAO")]
		public int CodigoProdutoExportacao { get; set; }

		[Column("PRO_CO_PRODUTO_SUFRAMA")]
		public int CodigoProdutoSuframa { get; set; }

		[Column("PRO_CO_NCM")]
		[StringLength(8)]
		public string CodigoNCM { get; set; }

		[Column("PRO_CO_TP_PRODUTO")]
		public int CodigoTipoProduto { get; set; }

		[Column("PRO_DS_MODELO")]
		[StringLength(255)]
		public string DescricaoModelo { get; set; }

		[Column("PRO_QT")]
		public decimal Qtd { get; set; }

		[Column("PRO_VL_DOLAR")]
		public decimal ValorDolar{ get; set; }

		[Column("PRO_VL_FLUXO_CAIXA")]
		public decimal ValorFluxoCaixa { get; set; }

		[Column("PRO_CO_UNIDADE")]
		public int CodigoUnidade { get; set; }

		[Column("PRO_VL_NACIONAL")]
		public int? ValorNacional { get; set; }

	}
}