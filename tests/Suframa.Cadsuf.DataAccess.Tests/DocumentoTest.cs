using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.DataAccess.Tests
{
	[TestClass]
	public class DocumentoTest
	{
		private readonly IUnitOfWork _uow;

		public DocumentoTest()
		{
			CrossCutting.Mapping.Initialize.Init();
			CrossCutting.DependenceInjetion.Initialize.InitSingleton();
			_uow = CrossCutting.DependenceInjetion.Initialize.Instance<UnitOfWork>(typeof(UnitOfWork));
		}

		[TestMethod]
		public void ListarDocumentos()
		{
			var documentos = _uow.QueryStack.VWDocumento.Listar<DocumentoComprobatorioVM>();
		}
	}
}