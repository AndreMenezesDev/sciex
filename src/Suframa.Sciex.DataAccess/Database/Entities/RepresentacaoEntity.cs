using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_REPRESENTACAO")]
	public partial class RepresentacaoEntity : BaseEntity
	{

		[Key]
		[Column("REP_ID")]
		public int IdRepresentacao { get; set; }

		[Required]
		[StringLength(100)]
		[Column("REP_DS_RAZAO_SOCIAL")]
		public string RazaoSocial { get; set; }

		[Required]
		[StringLength(11)]
		[Column("REP_NU_CPF")]
		public string CPF { get; set; }

		[Required]
		[StringLength(14)]
		[Column("REP_NU_CNPJ")]
		public string CNPJ { get; set; }

	}
}