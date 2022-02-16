using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;

namespace Suframa.Sciex.BusinessLogic.Tests
{
	[TestClass]
	public class PedidoCorrecaoTest
	{
		private IUnitOfWork _uow;

		public PedidoCorrecaoTest()
		{
			CrossCutting.Mapping.Initialize.Init();
			CrossCutting.DependenceInjetion.Initialize.InitSingleton();

			_uow = CrossCutting.DependenceInjetion.Initialize.Instance<UnitOfWork>(typeof(UnitOfWork));
		}

		[TestMethod]
		public void ListarDicionario()
		{
			var resultado = _uow.QueryStack.DicionarioDropDown.Listar();
		}
	}
}