using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_CONTROLE_IMPORTACAO")]
	public partial class ControleImportacaoEntity : BaseEntity
	{
		public virtual CodigoContaEntity CodigoConta { get; set; }
		public virtual CodigoUtilizacaoEntity CodigoUtilizacao { get; set; }
		public virtual PliAplicacaoEntity PliAplicacao { get; set; }		

		[Key]
		[Column("CIM_ID")]
		public int IdControleImportacao { get; set; }

		[Required]
		[Column("CCO_ID")]
		[ForeignKey(nameof(CodigoConta))]
		public int IdCodigoConta { get; set; }

		[Required]
		[Column("CUT_ID")]
		[ForeignKey(nameof(CodigoUtilizacao))]
		public int IdCodigoUtilizacao { get; set; }

		[Required]
		[Column("CIM_ST")]
		public byte Status { get; set; }

		[Required]
		[Column("PAP_ID")]
		[ForeignKey(nameof(PliAplicacao))]
		public int IdPliAplicacao { get; set; }

		[Required]
		[Column("CIM_CO_SETOR")]
		public int CodigoSetor { get; set; }

		[Required]
		[StringLength(200)]
		[Column("CIM_DS_SETOR")]
		public string DescricaoSetor { get; set; }

		[Column("CIM_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

	}
}