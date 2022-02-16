using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PE_ARQUIVO")]
	public partial class PEArquivoEntity : BaseEntity
	{
		public virtual PlanoExportacaoEntity PlanoExportacao { get; set; }
		public PEArquivoEntity()
		{
		}

		[Key]
		[Column("PEA_ID")]		
		public int IdPlanoExportacaoArquivo { get; set; }


		
		[Column("PEA_NO_ARQUIVO")]
		public string NomeArquivo { get; set; }


		[ForeignKey(nameof(PlanoExportacao))]
		[Column("PEX_ID")]
		public int IdPlanoExportacao { get;set; }

		[Column("PEA_ME_ARQUIVO")]
		public byte[] Anexo { get; set; }

	}
}