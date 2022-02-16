using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("PRJ_PRODUTO_EMPRESA_EXPORTACAO")]
    public partial class PRJProdutoEmpresaExportacaoEntity : BaseEntity
    {
		[Key]
		[Column("PEX_ID")]
		public int IdProdutoEmpresaExportacao { get; set; }		

		[Column("PEX_CO_PRODUTO")]
		public short CodigoProduto { get; set; }

		[StringLength(255)]
		[Column("PEX_DS_PRODUTO")]
		public string DescricaoProduto { get; set; }

		[Column("PEX_CO_TP_PRODUTO")]
		public short CodigoTipoProduto { get; set; }

		[StringLength(255)]
		[Column("PEX_DS_TP_PRODUTO")]
		public string DescricaoTipoProduto { get; set; }

		[StringLength(14)]
		[Column("PEX_NU_CNPJ")]
		public string Cnpj { get; set; }

		[Column("PEX_NU_INSCRICAO_CADASTRAL")]
		public int InscricaoCadastral { get; set; }

		[Column("PEX_ID_UME")]
		public int IdUnidadeMedida { get; set; }

		[StringLength(8)]
		[Column("PEX_CO_NCM")]
		public string CodigoNCM { get; set; }

		[StringLength(120)]
		[Column("PEX_DS_NCM")]
		public string DescricaoNCM { get; set; }

		[Column("PEX_CO_MODELO")]
		public short? CodigoModelo { get; set; }

		[StringLength(255)]
		[Column("PEX_DS_MODELO")]
		public string DescricaoModelo { get; set; }

	}
}