using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_TAXA_FATO_GERADOR")]
	public partial class TaxaFatoGeradorEntity : BaseEntity
	{
	
		[Key]
		[Column("TFG_ID")]
		public int IdTaxaFatoGerador { get; set; }
		
		[Column("TFG_DS")]
		[MaxLength(100)]
		public string Descricao { get; set; }

		[Required]		
		[Column("TFG_VL")]
		public decimal Valor { get; set; }

		[Required]
		[Column("TFG_VL_PERC_LIMITADOR")]
		public decimal ValorPercentualLimitador { get; set; }

		[Required]
		[Column("TFG_DH_CADASTRO")]
		public DateTime DataCadastro { get; set; }

		[Required]
		[Column("TFG_CO")]
		public short Codigo { get; set; }

	}
}