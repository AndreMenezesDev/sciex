using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("VW_PRJ_PRODUTO_EMPRESA")]
    public partial class ViewProdutoEmpresaEntity : BaseEntity
    {
		[Key]
		[Column("PEM_ID")]
		public int IdProdutoEmpresa { get; set; }		

		[Column("PEM_CO_PRODUTO")]
		public short CodigoProduto { get; set; }

		[Column("PEM_CO_TP_PRODUTO")]
		public short CodigoTipoProduto { get; set; }

		[Column("PEM_CO_MODELO_PRODUTO")]
		public short CodigoModeloProduto { get; set; }

		[StringLength(11)]
		[Column("PEM_CO_PRODUTO_ZFM")]
		public string CodigoProdutoZFM { get; set; }

		[Required]
		[StringLength(255)]
		[Column("PEM_DS_PRODUTO")]
		public string Descricao { get; set; }

		[StringLength(14)]
		[Column("PEM_NU_CNPJ")]
		public string Cnpj { get; set; }

		[Column("PEM_NU_INSCRICAO_CADASTRAL")]
		public int InscricaoCadastral { get; set; }

		[Column("PEM_CO_UTILIZACAO")]
		public int CodigoUltilizacao { get; set; }

		[Column("PEM_ID_UME")]
		public int UME { get; set; }

		[Column("PEM_CO_CRII")]
		public byte CRII { get; set; }

		[StringLength(20)]
		[Column("PEM_DS_CRII")]
		public string DescricaoCRII { get; set; }
    }
}