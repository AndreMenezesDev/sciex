using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic.Tests
{
	[TestClass]
	public class ProtocoloTest
	{
		private readonly IProtocoloBll _protocoloBll;

		public ProtocoloTest()
		{
			CrossCutting.Mapping.Initialize.Init();
			CrossCutting.DependenceInjetion.Initialize.InitSingleton();

			_protocoloBll = CrossCutting.DependenceInjetion.Initialize.Instance<ProtocoloBll>(typeof(ProtocoloBll));
		}

		[TestMethod]
		public void Listar()
		{
			var resultado = _protocoloBll.Listar(new ProtocoloVM
			{
				NumeroProtocolo = "000001/2017",
				CpfCnpj = "121.448.777-77"
			});
		}

		[TestMethod]
		public void ListarConsultaProtocolo()
		{
			var lista = _protocoloBll.Listar(new ProtocoloVM { StatusProtocoloGrupo = EnumStatusProtocoloGrupo.Analise });
		}

		[TestMethod]
		public void SalvarProtocolo()
		{
			_protocoloBll.GerarProtocolo(new ProtocoloEntity { IdRequerimento = 11, TipoOrigem = 1 }, new RequerimentoEntity());
		}

		[TestMethod]
		[ExpectedException(typeof(FluentValidation.ValidationException))]
		public void SalvarProtocoloComExceptionTipoRequerimento()
		{
			_protocoloBll.GerarProtocolo(new ProtocoloEntity { IdRequerimento = 87, TipoOrigem = 1 }, new RequerimentoEntity());
		}
	}
}