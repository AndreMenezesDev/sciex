using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.DataAccess.Tests
{
	[TestClass]
	public class RequerimentoTest
	{
		private IUnitOfWork _uow;

		public RequerimentoTest()
		{
			CrossCutting.Mapping.Initialize.Init();
			CrossCutting.DependenceInjetion.Initialize.InitSingleton();

			_uow = CrossCutting.DependenceInjetion.Initialize.Instance<UnitOfWork>(typeof(UnitOfWork));
		}

		[TestMethod]
		public void InserirRequerimento()
		{
			var requerimentoEntity = new RequerimentoEntity
			{
				IdDadosSolicitante = 110,
				IdTipoRequerimento = 8
			};

			_uow.CommandStack.Requerimento.Salvar(requerimentoEntity);
			_uow.CommandStack.Save();
		}

		[TestMethod]
		public void ListarPaginadoDinamicoTest()
		{
			Suframa.Sciex.CrossCutting.DataTransferObject.PagedOptions options = new CrossCutting.DataTransferObject.PagedOptions
			{
				Size = 5
			};

			var ret = _uow.QueryStack.NaturezaJuridica
				.ListarPaginado(o => o.IdNaturezaGrupo > 5, options);

			Assert.IsTrue(ret.Items.Count == 5);
		}

		[TestMethod]
		public void ListarPaginadoTipadoTest()
		{
			var ret = _uow.QueryStack.NaturezaJuridica
				.ListarPaginado(o => o.IdNaturezaGrupo > 5, new PagedOptions { Page = 2, Size = 5, Sort = "NJU_ID", Reverse = true });

			Assert.IsTrue(ret.Items.Count == 5);
		}

		//[TestMethod]
		//public void SelecionarPorChavePrimaria()
		//{
		//    var ret = _uow.QueryStack.ObjetivoSocial.Selecionar(1);
		//    Assert.IsTrue(ret.IdObjetivoSocial == 20);
		//}

		//[TestMethod]
		//public void AdicionarRegistros()
		//{
		//    var a = new ObjetivoSocial()
		//    {
		//        Descricao = "teste",
		//        PessoaJuridica = new PessoaJuridica()
		//        {
		//            Cnpj = "asdsdefdas",
		//            RazaoSocial = "sdadsf"
		//        }
		//    };
		//    _uow.CommandStack.ObjetivoSocial.Adicionar(a);
		//    _uow.CommandStack.Discart();
		//    _uow.CommandStack.Save();

		// var b = new ObjetivoSocial() { Descricao = "teste", PessoaJuridica = new PessoaJuridica()
		// { Cnpj = "1234", RazaoSocial = "dfgdfg" } };

		//    _uow.CommandStack.ObjetivoSocial.Adicionar(b);
		//    _uow.CommandStack.Save();
		//}
	}
}