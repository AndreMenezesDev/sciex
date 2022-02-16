using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using Suframa.Sciex.BusinessLogic.Pss;
using System.Text;
using System.Web.UI;
using Suframa.Sciex.CrossCutting.SuperStructs;

namespace Suframa.Sciex.BusinessLogic
{
	public class ParametrizarAnalistaBll : IParametrizarAnalistaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IDistribuicaoAutomaticaBll _distribuicaoAutomaticaBll;
		private readonly IUsuarioPssBll _usuarioPssBll;


		public ParametrizarAnalistaBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPssBll, IDistribuicaoAutomaticaBll distribuicaoAutomaticaBll)
		{
			_uowSciex = uowSciex;
			_usuarioPssBll = usuarioPssBll;
			_distribuicaoAutomaticaBll = distribuicaoAutomaticaBll;
		}

		public IEnumerable<object> ListarAnalistaPliDropDown()
		{
			List<Object> lista = new List<Object>();
			lista.Add(new { îd = 0, text = "Todos" });

			lista.AddRange(_uowSciex.QueryStackSciex.Analista
						.Listar().Where(o => o.SituacaoVisual == 1)
						.OrderBy(o => o.Nome)
						.Select(
							s => new
							{
								id = s.IdAnalista,
								text = s.Nome
							}));
			return lista;
		}
		public IEnumerable<object> ListarAnalistaLeDropDown()
		{
			List<Object> lista = new List<Object>();
			lista.Add(new { îd = 0, text = "Todos" });

			lista.AddRange(_uowSciex.QueryStackSciex.Analista
						.Listar().Where(o => o.SituacaoLE == 1)
						.OrderBy(o => o.Nome)
						.Select(
							s => new
							{
								id = s.IdAnalista,
								text = s.Nome
							}));
			return lista;
		}

		public IEnumerable<object> ListarAnalistaPlanoDropDown()
		{
			List<Object> lista = new List<Object>();
			lista.Add(new { îd = 0, text = "Todos" });

			lista.AddRange(_uowSciex.QueryStackSciex.Analista
						.Listar().Where(o => o.SituacaoPlano == 1)
						.OrderBy(o => o.Nome)
						.Select(
							s => new
							{
								id = s.IdAnalista,
								text = s.Nome
							}));
			return lista;
		}

		public IEnumerable<AnalistaVM> Listar(AnalistaVM pliVM)
		{
			var pli = _uowSciex.QueryStackSciex.Analista.Listar<AnalistaVM>();
			return pli;//AutoMapper.Mapper.Map<IEnumerable<PliVM>>(pli);
		}

		public IEnumerable<object> Listar()
		{
			return _uowSciex.QueryStackSciex.Pli
				.Listar()
				.OrderBy(o => o.NumeroPli)
				.Select(
					s => new
					{
						id = s.IdPLI,
						text = s.CodigoCNAE + " - " + s.NumeroPli
					});
		}

		public AnalistaVM Salvar(AnalistaVM analistaVM)
		{
			if (analistaVM == null)
				return null;

			var usuarioLogado = _usuarioPssBll.ObterUsuarioLogado();
			var cpfUnformat = analistaVM.CPF.CnpjCpfUnformat();
			var analista = _uowSciex.QueryStackSciex.Analista.Selecionar(o => o.IdAnalista == analistaVM.IdAnalista);
			
			if(analistaVM.Path == "PLI")
			{
				if (analistaVM.SituacaoVisual == 0)
				{
					analista.SituacaoVisual = analistaVM.SituacaoVisual;
					if (analista.SituacaoVisualSetada == 1)
					{
						analista.SituacaoVisualSetada = 0;
						_uowSciex.CommandStackSciex.Analista.Salvar(analista);
						_uowSciex.CommandStackSciex.Save();

						var analistasAtivo = _uowSciex.QueryStackSciex.Analista.Listar(o => o.SituacaoVisual == 1);

						if (analistasAtivo.Count > 0)
						{
							int index = analistasAtivo.FindIndex(nd => nd.IdAnalista == analista.IdAnalista);
							var prox = analistasAtivo.ElementAtOrDefault((analistasAtivo.Count - 1) == index ? 0 : index + 1);
							prox.SituacaoVisualSetada = 1;

							_uowSciex.CommandStackSciex.Analista.Salvar(prox);
							_uowSciex.CommandStackSciex.Save();
						}
					}
					else
					{
						_uowSciex.CommandStackSciex.Analista.Salvar(analista);
						_uowSciex.CommandStackSciex.Save();
					}

					var listaPliAnaliseVisual = _uowSciex.QueryStackSciex.PliAnaliseVisual.Listar(o => o.CpfAnalista.Equals(cpfUnformat) && (o.StatusAnalise == 2 || o.StatusAnalise == 9));
					if (listaPliAnaliseVisual != null && listaPliAnaliseVisual.Count > 0)
					{
						foreach (var item in listaPliAnaliseVisual)
						{
							item.CpfAnalista = null;
							item.NomeAnalista = null;

							_uowSciex.CommandStackSciex.PliAnaliseVisual.Salvar(item);
							_uowSciex.CommandStackSciex.Save();
						}
					}

					_distribuicaoAutomaticaBll.DistribuirPlisAutomaticamente();

				}
				else if (analistaVM.SituacaoVisual == 1)
				{
					analista.SituacaoVisual = analistaVM.SituacaoVisual;
					analista.SituacaoVisualSetada = 0;

					_uowSciex.CommandStackSciex.Analista.Salvar(analista);
					_uowSciex.CommandStackSciex.Save();
				}

				AuditoriaEntity auditoria = new AuditoriaEntity();
				auditoria.IdAuditoriaAplicacao = 4;
				auditoria.CpfCnpjResponsavel = usuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();
				auditoria.NomeResponsavel = usuarioLogado.usuarioLogadoNome;
				auditoria.TipoAcao = 2;
				auditoria.DataHoraAcao = DateTime.Now;
				auditoria.IdReferencia = analista.IdAnalista;
				auditoria.DescricaoAcao = analistaVM.SituacaoVisual == 0 ? "ALTERAÇÃO: Alterando registro: " + analista.IdAnalista + " – Campos afetados: SituacaoVisual DE: 1 PARA: 0 " : "ALTERAÇÃO: Alterando registro: " + analista.IdAnalista + " – Campos afetados:  SituacaoVisual DE: 0 PARA: 1 ";

				_uowSciex.CommandStackSciex.Auditoria.Salvar(auditoria);
				_uowSciex.CommandStackSciex.Save();
			}
			

			if(analistaVM.Path == "LE")
			{
				if (analistaVM.SituacaoLE == 0)
				{
					analistaVM.SituacaoPlano = analistaVM.SituacaoLE;
					analista.SituacaoPlano = analistaVM.SituacaoPlano;
					analista.SituacaoLE = analistaVM.SituacaoLE;
					analista.Solicitacao = analistaVM.SituacaoLE;
					
					if (analista.SituacaoLESetado == 1)
					{
						analista.SituacaoLESetado = 0;
						analista.SituacaoPlanoSetado = 0;
						analista.SituacaoSolicitacaoSetado = 0;

						_uowSciex.CommandStackSciex.Analista.Salvar(analista);
						_uowSciex.CommandStackSciex.Save();

						var analistasAtivo = _uowSciex.QueryStackSciex.Analista.Listar(o => o.SituacaoLE == 1);

						if (analistasAtivo.Count > 0)
						{
							int index = analistasAtivo.FindIndex(nd => nd.IdAnalista == analista.IdAnalista);
							var prox = analistasAtivo.ElementAtOrDefault((analistasAtivo.Count - 1) == index ? 0 : index + 1);
							prox.SituacaoLESetado = 1;
							prox.SituacaoPlanoSetado = 1;
							prox.SituacaoSolicitacaoSetado = 1;

							_uowSciex.CommandStackSciex.Analista.Salvar(prox);
							_uowSciex.CommandStackSciex.Save();
						}
					}
					else
					{
						_uowSciex.CommandStackSciex.Analista.Salvar(analista);
						_uowSciex.CommandStackSciex.Save();
					}

					var listaLEs = _uowSciex.QueryStackSciex.LEProduto.Listar(o => o.CpfResponsavel.Equals(cpfUnformat) && o.StatusLE == 3);
					if (listaLEs != null && listaLEs.Count > 0)
					{
						foreach (var item in listaLEs)
						{
							item.CpfResponsavel = null;
							item.NomeResponsavel = null;

							_uowSciex.CommandStackSciex.LEProduto.Salvar(item);
							_uowSciex.CommandStackSciex.Save();
						}
					}

					var listaPlanos = _uowSciex.QueryStackSciex.PlanoExportacao.Listar(o => o.CpfResponsavel.Equals(cpfUnformat) && o.Situacao == 3);
					if (listaPlanos != null && listaPlanos.Count > 0)
					{
						foreach (var item in listaPlanos)
						{
							item.CpfResponsavel = null;
							item.NomeResponsavel = null;

							_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(item);
							_uowSciex.CommandStackSciex.Save();
						}
					}
					var listaSolicitacoes = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Listar(o => o.CpfResponsavel.Equals(cpfUnformat) && o.Status == 3);
					if (listaSolicitacoes != null && listaSolicitacoes.Count > 0)
					{
						foreach (var item in listaSolicitacoes)
						{
							item.CpfResponsavel = null;
							item.NomeResponsavel = null;

							_uowSciex.CommandStackSciex.PRCSolicitacaoAlteracao.Salvar(item);
							_uowSciex.CommandStackSciex.Save();
						}
					}
					_distribuicaoAutomaticaBll.DistribuirPlisAutomaticamente();

				}
				else if (analistaVM.SituacaoLE == 1)
				{
					analistaVM.SituacaoPlano = analistaVM.SituacaoLE;
					analista.SituacaoLE = analistaVM.SituacaoLE;
					analista.SituacaoPlano = analistaVM.SituacaoPlano;
					analista.Solicitacao = analistaVM.SituacaoLE;
					
					analista.SituacaoLESetado = 0;
					analista.SituacaoPlanoSetado = 0;
					analista.SituacaoSolicitacaoSetado = 0;

					_uowSciex.CommandStackSciex.Analista.Salvar(analista);
					_uowSciex.CommandStackSciex.Save();
				}

				AuditoriaEntity auditoria = new AuditoriaEntity();
				auditoria.IdAuditoriaAplicacao = 4;
				auditoria.CpfCnpjResponsavel = usuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();
				auditoria.NomeResponsavel = usuarioLogado.usuarioLogadoNome;
				auditoria.TipoAcao = 2;
				auditoria.DataHoraAcao = DateTime.Now;
				auditoria.IdReferencia = analista.IdAnalista;
				auditoria.DescricaoAcao = analistaVM.SituacaoVisual == 0 ? "ALTERAÇÃO: Alterando registro: " + analista.IdAnalista + " – Campos afetados: SituacaoLE DE: 1 PARA: 0 " : "ALTERAÇÃO: Alterando registro: " + analista.IdAnalista + " – Campos afetados:  SituacaoLE DE: 0 PARA: 1 ";

				_uowSciex.CommandStackSciex.Auditoria.Salvar(auditoria);
				_uowSciex.CommandStackSciex.Save();
			}

			return new AnalistaVM() { Mensagem = "Salvo com Sucesso!"};
		}

		public PagedItems<AnalistaVM> ListarPaginado(AnalistaVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<AnalistaVM>(); }

			var analistasPss = _usuarioPssBll.ObterListaUsuariosPerfilAnalista();

			foreach (var item in analistasPss)
			{
				var ret = _uowSciex.QueryStackSciex.Analista.Selecionar(o => o.CPF.Equals(item.usuarioLogadoCpfCnpj));
				if(ret == null)
				{
					AnalistaEntity analista = new AnalistaEntity();
					analista.CPF = item.usuarioLogadoCpfCnpj;
					analista.DataHoraSincronizacao = DateTime.Now;
					analista.DescricaoSetor = "-";
					analista.Nome = item.usuNomeUsuario;
					analista.SiglaSetor = "-";
					analista.SituacaoVisual = 0;
					analista.SituacaoVisualSetada = 0;
					analista.SituacaoLE = 0;
					analista.SituacaoLESetado = 0;
					analista.Solicitacao = 0;
					analista.SituacaoSolicitacaoSetado = 0;

					_uowSciex.CommandStackSciex.Analista.Salvar(analista);
					_uowSciex.CommandStackSciex.Save();
				}
			}


			var analistas = _uowSciex.QueryStackSciex.Analista.ListarPaginado<AnalistaVM>(o => o.IdAnalista != null, pagedFilter);

			foreach (var item in analistas.Items)
			{
				var cpfCnpj = item.CPF.CnpjCpfUnformat();
				var plis = _uowSciex.QueryStackSciex.PliAnaliseVisual.Listar(o => o.CpfAnalista.Equals(cpfCnpj) && (o.StatusAnalise == 2 || o.StatusAnalise == 9));
				item.QuantidadePLI = plis.Count;
				var les = _uowSciex.QueryStackSciex.LEProduto.Listar(o => (o.CpfResponsavel.Equals(cpfCnpj) && o.StatusLE == 3) 
																			|| 
																			(o.CpfResponsavel.Equals(cpfCnpj) && o.StatusLE == 4 && o.StatusLEAlteracao == 3));
				item.QuantidadeLE = les.Count;
				var pes = _uowSciex.QueryStackSciex.PlanoExportacao.Listar(o => o.CpfResponsavel.Equals(cpfCnpj) && o.Situacao == 3);
				item.QuantidadePE = pes.Count;
				var slcDeAlteracaoPRC = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Listar(o => o.CpfResponsavel.Equals(cpfCnpj) && o.Status == 3);
				item.SolicAlteracaoProcesso = slcDeAlteracaoPRC.Count;

			}

			return analistas;

		}

	}
}