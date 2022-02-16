using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;

namespace Suframa.Sciex.BusinessLogic.Tests
{
	[TestClass]
	public class CalendarioAgendamentoTest
	{
		private readonly ICalendarioAgendamentoBll _calendarioAgendamentoBll;

		public CalendarioAgendamentoTest()
		{
			CrossCutting.Mapping.Initialize.Init();
			CrossCutting.DependenceInjetion.Initialize.InitSingleton();

			_calendarioAgendamentoBll = CrossCutting.DependenceInjetion.Initialize.Instance<CalendarioAgendamentoBll>(typeof(CalendarioAgendamentoBll));
		}

		[TestMethod]
		public void Listar()
		{
			var agendaAtendimento = _calendarioAgendamentoBll.Listar(new CalendarioAgendamentoVM
			{
				IdUnidadeCadastradora = 21,
				Mes = 12,
				Ano = 2017
			});
		}
	}
}