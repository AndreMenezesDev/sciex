using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_CODIGO_LANCAMENTO")]
	public partial class CodigoLancamentoEntity : BaseEntity
	{

		public virtual ICollection<LancamentoEntity> Lancamento { get; set; }


		public CodigoLancamentoEntity()
		{
			Lancamento = new HashSet<LancamentoEntity>();
		}


		[Key] // RN - Este ID não será dado por auto incremento, será definido por carga de dados no banco
		[Column("CLA_ID")]
		public short IdCodigoLancamento { get; set; }
		
		[Required]
		[Column("CLA_DS")]
		[StringLength(255)]
		public string Descricao { get; set; }

		[Required]
		[Column("CLA_ST")]		
		public byte Status { get; set; }

		[Required]
		[Column("CLA_TP_OPERACAO")]
		public byte TipoOperacao { get; set; }

		[Required]
		[Column("CLA_TP_LANCAMENTO")]
		public byte TipoLancamento { get; set; }

	}
}