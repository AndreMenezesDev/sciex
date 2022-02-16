
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PE_PRODUTO_PAIS")]
	public partial class PEProdutoPaisEntity : BaseEntity
	{
		public virtual PEProdutoEntity PlanoExportacaoProduto { get; set; }
		public virtual ICollection<PlanoExportacaoDUEEntity> ListaPEDue { get; set; }
		public PEProdutoPaisEntity()
		{
			ListaPEDue = new HashSet<PlanoExportacaoDUEEntity>();
		}

		[Key]
		[Column("PRP_ID")]
		public int IdPEProdutoPais { get; set; }

		[Column("PRO_ID")]
		[ForeignKey(nameof(PlanoExportacaoProduto))]
		public int? IdPEProduto { get; set; }

		[Column("PRP_QT")]
		public decimal Quantidade { get; set; }

		[Column("PRP_VL_DOLAR")]
		public decimal ValorDolar { get; set; }

		[Column("PAI_CO")]
		public int CodigoPais { get; set; }
		
		[Column("PRP_ST_ANALISE")]
		public int? SituacaoAnalise { get; set; }

		[StringLength(1000)]
		[Column("PRP_DS_JUSTIFICATIVA_ERRO")]
		public string DescricaoJustificativaErro { get; set; }

	}
}