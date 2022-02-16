using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ERRO_MENSAGEM")]
	public partial class ErroMensagemEntity : BaseEntity
	{

		public virtual ICollection<ErroProcessamentoEntity> ErroProcessamento { get; set; }

		public ErroMensagemEntity()
		{
			ErroProcessamento = new HashSet<ErroProcessamentoEntity>();
		}

		[Key] // RN - Este ID não será dado por auto incremento, será definido por carga de dados no banco
		[Column("EME_ID")]
		public short IdErroMensagem { get; set; }
		
		[StringLength(500)]
		[Column("EME_DS_ERRO")]
		public string Descricao { get; set; }
		
		[Column("EME_CO_FASE")]
		public byte? CodigoFase { get; set; }

		[Column("EME_CO_SISTEMA_ORIGEM")]
		public byte? CodigoSistemaOrigem { get; set; }

	}
}