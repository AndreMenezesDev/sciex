using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SEQUENCIAL_DI")]	
	public class SequencialEntity : BaseEntity
	{
		[Key]
		[Column("SEQDI_ID")]
		public int Id { get; set; }

		[Column("SEQDI_SEQUENCIAL")]
		public int Sequencial { get; set; }	

	}
}