using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_SAGAT_EMPRESA_ATUACAO")]
	public partial class TaxaEmpresaAtuacaoEntity : BaseEntity
	{

		[Key]
		[Column("TEA_ID")]
		public int IdTaxaEmpresaAtuacao { get; set; }

		[Required]
		[Column("TEA_NU_CNPJ")]
		[MaxLength(14)]
		public string CNPJ { get; set; }
		
		[Required]
		[Column("TEA_DH_CADASTRO")]
		public DateTime DataCadastro { get; set; }

		[Required]
		[Column("TEA_CO")]
		public short Codigo { get; set; }
	}
}