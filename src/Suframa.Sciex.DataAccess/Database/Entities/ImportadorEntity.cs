using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_IMPORTADOR")]
	public partial class ImportadorEntity : BaseEntity
	{
	


		public ImportadorEntity()
		{
		}

		[Key]
		[Column("IMP_ID")]
		public int IdImportador { get; set; }

		[Required]
		[Column("IMP_NU_INSCRICAO_CADASTRAL")]
		public decimal InscricaoCadastral { get; set; }

		[Required]
		[StringLength(14)]
		[Column("IMP_NU_CNPJ")]
		public string CNPJ { get; set; }

		[Required]
		[StringLength(120)]
		[Column("IMP_DS_RAZAO_SOCIAL")]
		public string RazaoSocial { get; set; }

		[StringLength(11)]
		[Column("IMP_NU_CPF_REP_LEGAL")]
		public string CPFRepresentanteLegal { get; set; }

		[StringLength(4)]
		[Column("IMP_NU_CNAE")]
		public string CNAE { get; set; }

		[Required]
		[StringLength(3)]
		[Column("PAI_CO")]
		public string CodigoPais { get; set; }
	}
}