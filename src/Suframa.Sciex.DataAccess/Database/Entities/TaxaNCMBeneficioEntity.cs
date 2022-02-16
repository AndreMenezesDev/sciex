using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_TAXA_NCM_BENEFICIO")]
	public partial class TaxaNCMBeneficioEntity : BaseEntity
	{
		public virtual ICollection<TaxaGrupoBeneficioEntity> TaxaGrupoBeneficio { get; set; }		

		public TaxaNCMBeneficioEntity()
		{
			TaxaGrupoBeneficio = new HashSet<TaxaGrupoBeneficioEntity>();
		}


		[Key]
		[Column("TNB_ID")]
		public int IdTaxaNCMBeneficio { get; set; }

		[Required]
		[StringLength(8)]
		[Column("TNB_CO_NCM")]		
		public string CodigoNCM { get; set; }

		[Required]		
		[Column("TNB_DH_CADASTRO")]
		public DateTime DataCadastro { get; set; }

		[Required]
		[Column("TGB_ID")]
		[ForeignKey(nameof(TaxaGrupoBeneficio))]
		public int IdTaxaGrupoBeneficio { get; set; }
	}
}