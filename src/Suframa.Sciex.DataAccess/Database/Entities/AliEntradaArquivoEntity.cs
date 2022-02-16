using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ALI_ENTRADA_ARQUIVO")]
	public partial class AliEntradaArquivoEntity : BaseEntity
	{

		public virtual PliMercadoriaEntity PliMercadoria { get; set; }
		public virtual AliArquivoEnvioEntity AliArquivoEnvio { get; set; }


		[Key]
		[Column("AEA_ID")]
		public long IdAliEntradaArquivo { get; set; }

		[Column("AAE_ID")]
		[ForeignKey(nameof(AliArquivoEnvio))]
		public long IdAliArquivoEnvio { get; set; }

		[Column("PME_ID")]
		[ForeignKey(nameof(PliMercadoria))]
		public long IdPliMercadoria { get; set; }

		[Column("AEA_DH_ARQUIVO_ENVIO")]
		public DateTime? DataEnvioArquivoRetorno { get; set; }
	}
}
