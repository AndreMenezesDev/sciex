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
using Suframa.Sciex.CrossCutting.Extension;
using System.Data.Entity.Core.Mapping;
using System.Web.UI.WebControls;

namespace Suframa.Sciex.BusinessLogic
{
	public class DistribuicaoAutomaticaBll : IDistribuicaoAutomaticaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public DistribuicaoAutomaticaBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}

		public PliVM DistribuirPlisAutomaticamente()
		{
			PliVM retornoPLI = new PliVM();
			PliVM retornoLE = new PliVM();
			PliVM retornoPlano = new PliVM();
			PliVM retornoSolicitacoes = new PliVM();


			var _controleExecucaoServico1 = new ControleExecucaoServicoEntity();
			_controleExecucaoServico1.DataHoraExecucaoInicio = GetDateTimeNowUtc();
			_controleExecucaoServico1.IdListaServico = 18;
			_controleExecucaoServico1.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
			_controleExecucaoServico1.NumeroCPFCNPJInteressado = "04407029000143";

			var listaPlis = _uowSciex.QueryStackSciex.Pli.Listar(o => (o.StatusPli == 23 && o.StatusAnaliseVisual == 1 && o.PliAnaliseVisual == null) || (o.StatusPli == 23 && o.StatusAnaliseVisual == 1 && o.PliAnaliseVisual != null && o.PliAnaliseVisual.CpfAnalista == null));

			#region PLis
			var analistas = _uowSciex.QueryStackSciex.Analista.Listar(o => o.SituacaoVisual == 1).OrderBy(p => p.IdAnalista).ToList();
			if (analistas.Count() == 0)
			{
				retornoPLI.Mensagem = "Não há analistas ativos para distribuir os plis";
			}
			else
			{
				if (analistas.Where(o => o.SituacaoVisualSetada == 1).Count() == 0)
				{
					var first = analistas.FirstOrDefault();
					first.SituacaoVisualSetada = 1;
					_uowSciex.CommandStackSciex.DetachEntries();
					_uowSciex.CommandStackSciex.Analista.Salvar(first);
					_uowSciex.CommandStackSciex.Save();
				}

				if (listaPlis.Count > 0)
				{
					foreach (var item in listaPlis)
					{
						if (item.PliAnaliseVisual == null)
						{
							var analistaVez = analistas.Where(o => o.SituacaoVisualSetada == 1).FirstOrDefault();
							PliAnaliseVisualEntity av = new PliAnaliseVisualEntity();
							av.IdPLI = item.IdPLI;
							av.StatusAnalise = 2;
							av.CpfAnalista = analistaVez.CPF;
							av.NomeAnalista = analistaVez.Nome;

							_uowSciex.CommandStackSciex.PliAnaliseVisual.Salvar(av, true);
							_uowSciex.CommandStackSciex.Save();

							analistaVez.SituacaoVisualSetada = 0;
							_uowSciex.CommandStackSciex.Analista.Salvar(analistaVez);
							_uowSciex.CommandStackSciex.Save();

							int index = analistas.FindIndex(nd => nd.IdAnalista == analistaVez.IdAnalista);
							var prox = analistas.ElementAtOrDefault((analistas.Count - 1) == index ? 0 : index + 1);

							prox.SituacaoVisualSetada = 1;
							_uowSciex.CommandStackSciex.Analista.Salvar(prox);
							_uowSciex.CommandStackSciex.Save();
						}
						else
						{
							if (item.PliAnaliseVisual.CpfAnalista == null)
							{
								var analistaVez = analistas.Where(o => o.SituacaoVisualSetada == 1).FirstOrDefault();
								var av = _uowSciex.QueryStackSciex.PliAnaliseVisual.Selecionar(o => o.IdPLI == item.PliAnaliseVisual.IdPLI);
								av.CpfAnalista = analistaVez.CPF;
								av.NomeAnalista = analistaVez.Nome;

								_uowSciex.CommandStackSciex.DetachEntries();
								_uowSciex.CommandStackSciex.PliAnaliseVisual.Salvar(av);
								_uowSciex.CommandStackSciex.Save();

								analistaVez.SituacaoVisualSetada = 0;
								_uowSciex.CommandStackSciex.Analista.Salvar(analistaVez);
								_uowSciex.CommandStackSciex.Save();

								int index = analistas.FindIndex(nd => nd.IdAnalista == analistaVez.IdAnalista);
								var prox = analistas.ElementAtOrDefault((analistas.Count - 1) == index ? 0 : index + 1);

								prox.SituacaoVisualSetada = 1;
								_uowSciex.CommandStackSciex.Analista.Salvar(prox);
								_uowSciex.CommandStackSciex.Save();
							}
						}
					}
				}
				else
				{
					retornoPLI.Mensagem = "Não há plis para serem distribuidos!";
				}
			}

			#endregion

			_controleExecucaoServico1.StatusExecucao = String.IsNullOrEmpty(retornoPLI.Mensagem) ? 1 : 2;
			_controleExecucaoServico1.MemoObjetoRetorno = String.IsNullOrEmpty(retornoPLI.Mensagem) ? "DISTRIBUIÇÃO OK" : retornoPLI.Mensagem;
			_controleExecucaoServico1.DataHoraExecucaoFim = GetDateTimeNowUtc();
			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServico1);
			_uowSciex.CommandStackSciex.Save();

		   //LE
		    var _controleExecucaoServico2 = new ControleExecucaoServicoEntity();
			_controleExecucaoServico2.DataHoraExecucaoInicio = GetDateTimeNowUtc();
			_controleExecucaoServico2.IdListaServico = 18;
			_controleExecucaoServico2.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
			_controleExecucaoServico2.NumeroCPFCNPJInteressado = "04407029000143";

			var listaLEs = _uowSciex.QueryStackSciex.LEProduto.Listar(o => (o.StatusLE == 2) || o.StatusLEAlteracao == 2 || (o.StatusLE > 1 && o.NomeResponsavel == null && o.CpfResponsavel == null));

			#region LEs
			var analistasLE = _uowSciex.QueryStackSciex.Analista.Listar(o => o.SituacaoLE == 1).OrderBy(p => p.IdAnalista).ToList();
			if (analistasLE.Count() == 0)
			{
				retornoLE.Mensagem = "Não há analistas ativos para distribuir as LEs";
			}
			else
			{
				if (analistasLE.Where(o => o.SituacaoLESetado == 1).Count() == 0)
				{
					var first = analistasLE.FirstOrDefault();
					first.SituacaoLESetado = 1;
					_uowSciex.CommandStackSciex.DetachEntries();
					_uowSciex.CommandStackSciex.Analista.Salvar(first);
					_uowSciex.CommandStackSciex.Save();
				}

				if (listaLEs.Count > 0)
				{
					foreach (var item in listaLEs)
					{
						if (item.StatusLE == 2)
						{
							var analistaVez = analistasLE.Where(o => o.SituacaoLESetado == 1).FirstOrDefault();
							var prod = _uowSciex.QueryStackSciex.LEProduto.Selecionar(o => o.IdLe == item.IdLe);
							prod.StatusLE = 3;
							prod.CpfResponsavel = analistaVez.CPF;
							prod.NomeResponsavel = analistaVez.Nome;

							_uowSciex.CommandStackSciex.DetachEntries();
							_uowSciex.CommandStackSciex.LEProduto.Salvar(prod);
							_uowSciex.CommandStackSciex.Save();

							LEProdutoHistoricoEntity histo = new LEProdutoHistoricoEntity();
							histo.DataOcorrencia = GetDateTimeNowUtc();
							histo.SituacaoLE = 3;
							histo.DescricaoObservacao = "DISTRIBUIÇÃO AUTOMÁTICA";
							histo.CpfCnpjResponsavel = null;
							histo.NomeResponsavel = null;
							histo.IdLe = prod.IdLe;
							_uowSciex.CommandStackSciex.DetachEntries();
							_uowSciex.CommandStackSciex.LEProdutoHistorico.Salvar(histo, true);
							_uowSciex.CommandStackSciex.Save();

							analistaVez.SituacaoLESetado = 0;
							_uowSciex.CommandStackSciex.Analista.Salvar(analistaVez);
							_uowSciex.CommandStackSciex.Save();

							int index = analistasLE.FindIndex(nd => nd.IdAnalista == analistaVez.IdAnalista);
							var prox = analistasLE.ElementAtOrDefault((analistasLE.Count - 1) == index ? 0 : index + 1);

							prox.SituacaoLESetado = 1;
							_uowSciex.CommandStackSciex.Analista.Salvar(prox);
							_uowSciex.CommandStackSciex.Save();
						}
						else if (item.StatusLEAlteracao == 2)
						{
							var analistaVez = analistasLE.Where(o => o.SituacaoLESetado == 1).FirstOrDefault();
							var prod = _uowSciex.QueryStackSciex.LEProduto.Selecionar(o => o.IdLe == item.IdLe);
							prod.StatusLEAlteracao = 3;
							prod.CpfResponsavel = analistaVez.CPF;
							prod.NomeResponsavel = analistaVez.Nome;

							_uowSciex.CommandStackSciex.DetachEntries();
							_uowSciex.CommandStackSciex.LEProduto.Salvar(prod);
							_uowSciex.CommandStackSciex.Save();

							LEProdutoHistoricoEntity histo = new LEProdutoHistoricoEntity();
							histo.DataOcorrencia = GetDateTimeNowUtc();
							histo.SituacaoLEAlteracao = 3;
							histo.DescricaoObservacao = "DISTRIBUIÇÃO AUTOMÁTICA";
							histo.CpfCnpjResponsavel = null;
							histo.NomeResponsavel = null;
							histo.IdLe = prod.IdLe;
							_uowSciex.CommandStackSciex.DetachEntries();
							_uowSciex.CommandStackSciex.LEProdutoHistorico.Salvar(histo, true);
							_uowSciex.CommandStackSciex.Save();

							analistaVez.SituacaoLESetado = 0;
							_uowSciex.CommandStackSciex.Analista.Salvar(analistaVez);
							_uowSciex.CommandStackSciex.Save();

							int index = analistasLE.FindIndex(nd => nd.IdAnalista == analistaVez.IdAnalista);
							var prox = analistasLE.ElementAtOrDefault((analistasLE.Count - 1) == index ? 0 : index + 1);

							prox.SituacaoLESetado = 1;
							_uowSciex.CommandStackSciex.Analista.Salvar(prox);
							_uowSciex.CommandStackSciex.Save();
						}
						else
						{
							if (item.CpfResponsavel == null && item.NomeResponsavel == null)
							{
								var analistaVez = analistasLE.Where(o => o.SituacaoLESetado == 1).FirstOrDefault();
								var prod = _uowSciex.QueryStackSciex.LEProduto.Selecionar(o => o.IdLe == item.IdLe);
								prod.CpfResponsavel = analistaVez.CPF;
								prod.NomeResponsavel = analistaVez.Nome;

								_uowSciex.CommandStackSciex.DetachEntries();
								_uowSciex.CommandStackSciex.LEProduto.Salvar(prod);
								_uowSciex.CommandStackSciex.Save();

								LEProdutoHistoricoEntity histo = new LEProdutoHistoricoEntity();
								histo.DataOcorrencia = GetDateTimeNowUtc();
								histo.SituacaoLE = prod.StatusLE;
								histo.DescricaoObservacao = "DISTRIBUIÇÃO AUTOMÁTICA";
								histo.CpfCnpjResponsavel = null;
								histo.NomeResponsavel = null;
								histo.IdLe = prod.IdLe;
								_uowSciex.CommandStackSciex.LEProdutoHistorico.Salvar(histo);
								_uowSciex.CommandStackSciex.Save();


								analistaVez.SituacaoLESetado = 0;
								_uowSciex.CommandStackSciex.Analista.Salvar(analistaVez);
								_uowSciex.CommandStackSciex.Save();

								int index = analistasLE.FindIndex(nd => nd.IdAnalista == analistaVez.IdAnalista);
								var prox = analistasLE.ElementAtOrDefault((analistasLE.Count - 1) == index ? 0 : index + 1);

								prox.SituacaoLESetado = 1;
								_uowSciex.CommandStackSciex.Analista.Salvar(prox);
								_uowSciex.CommandStackSciex.Save();
							}
						}
					}
				}
				else
				{
					retornoLE.Mensagem = "Não há LEs para serem distribuidos!";
				}
			}
			#endregion

			_controleExecucaoServico2.StatusExecucao = String.IsNullOrEmpty(retornoLE.Mensagem) ? 1 : 2;
			_controleExecucaoServico2.MemoObjetoRetorno = String.IsNullOrEmpty(retornoLE.Mensagem) ? "DISTRIBUIÇÃO OK" : retornoLE.Mensagem;
			_controleExecucaoServico2.DataHoraExecucaoFim = GetDateTimeNowUtc();
			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServico2);
			_uowSciex.CommandStackSciex.Save();


			// Plano Exportacao (PE)
			var _controleExecucaoServico3 = new ControleExecucaoServicoEntity();
			_controleExecucaoServico3.DataHoraExecucaoInicio = GetDateTimeNowUtc();
			_controleExecucaoServico3.IdListaServico = 18;
			_controleExecucaoServico3.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
			_controleExecucaoServico3.NumeroCPFCNPJInteressado = "04407029000143";

			var listaPlanos = _uowSciex.QueryStackSciex.PlanoExportacao.Listar(o => (o.Situacao == 2) || (o.Situacao > 1 && o.NomeResponsavel == null && o.CpfResponsavel == null));

			#region Planos
			var analistasPlanos = _uowSciex.QueryStackSciex.Analista.Listar(o => o.SituacaoPlano == 1).OrderBy(p => p.IdAnalista).ToList();
			if (analistasPlanos.Count() == 0)
			{
				retornoPlano.Mensagem = "Não há analistas ativos para distribuir os Planos de Exportação";
			}
			else
			{
				if (analistasPlanos.Where(o => o.SituacaoPlanoSetado == 1).Count() == 0)
				{
					var first = analistasPlanos.FirstOrDefault();
					first.SituacaoPlanoSetado = 1;
					_uowSciex.CommandStackSciex.DetachEntries();
					_uowSciex.CommandStackSciex.Analista.Salvar(first);
					_uowSciex.CommandStackSciex.Save();
				}

				if (listaPlanos.Count > 0)
				{
					foreach (var item in listaPlanos)
					{
						if (item.Situacao == 2)
						{
							var analistaVez = analistasPlanos.Where(o => o.SituacaoPlanoSetado == 1).FirstOrDefault();
							var planoExportacao = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(o => o.IdPlanoExportacao == item.IdPlanoExportacao);
							planoExportacao.Situacao = 3;
							planoExportacao.CpfResponsavel = analistaVez.CPF;
							planoExportacao.NomeResponsavel = analistaVez.Nome;

							_uowSciex.CommandStackSciex.DetachEntries();
							_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(planoExportacao);
							_uowSciex.CommandStackSciex.Save();

							PEHistoricoEntity histo = new PEHistoricoEntity();
							histo.Data = GetDateTimeNowUtc();
							histo.SituacaoPlano = 3;
							histo.DescricaoObservacao = "DISTRIBUIÇÃO AUTOMÁTICA";
							histo.CpfResponsavel = null;
							histo.NomeResponsavel = null;
							histo.IdPlanoExportacao = planoExportacao.IdPlanoExportacao;
							_uowSciex.CommandStackSciex.DetachEntries();
							_uowSciex.CommandStackSciex.PEHistorico.Salvar(histo, true);
							_uowSciex.CommandStackSciex.Save();

							analistaVez.SituacaoPlanoSetado = 0;
							_uowSciex.CommandStackSciex.Analista.Salvar(analistaVez);
							_uowSciex.CommandStackSciex.Save();

							int index = analistasPlanos.FindIndex(nd => nd.IdAnalista == analistaVez.IdAnalista);
							var prox = analistasPlanos.ElementAtOrDefault((analistasPlanos.Count - 1) == index ? 0 : index + 1);

							prox.SituacaoPlanoSetado = 1;
							_uowSciex.CommandStackSciex.Analista.Salvar(prox);
							_uowSciex.CommandStackSciex.Save();
						}
						else
						{
							if (item.CpfResponsavel == null && item.NomeResponsavel == null)
							{
								var analistaVez = analistasPlanos.Where(o => o.SituacaoPlanoSetado == 1).FirstOrDefault();
								var planoExportacao = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(o => o.IdPlanoExportacao == item.IdPlanoExportacao);
								planoExportacao.CpfResponsavel = analistaVez.CPF;
								planoExportacao.NomeResponsavel = analistaVez.Nome;

								_uowSciex.CommandStackSciex.DetachEntries();
								_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(planoExportacao);
								_uowSciex.CommandStackSciex.Save();

								PEHistoricoEntity histo = new PEHistoricoEntity();
								histo.Data = GetDateTimeNowUtc();
								histo.SituacaoPlano = 3;
								histo.DescricaoObservacao = "DISTRIBUIÇÃO AUTOMÁTICA";
								histo.CpfResponsavel = null;
								histo.NomeResponsavel = null;
								histo.IdPlanoExportacao = planoExportacao.IdPlanoExportacao;
								_uowSciex.CommandStackSciex.PEHistorico.Salvar(histo);
								_uowSciex.CommandStackSciex.Save();


								analistaVez.SituacaoPlanoSetado = 0;
								_uowSciex.CommandStackSciex.Analista.Salvar(analistaVez);
								_uowSciex.CommandStackSciex.Save();

								int index = analistasPlanos.FindIndex(nd => nd.IdAnalista == analistaVez.IdAnalista);
								var prox = analistasPlanos.ElementAtOrDefault((analistasPlanos.Count - 1) == index ? 0 : index + 1);

								prox.SituacaoPlanoSetado = 1;
								_uowSciex.CommandStackSciex.Analista.Salvar(prox);
								_uowSciex.CommandStackSciex.Save();
							}
						}
					}
				}
				else
				{
					retornoPlano.Mensagem = "Não há Planos de Exportação para serem distribuidos!";
				}
			}
			#endregion

			_controleExecucaoServico3.StatusExecucao = String.IsNullOrEmpty(retornoPlano.Mensagem) ? 1 : 2;
			_controleExecucaoServico3.MemoObjetoRetorno = String.IsNullOrEmpty(retornoPlano.Mensagem) ? "DISTRIBUIÇÃO OK" : retornoPlano.Mensagem;
			_controleExecucaoServico3.DataHoraExecucaoFim = GetDateTimeNowUtc();
			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServico3);
			_uowSciex.CommandStackSciex.Save();


			//SOLICITACOES
			var _controleExecucaoServico4 = new ControleExecucaoServicoEntity();
			_controleExecucaoServico4.DataHoraExecucaoInicio = GetDateTimeNowUtc();
			_controleExecucaoServico4.IdListaServico = 18;
			_controleExecucaoServico4.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
			_controleExecucaoServico4.NumeroCPFCNPJInteressado = "04407029000143";

			var listaSolics = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Listar(o => (o.Status == 2) 
																							|| 
																							(o.Status > 1 && o.NomeResponsavel == null && o.CpfResponsavel == null)
																							);

			#region Solicitacoes
			var analistaSolic = _uowSciex.QueryStackSciex.Analista.Listar(o => o.Solicitacao == 1).OrderBy(p => p.IdAnalista).ToList();

			if (analistaSolic.Count == 0)
			{
				retornoSolicitacoes.Mensagem = "Não há analistas ativos para distribuir as Solicitações";
			}
			else
			 {
				if (analistaSolic.Where(o => o.SituacaoSolicitacaoSetado == 1).Count() == 0)
				{
					var first = analistaSolic.FirstOrDefault();
					first.SituacaoSolicitacaoSetado = 1;
					_uowSciex.CommandStackSciex.DetachEntries();
					_uowSciex.CommandStackSciex.Analista.Salvar(first);
					_uowSciex.CommandStackSciex.Save();
				}
				if (listaSolics.Count > 0)
				{
					foreach (var item in listaSolics)
					{
						if (item.Status == 2)
						{
							var analistaVez = analistaSolic.Where(o => o.SituacaoSolicitacaoSetado == 1).FirstOrDefault();
							var solic = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.Id == item.Id);
							solic.Status = 3;
							solic.CpfResponsavel = analistaVez.CPF;
							solic.NomeResponsavel = analistaVez.Nome;

							_uowSciex.CommandStackSciex.DetachEntries();
							_uowSciex.CommandStackSciex.PRCSolicitacaoAlteracao.Salvar(solic);
							_uowSciex.CommandStackSciex.Save();

							analistaVez.SituacaoSolicitacaoSetado = 0;
							_uowSciex.CommandStackSciex.Analista.Salvar(analistaVez);
							_uowSciex.CommandStackSciex.Save();

							int index = analistaSolic.FindIndex(nd => nd.IdAnalista == analistaVez.IdAnalista);
							var prox = analistaSolic.ElementAtOrDefault((analistaSolic.Count - 1) == index ? 0 : index + 1);

							prox.SituacaoSolicitacaoSetado = 1;
							_uowSciex.CommandStackSciex.Analista.Salvar(prox);
							_uowSciex.CommandStackSciex.Save();

						}
						else
						{
							if (item.CpfResponsavel == null && item.NomeResponsavel == null)
							{
								var analistaVez = analistaSolic.Where(o => o.SituacaoSolicitacaoSetado == 1).FirstOrDefault();
								var solic = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.Id == item.Id);
								solic.CpfResponsavel = analistaVez.CPF;
								solic.NomeResponsavel = analistaVez.Nome;

								_uowSciex.CommandStackSciex.DetachEntries();
								_uowSciex.CommandStackSciex.PRCSolicitacaoAlteracao.Salvar(solic);
								_uowSciex.CommandStackSciex.Save();

								analistaVez.SituacaoSolicitacaoSetado = 0;
								_uowSciex.CommandStackSciex.Analista.Salvar(analistaVez);
								_uowSciex.CommandStackSciex.Save();

								int index = analistaSolic.FindIndex(nd => nd.IdAnalista == analistaVez.IdAnalista);
								var prox = analistaSolic.ElementAtOrDefault((analistaSolic.Count - 1) == index ? 0 : index + 1);

								prox.SituacaoSolicitacaoSetado = 1;
								_uowSciex.CommandStackSciex.Analista.Salvar(prox);
								_uowSciex.CommandStackSciex.Save();
							}
						}
					}
				}
				else
				{
					retornoSolicitacoes.Mensagem = "Não há Solicitações para serem distribuidas!";
				}
			}
			#endregion

			_controleExecucaoServico4.StatusExecucao = String.IsNullOrEmpty(retornoSolicitacoes.Mensagem) ? 1 : 2;
			_controleExecucaoServico4.MemoObjetoRetorno = String.IsNullOrEmpty(retornoSolicitacoes.Mensagem) ? "DISTRIBUIÇÃO OK" : retornoSolicitacoes.Mensagem;
			_controleExecucaoServico4.DataHoraExecucaoFim = GetDateTimeNowUtc();
			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServico4);
			_uowSciex.CommandStackSciex.Save();

			var retorno = new PliVM();
			var msgRetPli = (String.IsNullOrEmpty(retornoPLI.Mensagem) ? "DISTRIBUIÇÃO PLI OK" : retornoPLI.Mensagem);
			var msgRetLe = (String.IsNullOrEmpty(retornoLE.Mensagem) ? "DISTRIBUIÇÃO LE OK" : retornoLE.Mensagem);
			var msgRetPlano = (String.IsNullOrEmpty(retornoPlano.Mensagem) ? "DISTRIBUIÇÃO PLANOS OK" : retornoPlano.Mensagem);
			var msgSolicitacao = (String.IsNullOrEmpty(retornoSolicitacoes.Mensagem) ? "DISTRIBUIÇÃO SOLICITAÇÕES OK" : retornoSolicitacoes.Mensagem);
			retorno.Mensagem = $@"Retorno Distribuição PLI: {msgRetPli} | Retorno Distribuição LE: {msgRetLe} | Retorno Distribuição PE: {msgRetPlano} | "
								+$"Retorno Distribuição Solicitações Alteração: {msgSolicitacao} ";
			return retorno;
		}
	}
}