using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_LI_ARQUIVO_RETORNO")]
	public partial class LiArquivoRetornoEntity : BaseEntity
	{
		public virtual LiArquivoEntity LiArquivo { get; set; }

		[Key]
		[Column("LAR_ID")]
		public long IdLiArquivoRetorno { get; set; }

		[Required]
		[StringLength(255)]
		[Column("LAR_DS_NOME_ARQUIVO")]
		public string NomeArquivo { get; set; }

		[Required]
		[Column("LAR_DH_RECEPCAO_ARQUIVO")]
		public DateTime DataRecepcaoArquivo { get; set; }

		[Column("LAR_ST_LEITURA")]				
		public byte? StatusLeituraArquivo { get; set; }

		[Column("LAR_QT_LI")]
		public short? QuantidadeLI { get; set; }

		[Column("LAR_QT_LI_DEFERIDA")]
		public short? QuantidadeLIDeferida { get; set; }

		[Column("LAR_QT_LI_INDEFERIDA")]
		public short? QuantidadeLIIndeferida { get; set; }

		[Column("LAR_QT_LI_ERRO")]
		public short? QuantidadeErroLI { get; set; }

		[Column("LAR_TP_ARQUIVO")]
		public byte? TipoArquivoLI { get; set; }

	}
}