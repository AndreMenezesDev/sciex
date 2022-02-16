using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("SCIEX_PARECER_COMPLEMENTAR")]
    public partial class ParecerComplementarEntity : BaseEntity
    {
		[Key]
		[Column("pac_id")]
		public int IdParecerComplementar { get; set; }
	
		[StringLength(14)]
		[Column("pac_ds_resolucao")]
		public string DescricaoResolucao { get; set; }
		
		[StringLength(100)]
		[Column("pac_ds_conclusao")]
		public string DescricaoConclusao { get; set; }
	}
}