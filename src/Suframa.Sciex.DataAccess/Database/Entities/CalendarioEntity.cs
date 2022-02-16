using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_CALENDARIO")]
	public partial class CalendarioEntity : BaseEntity
	{
		[Column("CAG_NU_ANO")]
		public int Ano { get; set; }

		[Column("CAD_DT_ATENDIMENTO")]
		public DateTime? DataAtendimento { get; set; }

		[Column("CAH_HR_ATENDIMENTO")]
		public DateTime? HoraAtendimento { get; set; }

		[Column("AGD_ID")]
		public int? IdAgendaAtendimento { get; set; }

		[Key]
		[Column("CAL_ID")]
		public long IdCalendario { get; set; }

		[Column("CAG_ID")]
		public int? IdCalendarioAgendamento { get; set; }

		[Column("CAD_ID")]
		public int? IdCalendarioDia { get; set; }

		[Column("UND_ID")]
		public int? IdUnidadeCadastradora { get; set; }

		[Column("USI_ID")]
		public int? IdUsuarioInterno { get; set; }

		[Column("CAG_NU_MES")]
		public int Mes { get; set; }

		[Column("USI_NO")]
		public string NomeUsuarioInterno { get; set; }

		[Column("CAG_ST")]
		public int? StatusAgendamento { get; set; }

		[Column("CAH_ST_HORARIO_ATENDIMENTO")]
		public int? StatusHorarioAtendimento { get; set; }

		[Column("UND_DS")]
		public string UnidadeCadastradora { get; set; }
	}
}