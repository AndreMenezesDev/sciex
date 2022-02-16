using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_LE_INSUMO_ERRO")]
	public partial class LEInsumoErroEntity : BaseEntity
	{
		public virtual LEInsumoEntity LEInsumo { get; set; }

		public LEInsumoErroEntity()
		{
			
		}

		[Key]
		[Column("LIE_ID")]
		public int IdLeInsumoErro { get; set; }

		[Column("LEI_ID")]
		[ForeignKey(nameof(LEInsumo))]
		public int IdLeInsumo { get; set; }

		[Required]
		[Column("LIE_DT")]
		public DateTime DataErroRegistro { get; set; }

		[Required]
		[Column("LIE_DS_ERRO")]
		[StringLength(500)]
		public string DescricaoErro { get; set; }

		[Column("LIE_NU_CPF_RESPONSAVEL")]
		[StringLength(11)]
		public string CpfResponsavel { get; set; }

		[Column("LIE_NO_RESPONSAVEL")]
		[StringLength(80)]
		public string NomeResponsavel { get; set; }

	}
}