using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PE_DUE")]
	public partial class PlanoExportacaoDUEEntity : BaseEntity
	{
		public virtual PEProdutoPaisEntity PEProdutoPais { get; set; }

		[Key]
		[Column("DUE_ID")]
		public int IdDue { get; set; }

		[Column("PRP_ID")]
		[ForeignKey(nameof(PEProdutoPais))]
		public int? IdPEProdutoPais { get; set; }

		[Column("PAI_CO")]
		public int CodigoPais { get; set; }

		[Column("DUE_NU")]
		[StringLength(15)]
		public string Numero { get; set; }

		[Column("DUE_DT_AVERBACAO")]
		public DateTime DataAverbacao { get; set; }

		[Column("DUE_VL_DOLAR")]
		public decimal ValorDolar { get; set; }

		[Column("DUE_QT")]
		public decimal Quantidade { get; set; }

		[Column("DUE_ST_ANALISE")]
		public int? SituacaoAnalise { get; set; }

		[Column("DUE_DS_JUSTIFICATIVA_ERRO")]
		[StringLength(1000)]
		public string DescricaoJustificativa { get; set; }

	}
}