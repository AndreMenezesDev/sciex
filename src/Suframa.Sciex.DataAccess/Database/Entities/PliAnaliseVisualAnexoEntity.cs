using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PLI_ANALISE_VISUAL_ANEXO")]
	public partial class PliAnaliseVisualAnexoEntity : BaseEntity
	{
		public virtual PliEntity Pli { get; set; }


		public PliAnaliseVisualAnexoEntity()
		{
			
		}

		[Key]
		[Column("AVA_ID")]
		public int IdAnaliseVisualAnexo { get; set; }

		[Column("PLI_ID")]
		[ForeignKey(nameof(Pli))]
		public long? IdPli { get; set; }

		[StringLength(100)]
		[Column("AVA_DS_ARQUIVO")]
		public string NomeArquivo { get; set; }

		
		[Column("AVA_ME_ANEXO")]
		public byte[] Arquivo { get; set; }
	}
}