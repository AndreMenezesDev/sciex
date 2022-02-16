using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_LE_PRODUTO_HISTORICO")]
	public partial class LEProdutoHistoricoEntity : BaseEntity
	{
		public virtual LEProdutoEntity LEProduto { get; set; }

		public LEProdutoHistoricoEntity()
		{
			
		}

		[Key]
		[Column("LPH_ID")]
		public int IdLeh { get; set; }

		[Column("LPH_DT_OCORRENCIA")]
		public DateTime? DataOcorrencia { get; set; }

		[StringLength(14)]
		[Column("LPH_NU_CPFCNPJ_RESPONSAVEL")]
		public string CpfCnpjResponsavel { get; set; }

		[Column("LPH_NO_RESPONSAVEL")]
		[StringLength(80)]
		public string NomeResponsavel { get; set; }

		[Column("LPH_DS_OBSERVACAO")]
		[StringLength(1000)]
		public string DescricaoObservacao { get; set; }

		[Column("LPH_ST_LE")]
		public int SituacaoLE { get; set; }

		[Column("LPH_ST_LE_ALTERACAO")]
		public int SituacaoLEAlteracao { get; set; }

		[Required]
		[Column("LEP_ID")]
		[ForeignKey(nameof(LEProduto))]
		public int IdLe { get; set; }
	}
}