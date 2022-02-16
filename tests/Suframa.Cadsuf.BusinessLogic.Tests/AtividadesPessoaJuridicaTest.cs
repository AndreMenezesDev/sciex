using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class AtividadesPessoaJuridicaTest
    {
        private readonly IAtividadesPessoaJuridicaBll _atividadesPessoaJuridicaBll;

        public AtividadesPessoaJuridicaTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _atividadesPessoaJuridicaBll = CrossCutting.DependenceInjetion.Initialize.Instance<AtividadesPessoaJuridicaBll>(typeof(AtividadesPessoaJuridicaBll));
        }

        [TestMethod]
        public void ListarTest()
        {
            var result = _atividadesPessoaJuridicaBll.Listar(new AtividadePessoaJuridicaVM());
            //var lista = _naturezaJuridicaBll.ListarNaturezaJuridica(new NaturezaJuridicaDto
            //{
            //    IdNaturezaGrupo = 1,
            //    Codigo = 9999
            //});
        }

        [TestMethod]
        public void ListarTodosTest()
        {
            var result = _atividadesPessoaJuridicaBll.Listar();
        }

        [TestMethod]
        public void SalvarAlteracaoAtividadesPessoaJuridicaTest()
        {
            var dto = new AtividadesPessoaJuridicaVM
            {
                IdPessoaJuridica = 35,
                IdSubClasseAtividade = 2,

                AtividadesPessoaJuridica = new List<AtividadePessoaJuridicaVM>
                  {
                     new AtividadePessoaJuridicaVM{  IdAtividade= 35,IdPessoaJuridica= 35,StatusAtuante= true,IdSubClasseAtividade= 35,Tipo= EnumTipoAtividade.Principal}
                 },
                AtividadeSecundariaPessoaJuridica = new List<AtividadePessoaJuridicaVM>
                 {
                      new AtividadePessoaJuridicaVM{  IdAtividade= 35,IdPessoaJuridica= 35,StatusAtuante= true,IdSubClasseAtividade= 35,Tipo= EnumTipoAtividade.Secundaria},
                      new AtividadePessoaJuridicaVM{  IdAtividade= 35,IdPessoaJuridica= 35,StatusAtuante= true,IdSubClasseAtividade= 35,Tipo= EnumTipoAtividade.Secundaria,},
                 }
            };

            _atividadesPessoaJuridicaBll.Salvar(dto);
        }

        [TestMethod]
        public void SalvarAtividadesPessoaJuridicaTest()
        {
            var dto = new AtividadesPessoaJuridicaVM
            {
                IdPessoaJuridica = 35,
                IdSubClasseAtividade = 2,

                AtividadesPessoaJuridica = new List<AtividadePessoaJuridicaVM>
                  {
                     new AtividadePessoaJuridicaVM{IdPessoaJuridica= 35,StatusAtuante= true,IdSubClasseAtividade= 16,Tipo= EnumTipoAtividade.Principal}
                 },
                AtividadeSecundariaPessoaJuridica = new List<AtividadePessoaJuridicaVM>
                 {
                      new AtividadePessoaJuridicaVM{IdPessoaJuridica= 35,StatusAtuante= true,IdSubClasseAtividade= 17,Tipo= EnumTipoAtividade.Secundaria},
                      new AtividadePessoaJuridicaVM{IdPessoaJuridica= 35,StatusAtuante= true,IdSubClasseAtividade= 18,Tipo= EnumTipoAtividade.Secundaria,}
                 }
            };

            _atividadesPessoaJuridicaBll.Salvar(dto);
        }

        [TestMethod]
        public void SelecionarTest()
        {
            var result = _atividadesPessoaJuridicaBll.Selecionar(new AtividadesPessoaJuridicaVM { IdPessoaJuridica = 35 });
        }
    }
}