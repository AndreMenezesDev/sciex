using Suframa.Sciex.DataAccess.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_LI_ARQUIVO")]
	public partial class LiArquivoEntity : BaseEntity
	{
		public virtual LiArquivoRetornoEntity LiArquivoRetorno { get; set; }

		[Key]
		[Column("LAR_ID")]
		[ForeignKey(nameof(LiArquivoRetorno))]
		public long IdLiArquivo { get; set; }

		[Column("LIA_ME_ARQUIVO_LI")]
		public byte[] ArquivoLIRetorno { get; set; }
	}
}