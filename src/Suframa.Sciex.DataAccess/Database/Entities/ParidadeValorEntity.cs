using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("SCIEX_PARIDADE_VALOR")]
    public class ParidadeValorEntity : BaseEntity
    {
		public virtual ParidadeCambialEntity ParidadeCambial { get; set; }
		public virtual MoedaEntity Moeda { get; set; }
		public virtual ICollection<TaxaPliMercadoriaEntity> TaxaPliMercadoria { get; set; }

		public ParidadeValorEntity()
		{
			TaxaPliMercadoria = new HashSet<TaxaPliMercadoriaEntity>();
		}

		[Key]
		[Column("PVA_ID")]
		public int IdParidadeValor { get; set; }

		[Required]
		[Column("PCA_ID")]
		[ForeignKey(nameof(ParidadeCambial))]
		public int IdParidadeCambial { get; set; }

		[Required]
		[Column("MOE_ID")]
		[ForeignKey(nameof(Moeda))]
		public int IdMoeda { get; set; }

		[Column("PVA_VL_PARIDADE")]
		public Decimal Valor { get; set; }
    }
}