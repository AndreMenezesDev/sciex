using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_DI_EMBALAGEM")]
	public partial class DiEmbalagemEntity : BaseEntity
	{		
		[Key]
		[Column("DEM_ID")]
		public long Id { get; set; }
		
		[Column("DEM_CO_TIPO_EMBALAGEM")]
		public int CodigoTipoEmbalagem { get; set; }

		[Column("DEM_QT_VOLUME_CARGA")]
		public int QuantidadeVolumeCarga { get; set; }

		public virtual DiEntity Di { get; set; }

		[Column("DI_ID")]
		[ForeignKey(nameof(Di))]
		public long IdDi { get; set; }
	}
}