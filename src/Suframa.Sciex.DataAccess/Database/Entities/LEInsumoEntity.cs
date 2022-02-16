using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_LE_INSUMO")]
	public partial class LEInsumoEntity : BaseEntity
	{
		public virtual LEProdutoEntity LEProduto { get; set; }
		public virtual ICollection<LEInsumoErroEntity> listaLEInsumoErro { get; set; }

		public LEInsumoEntity()
		{
			listaLEInsumoErro = new HashSet<LEInsumoErroEntity>();
		}

		[Key]
		[Column("LEI_ID")]
		public int IdLeInsumo { get; set; }

		[Required]
		[Column("LEI_CO_PRODUTO")]
		public int CodigoProduto { get; set; }

		[Required]
		[Column("LEI_CO_INSUMO")]
		public int CodigoInsumo { get; set; }

		[Column("LEI_CO_NCM")]
		[StringLength(8)]
		public string CodigoNCM { get; set; }

		[Column("LEI_TP")]
		[StringLength(1)]
		public string TipoInsumo { get; set; }

		[Column("LEI_TP_ALTERACAO")]

		public int? TipoInsumoAlteracao { get; set; }

		[Column("LEI_CO_UNIDADE")]
		
		public int CodigoUnidadeMedida { get; set; }

		[Column("LEI_CO_DETALHE")]
		public int CodigoDetalhe { get; set; }

		[Column("LEI_DS_INSUMO")]
		[StringLength(255)]
		public string DescricaoInsumo { get; set; }

		[Column("LEI_VL_COEFICIENTE_TECNICO")]
		public decimal ValorCoeficienteTecnico { get; set; }

		[Column("LEI_ST_INSUMO")]
		public int? SituacaoInsumo { get; set; }

		[Column("LEI_CO_PART_NUMBER")]
		[StringLength(30)]
		public string CodigoPartNumber { get; set; }

		[Column("LEI_DS_ESPECIFICACAO_TECNICA")]
		[StringLength(3723)]
		public string DescricaoEspecTecnica { get; set; }

		[Required]
		[Column("LEP_ID")]
		[ForeignKey(nameof(LEProduto))]
		public int IdLe { get; set; }
	}
}