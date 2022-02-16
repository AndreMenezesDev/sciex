using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_DI_ARQUIVO")]
	public partial class DiArquivoEntity : BaseEntity
	{
		public virtual DiArquivoEntradaEntity DiArquivoEntrada { get; set; }

		[Key]
		[ForeignKey(nameof(DiArquivoEntrada))]
		[Column("DAR_ID")]
		public long Id { get; set; }

		[Column("DIA_ME_ARQUIVO_DI")]
		public byte[] Arquivo { get; set; }

	}
}