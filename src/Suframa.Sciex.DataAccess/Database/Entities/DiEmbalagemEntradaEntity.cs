using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_DI_EMBALAGEM_ENTRADA")]
	public partial class DiEmbalagemEntradaEntity : BaseEntity
	{		
		[Key]
		[Column("DEE_ID")]
		public long Id { get; set; }
		
		[Column("DEE_CO_TIPO_EMBALAGEM")]
		public string CodigoTipoEmbalagem { get; set; }

		[Column("DEE_QT_VOLUME_CARGA")]
		public string QuantidadeVolumeCarga { get; set; }

		[Column("DEE_NU_DI")]
		public string NumeroDi { get; set; }

		public virtual DiEntradaEntity DiEntrada { get; set; }

		[Column("DIE_ID")]
		[ForeignKey(nameof(DiEntrada))]
		public long IdDiEntrada { get; set; }
	}
}