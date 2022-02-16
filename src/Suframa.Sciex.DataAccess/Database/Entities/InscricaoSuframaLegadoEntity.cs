using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CSUF_OLD_INSCSUF")]
	public partial class InscricaoSuframaLegadoEntity : BaseEntity
	{
		[Column("OIN_DT_DTVAL")]
		public DateTime? DataValidade { get; set; }

		[Key]
		[Column("OIN_ID")]
		public int IdInscricaoSuframaLegado { get; set; }

		[Column("OIN_CO_EMP_CNPJ")]
		public string NumeroCnpj { get; set; }

		[Column("OIN_NU_INSCSUF")]
		public decimal? NumeroInscricaoSuframa { get; set; }

		[Column("OIN_CO_SET_CD")]
		public int? OinCoSetCd { get; set; }

		[Column("OIN_CO_SIT")]
		public int? Status { get; set; }
	}
}