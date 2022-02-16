using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic.Tests
{
	[TestClass]
	public class NaturezaJuridicaTest
	{
		private readonly INaturezaJuridicaBll _naturezaJuridicaBll;

		public NaturezaJuridicaTest()
		{
			CrossCutting.Mapping.Initialize.Init();
			CrossCutting.DependenceInjetion.Initialize.InitSingleton();

			_naturezaJuridicaBll = CrossCutting.DependenceInjetion.Initialize.Instance<NaturezaJuridicaBll>(typeof(NaturezaJuridicaBll));
		}

		[TestMethod]
		public void ApagarNaturezaJuridicTest()
		{
			_naturezaJuridicaBll.Apagar(new NaturezaJuridicaDto
			{
				IdNaturezaGrupo = 14214234,
				Codigo = 9999,
				Descricao = "teste descrção",
				DataInclusao = DateTime.Now,
			});
		}

		[TestMethod]
		public void SalvarAlteracaoNaturezaJuridicaTest()
		{
			var dto = new ManterNaturezaJuridicaVM
			{
				IdNaturezaJuridica = 22,
				IdNaturezaGrupo = 1,
				Codigo = 8888,
				Descricao = "teste descrição 3 ALTERADO 11"
				,
				Qualificacoes = new List<QualificacaoVM>
				{
					new QualificacaoVM{IdQualificacao = 1, Descricao="Representante teste 1"},
                   // new QualificacaoDto{IdQualificacao = 4,Codigo = 4, Descricao="8Representante
                   // teste 4"},
                    new QualificacaoVM{IdQualificacao = 3, Descricao="7Representante teste 3"},
				}
			};

			_naturezaJuridicaBll.Salvar(dto);
			_naturezaJuridicaBll.Apagar(new NaturezaJuridicaDto { IdNaturezaJuridica = dto.IdNaturezaJuridica });
		}

		[TestMethod]
		public void SalvarNaturezaJuridicaTest()
		{
			var dto = new ManterNaturezaJuridicaVM
			{
				IdNaturezaGrupo = 1,
				Codigo = 6452688,
				Descricao = "teste descrição 33"
			   ,
				Qualificacoes = new List<QualificacaoVM>
				{
					new QualificacaoVM{IdQualificacao = 1, Descricao="Representante teste 1"},
					new QualificacaoVM{IdQualificacao = 4, Descricao="Representante teste 4"},
					new QualificacaoVM{IdQualificacao = 3, Descricao="Representante teste 3"},
				}
			};

			_naturezaJuridicaBll.Salvar(dto);
			_naturezaJuridicaBll.Apagar(new NaturezaJuridicaDto { IdNaturezaJuridica = dto.IdNaturezaJuridica });
		}
	}
}