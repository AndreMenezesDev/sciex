using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_LE_PRODUTO")]
	public partial class LEProdutoEntity : BaseEntity
	{
		public virtual ICollection<LEInsumoEntity> LEInsumo { get; set; }
		public LEProdutoEntity()
		{
			LEInsumo = new HashSet<LEInsumoEntity>();
		}

		[Key]
		[Column("LEP_ID")]
		public int IdLe { get; set; }

		[Required]
		[Column("LEP_NU_INSCRICAO_CADASTRAL")]
		public int InscricaoCadastral { get; set; }

		[Required]
		[StringLength(14)]
		[Column("LEP_NU_CNPJ")]
		public string Cnpj { get; set; }

		[Required]
		[Column("LEP_CO_PRODUTO")]
		public int CodigoProduto { get; set; }

		[Column("LEP_CO_PRODUTO_SUFRAMA")]
		public int CodigoProdutoSuframa { get; set; }

		[Column("LEP_CO_TIPO_PRODUTO_SUFRAMA")]
		public int CodigoTipoProduto { get; set; }

		[Column("LEP_CO_UNIDADE")]
		public int CodigoUnidadeMedida { get; set; }

		[Column("LEP_DS_MODELO")]
		[StringLength(255)]
		public string DescricaoModelo { get; set; }

		[Column("LEP_ST_LE")]
		public int StatusLE { get; set; }

		[Column("LEP_ST_LE_ALTERACAO")]
		public int? StatusLEAlteracao { get; set; }

		[Column("LEP_DT_ENVIO")]
		public DateTime? DataEnvio { get; set; }

		[Column("LEP_DT_APROVACAO")]
		public DateTime? DataAprovacao { get; set; }

		[Column("LEP_CO_MODELO_EMPRESA")]
		[StringLength(30)]
		public string CodigoModeloEmpresa { get; set; }

		[Column("LEP_DS_CENTRO_CUSTO")]
		[StringLength(10)]
		public string DescricaoCentroCusto { get; set; }

		[Column("LEP_NU_CPF_RESPONSAVEL")]
		[StringLength(11)]
		public string CpfResponsavel { get; set; }

		[Column("LEP_NO_RESPONSAVEL")]
		[StringLength(80)]
		public string NomeResponsavel { get; set; }

		[Column("LEP_CO_NCM")]
		[StringLength(8)]
		public string CodigoNCM { get; set; }

		[Column("LEP_DT_CADASTRO")]
		public DateTime? DataCadastro { get; set; }

		[Column("LEP_DS_RAZAO_SOCIAL")]
		[StringLength(100)]
		public string RazaoSocial { get; set; }
	}
}