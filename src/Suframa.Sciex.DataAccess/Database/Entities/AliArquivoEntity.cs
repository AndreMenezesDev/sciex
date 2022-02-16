using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ALI_ARQUIVO")]
	public partial class AliArquivoEntity : BaseEntity
	{
		public virtual AliArquivoEnvioEntity AliArquivoEnvio { get; set; }

		[Key]
		[ForeignKey(nameof(AliArquivoEnvio))]
		[Column("AAE_ID")]
		public long IdAliArquivoEnvio { get; set; }

		[Required]
		[Column("AAR_ME_ARQUIVO_ALI")]
		public byte[] Arquivo { get; set; }

	}
}
