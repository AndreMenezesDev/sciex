using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_TAXA_GRUPO_BENEFICIO")]
	public partial class TaxaGrupoBeneficioEntity : BaseEntity
	{

		public virtual ICollection<TaxaNCMBeneficioEntity> TaxaNCMBeneficioEntity { get; set; }
		public TaxaGrupoBeneficioEntity()
		{
			TaxaNCMBeneficioEntity = new HashSet<TaxaNCMBeneficioEntity>();
		}

		[Key]
		[Column("TGB_ID")]
		public int IdTaxaGrupoBeneficio { get; set; }

		[Required]
		[MaxLength(100)]
		[Column("TGB_DS")]		
		public string Descricao { get; set; }

		[Column("TGB_DH_CADASTRO")]
		public DateTime? DataCadastro { get; set; }

		[Required]
		[Column("TGB_VL_PERC_REDUCAO")]
		public decimal ValorPercentualReducao { get; set; }

		[Required]
		[Column("TGB_CO")]
		public short Codigo { get; set; }

		[MaxLength(3000)]
		[Column("TGB_DS_AMPARO_LEGAL")]
		public string DescricaoAmparoLegal { get; set; }

		[Required]
		[Column("TGB_TP_BENEFICIO")]
		public short TipoBeneficio { get; set; }

		[Column("TGB_ST")]
		public byte? StatusBeneficio { get; set; }
	}
}