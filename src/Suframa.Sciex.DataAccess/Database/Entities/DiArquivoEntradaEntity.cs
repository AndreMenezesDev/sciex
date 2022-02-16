using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_DI_ARQUIVO_ENTRADA")]
	public partial class DiArquivoEntradaEntity : BaseEntity
	{
		public virtual DiArquivoEntity DiArquivo { get; set; }

		[Key]
		[Column("DAR_ID")]
		public long Id { get; set; }

		[Column("DAR_DH_RECEPCAO_ARQUIVO")]
		public DateTime DataHoraRecepcao { get; set; }

		[Column("DAR_ST_LEITURA")]
		public Byte SituacaoLeitura { get; set; }

		[Column("DAR_QT_DI")]
		public Int16 QuantidadeDi { get; set; }

		[Column("DAR_QT_DI_PROCESSADA")]
		public Int16 QuantidadeDiProcessada { get; set; }

		[Column("DAR_QT_DI_ERRO")]
		public Int16 QuantidadeDiErro { get; set; }

		[Column("DAR_DH_INICIO_PROCESSO")]
		public DateTime? DataHoraInicioProcesso { get; set; }

		[Column("DAR_DH_FIM_PROCESSO")]
		public DateTime? DataHoraFimProcesso { get; set; }
	}
}