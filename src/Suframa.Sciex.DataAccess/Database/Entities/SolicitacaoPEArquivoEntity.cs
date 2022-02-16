using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SOLIC_PE_ARQUIVO")]
	public class SolicitacaoPEArquivoEntity : BaseEntity
	{
		public virtual EstruturaPropriaPliEntity EstruturaPropria { get; set; }
		public virtual SolicitacaoPELoteEntity SolicitacaoPELote { get; set; }
		public SolicitacaoPEArquivoEntity()
		{

		}

		[Key]
		[Column("saq_id")]
		public int Id { get; set; }

		[Column("saq_me_arquivo")]
		public byte[] Arquivo { get; set; }

		[Column("saq_no_arquivo")]
		public string NomeArquivo { get; set; }

		[ForeignKey(nameof(SolicitacaoPELote))]
		[Column("slo_id")]
		public int? IdPELote { get; set; }

		[Column("esp_id")]
		[ForeignKey(nameof(EstruturaPropria))]
		public long? EspId { get; set; }

		

	}
}
