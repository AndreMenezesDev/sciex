using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database
{
	[Table("SCIEX_ESTRUTURA_PROPRIA_ARQUIVO")]
	public class EstruturaPropriaPliArquivoEntity : BaseEntity
	{
		public virtual EstruturaPropriaPliEntity EstruturaPropriaPli { get; set; }

		[Key]
		[Column("ESP_ID")]
		[ForeignKey(nameof(EstruturaPropriaPli))]
		public long IdEstruturaPropria { get; set; }

		[Required]
		[Column("ESA_ME_ARQUIVO_ESTRUTURA_PROPRIA")]
		public byte[] Arquivo { get; set; }

	}
}
