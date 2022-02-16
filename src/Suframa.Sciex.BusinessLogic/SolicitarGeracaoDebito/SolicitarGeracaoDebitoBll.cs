using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Resources;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.DataAccess.RestService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Suframa.Sciex.BusinessLogic
{

	public class SolicitarGeracaoDebitoBll : ISolicitarGeracaoDebitoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IArredacacaoApi _arredacacaoApi;
		private readonly IPliBll _pliBll;
		private readonly IControleExecucaoServicoBll _controleExecucaoServicoBll;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IPliHistoricoBll _pliHistoricoBll;


		public SolicitarGeracaoDebitoBll(IUnitOfWorkSciex uowSciex,
			IArredacacaoApi arredacacaoApi, IControleExecucaoServicoBll controleExecucaoServicoBll,
			IPliBll pliBll, IUsuarioLogado IUsuarioLogado, IPliHistoricoBll pliHistoricoBll)
		{
			_uowSciex = uowSciex;
			_arredacacaoApi = arredacacaoApi;
			_controleExecucaoServicoBll = controleExecucaoServicoBll;
			_pliBll = pliBll;
			_IUsuarioLogado = IUsuarioLogado;
			_pliHistoricoBll = pliHistoricoBll;
		}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}

		public void GerarDebito()
		{
			string erroSac = "Erro ao se comunicar com o SAC.";
			ControleExecucaoServicoEntity _controleGeralServico = new ControleExecucaoServicoEntity();
			_controleGeralServico.DataHoraExecucaoInicio = GetDateTimeNowUtc();
			
			_controleGeralServico.IdListaServico = 5; // gerarDebito
			_controleGeralServico.MemoObjetoEnvio = "Executando serviço";

			try
			{
				CalularTcif();


				StringBuilder listaControleDebitoGerado = new StringBuilder();

				var listaPLI = _uowSciex.QueryStackSciex.Pli.Listar(o => o.TaxaPli.StatusTaxa == 1 || o.TaxaPli.StatusTaxa == 4);


				foreach (var item in listaPLI)
				{
					ControleExecucaoServicoEntity _ControleExecucaoServicoVM = new ControleExecucaoServicoEntity();
					var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));
					_ControleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
					_ControleExecucaoServicoVM.IdListaServico = 5; // gerarDebito

					SolicitarGeracaoDebitoVM _solicitarGeracaoDebitoVM;
					_solicitarGeracaoDebitoVM = MontarSolicitarGeracaoDebitoVM(item);

					// adiciona json controle servico
					_ControleExecucaoServicoVM.MemoObjetoEnvio = _solicitarGeracaoDebitoVM.ToJson();

					// valida Json

					if (item.TaxaPli.TaxaPliMercadoria == null || item.TaxaPli.TaxaPliMercadoria.Count == 0)
					{
						_solicitarGeracaoDebitoVM.Debito = new List<DebitoGerado>() {
											new DebitoGerado() {
													MensagemErro = "PLI_ID = " + item.IdPLI.ToString() + ". Não possui Taxa Pli Mercadoria."
											}
						};
					}
					if (item.TaxaPli.DataEnvioPLI == null)
					{
						_solicitarGeracaoDebitoVM.Debito = new List<DebitoGerado>() {
											new DebitoGerado() {
													MensagemErro = "PLI_ID = " + item.IdPLI.ToString() + ". Campo Data Envio Pli vazio."
											}
						};						
					}
					else
					{
						try
						{
							_solicitarGeracaoDebitoVM = _arredacacaoApi.RegistrarDebito(_solicitarGeracaoDebitoVM);
							
						}
						catch (Exception ex)
						{
							if(ex.Message == "Erro processamento Sac")
							{
								_ControleExecucaoServicoVM.MemoObjetoRetorno += ex.Message;
							}
							else
							{
								_ControleExecucaoServicoVM.MemoObjetoRetorno += erroSac;
							}
						}
						// envio para SAC				

					}

					if (_solicitarGeracaoDebitoVM != null)
					{

						_ControleExecucaoServicoVM.MemoObjetoRetorno += _solicitarGeracaoDebitoVM.requestBody ??
													(_solicitarGeracaoDebitoVM.Debito == null
																	? "Não gerou Debito"
																	: _solicitarGeracaoDebitoVM.Debito.FirstOrDefault(q => !string.IsNullOrEmpty(q.MensagemErro)).MensagemErro);

						if(_solicitarGeracaoDebitoVM.Debito == null && _solicitarGeracaoDebitoVM.MensagemErro == "Erro processamento Sac")
						{
							AtualizarPliDebitoErro(item, _solicitarGeracaoDebitoVM);
							_ControleExecucaoServicoVM.StatusExecucao = 2;
						}
						else if (_solicitarGeracaoDebitoVM.Debito == null)
						{
							_ControleExecucaoServicoVM.MemoObjetoRetorno += erroSac;
						}
						else if (_solicitarGeracaoDebitoVM.Debito.Count > 0 && !_solicitarGeracaoDebitoVM.Debito.Any(q => !string.IsNullOrEmpty(q.MensagemErro)))
						{
							AtualizarPliDebitoSucesso(item, _solicitarGeracaoDebitoVM);
							_ControleExecucaoServicoVM.StatusExecucao = 1;
						}
						else
						{
							AtualizarPliDebitoErro(item, _solicitarGeracaoDebitoVM);
							_ControleExecucaoServicoVM.StatusExecucao = 2;
						}

					}
					else
					{
						_ControleExecucaoServicoVM.MemoObjetoRetorno += "Não gerou _solicitarGeracaoDebitoVM";
					}

					

					_ControleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
					_ControleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
					_ControleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";

					_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_ControleExecucaoServicoVM);
					_uowSciex.CommandStackSciex.Save();
				}

				_controleGeralServico.MemoObjetoRetorno = "Fim da execução serviço - Sucesso";
				_controleGeralServico.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleGeralServico.StatusExecucao = 1;
				_controleGeralServico.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				_controleGeralServico.NumeroCPFCNPJInteressado = "04407029000143";
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleGeralServico);
				_uowSciex.CommandStackSciex.Save();
			}
			catch (Exception ex)
			{
				_controleGeralServico.MemoObjetoRetorno += " Erro na execução do serviço: " + ex.Message;
				_controleGeralServico.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleGeralServico.StatusExecucao = 2;
				_controleGeralServico.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				_controleGeralServico.NumeroCPFCNPJInteressado = "04407029000143";
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleGeralServico);
				_uowSciex.CommandStackSciex.Save();

			}
		}


		private void AtualizarPliDebitoSucesso(PliEntity pli, SolicitarGeracaoDebitoVM solicitarGeracaoDebitoVM)
		{
			_uowSciex.CommandStackSciex.DetachEntries();
			// RN06
			var _TaxaPLI = _uowSciex.QueryStackSciex.TaxaPli.Selecionar(o => o.IdPli == pli.IdPLI);
			_TaxaPLI.StatusTaxa = 3; //DEBITO GERADO
			_TaxaPLI.DataEnvioSac = GetDateTimeNowUtc();
			_uowSciex.CommandStackSciex.TaxaPli.Salvar(_TaxaPLI);
			_uowSciex.CommandStackSciex.Save();


			if (solicitarGeracaoDebitoVM.Debito.Count == 1)
			{
				var _taxaPliDebito = _uowSciex.QueryStackSciex.TaxaPliDebito.Selecionar(q => q.IdPli == pli.IdPLI);

				_taxaPliDebito.NumeroDebito = solicitarGeracaoDebitoVM.Debito.FirstOrDefault().Numero;
				_taxaPliDebito.AnoDebito = Convert.ToInt16(solicitarGeracaoDebitoVM.Debito.FirstOrDefault().AnoDebito);				
				_taxaPliDebito.DataDebitoVencimento = Convert.ToDateTime(solicitarGeracaoDebitoVM.Debito.FirstOrDefault().DataVencimento);

				_uowSciex.CommandStackSciex.TaxaPliDebito.Salvar(_taxaPliDebito);
			}
			else if (solicitarGeracaoDebitoVM.Debito.Count > 1)
			{
				var _taxaPliDebitoCapa = _uowSciex.QueryStackSciex.TaxaPliDebito.Selecionar(q => q.IdPli == pli.IdPLI);

				_taxaPliDebitoCapa.NumeroDebito = solicitarGeracaoDebitoVM.Debito.FirstOrDefault(q=> q.ValorCapa != 0).Numero;
				_taxaPliDebitoCapa.AnoDebito = Convert.ToInt16(solicitarGeracaoDebitoVM.Debito.FirstOrDefault(q => q.ValorCapa != 0).AnoDebito);
				_taxaPliDebitoCapa.DataDebitoVencimento = Convert.ToDateTime(solicitarGeracaoDebitoVM.Debito.FirstOrDefault(q => q.ValorCapa != 0).DataVencimento);

				_uowSciex.CommandStackSciex.TaxaPliDebito.Salvar(_taxaPliDebitoCapa);

				var _taxaPliDebitoItens = new TaxaPliDebitoEntity()
				{
					IdPli = pli.IdPLI,
					NumeroDebito = solicitarGeracaoDebitoVM.Debito.FirstOrDefault(q => q.ValorItens != 0).Numero,
					AnoDebito = Convert.ToInt16(solicitarGeracaoDebitoVM.Debito.FirstOrDefault(q => q.ValorItens != 0).AnoDebito),
					DataDebitoVencimento = Convert.ToDateTime(solicitarGeracaoDebitoVM.Debito.FirstOrDefault(q => q.ValorItens != 0).DataVencimento),
					NumeroControleCobrancaTCIF = (short)EnumStatusDebito.SUSPENDER_DEBITO
				};
				_uowSciex.CommandStackSciex.TaxaPliDebito.Salvar(_taxaPliDebitoItens);
			}

			// RN06
			TaxaPliHistoricoEntity _TaxaPliHistoricoEntity = new TaxaPliHistoricoEntity();
			_TaxaPliHistoricoEntity.IdPli = pli.IdPLI;
			_TaxaPliHistoricoEntity.StatusTaxa = 3; // DEBITO GERADO
			_TaxaPliHistoricoEntity.DataEvento = GetDateTimeNowUtc();
			_TaxaPliHistoricoEntity.RetornoSac = solicitarGeracaoDebitoVM.Debito.ToJson();

			_uowSciex.CommandStackSciex.TaxaPliHistorico.Salvar(_TaxaPliHistoricoEntity);
			_uowSciex.CommandStackSciex.Save();

			//RN06
			_uowSciex.CommandStackSciex.DetachEntries();

			var _Pli = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == pli.IdPLI);

			if (_Pli.StatusAnaliseVisual == 1)//SIM
			{
				_Pli.StatusPli = (byte)EnumPliStatus.AGUARDANDO_ANÁLISE_VISUAL;
			}
			else
			{
				_Pli.StatusPli = (byte)EnumPliStatus.AGUARDANDO_PROCESSAMENTO;
			}

			_Pli.Debito = solicitarGeracaoDebitoVM.Debito.FirstOrDefault(q => q.ValorCapa != 0).Numero;
			_Pli.DebitoAno = Convert.ToInt16(solicitarGeracaoDebitoVM.Debito.FirstOrDefault(q => q.ValorCapa != 0).AnoDebito);
			_Pli.DataDebitoGeracao = GetDateTimeNowUtc();

			//VALIDAR CAMPO VAZIO DESCRICAO SETOR
			_Pli.DescricaoSetor = string.IsNullOrEmpty(_Pli.DescricaoSetor) ? "-" : _Pli.DescricaoSetor;

			_uowSciex.CommandStackSciex.Pli.Salvar(_Pli);
			_uowSciex.CommandStackSciex.Save();

			//RN06
			PliHistoricoVM _PliHistoricoVM = new PliHistoricoVM();

			_PliHistoricoVM.DataEvento = GetDateTimeNowUtc();
			if (pli.StatusAnaliseVisual == 1)
			{
				_PliHistoricoVM.DescricaoStatusPli = EnumPliStatus.AGUARDANDO_ANÁLISE_VISUAL.ToString().Replace("_", " ");
				_PliHistoricoVM.StatusPli = (byte)EnumPliStatus.AGUARDANDO_ANÁLISE_VISUAL;
			}
			else
			{
				_PliHistoricoVM.DescricaoStatusPli = EnumPliStatus.AGUARDANDO_PROCESSAMENTO.ToString().Replace("_", " ");
				_PliHistoricoVM.StatusPli = (byte)EnumPliStatus.AGUARDANDO_PROCESSAMENTO;
			}
			_PliHistoricoVM.IdPLI = pli.IdPLI;
			_PliHistoricoVM.NomeResponsavel = "Administrador do Sistema - SUFRAMA";
			_PliHistoricoVM.CPFCNPJ = "04407029000143";
			_pliHistoricoBll.Salvar(_PliHistoricoVM);


		}



		private void AtualizarPliDebitoErro(PliEntity pli, SolicitarGeracaoDebitoVM solicitarGeracaoDebitoVM)
		{
			// RN07
			var _TaxaPLI = _uowSciex.QueryStackSciex.TaxaPli.Selecionar(o => o.IdPli == pli.IdPLI);
			_TaxaPLI.StatusTaxa = 4; //ERRO NO SAC
			_uowSciex.CommandStackSciex.TaxaPli.Salvar(_TaxaPLI);
			_uowSciex.CommandStackSciex.Save();

			// RN07
			TaxaPliHistoricoEntity _TaxaPliHistoricoEntity = new TaxaPliHistoricoEntity();
			_TaxaPliHistoricoEntity.IdPli = pli.IdPLI;
			_TaxaPliHistoricoEntity.StatusTaxa = Convert.ToInt16(_TaxaPLI.StatusTaxa); // ERRO SAC
			_TaxaPliHistoricoEntity.DataEvento = GetDateTimeNowUtc();
			_TaxaPliHistoricoEntity.RetornoSac = solicitarGeracaoDebitoVM.Debito.ToJson();
			_TaxaPliHistoricoEntity.Observacao = "Erro na geração do débito";
			_uowSciex.CommandStackSciex.TaxaPliHistorico.Salvar(_TaxaPliHistoricoEntity);
			_uowSciex.CommandStackSciex.Save();
		}


		private SolicitarGeracaoDebitoVM MontarSolicitarGeracaoDebitoVM(PliEntity pli)
		{
			SolicitarGeracaoDebitoVM solicitarGeracaoDebitoVM = new SolicitarGeracaoDebitoVM();

			solicitarGeracaoDebitoVM.Servico = new Servico();
			solicitarGeracaoDebitoVM.Servico.Codigo = pli.TaxaPli.NumeroServicoSAC.Value;

			//OCORRERÁ A MODIFICAÇÃO
			if (pli.IdEstruturaPropria != null)
			{
				solicitarGeracaoDebitoVM.NumeroProtocoloEnvio = pli.IdEstruturaPropria.Value;
			}
			else
			{
				solicitarGeracaoDebitoVM.NumeroProtocoloEnvio = pli.NumeroPli;
			}


			solicitarGeracaoDebitoVM.AnoCorrente = pli.Ano;

			solicitarGeracaoDebitoVM.NumeroPLI = pli.Ano.ToString() + "/" + pli.NumeroPli.ToString("d6");
			solicitarGeracaoDebitoVM.InscricaoSuframa = pli.InscricaoCadastral.Value;
			solicitarGeracaoDebitoVM.CNPJCPFImportador = pli.Cnpj;

			//* Solicitar Razao Social da ViewImportador
			var empresa = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(o => o.Cnpj == pli.Cnpj);
			solicitarGeracaoDebitoVM.RazaoSocialImportador = empresa.RazaoSocial;

			//Persiste taxa debito em outra tabela (Modificacao 04/02/2020)
			var taxaPliDebito = _uowSciex.QueryStackSciex.TaxaPliDebito.Selecionar(o => o.IdPli == pli.IdPLI);

			solicitarGeracaoDebitoVM.CobrarDebito = taxaPliDebito.NumeroControleCobrancaTCIF.Value;
			solicitarGeracaoDebitoVM.Localidade = pli.TaxaPli.CodigoLocalidade.Value;
			solicitarGeracaoDebitoVM.ValorDebito = pli.TaxaPli.ValorGeralTCIF.Value;
			solicitarGeracaoDebitoVM.ValorTcifPli = pli.TaxaPli.ValorTCIFPLI.Value;
			solicitarGeracaoDebitoVM.ValorTcifItens = pli.TaxaPli.ValorTotalTCIFItens.Value;
			solicitarGeracaoDebitoVM.ValorReducao = pli.TaxaPli.ValorGeralReducaoTCIF.Value;

			solicitarGeracaoDebitoVM.ExtratoEstrangeiro = new ExtratoEstrangeiro();

			solicitarGeracaoDebitoVM.ExtratoEstrangeiro.NumeroPLI = pli.Ano.ToString() + "/" + pli.NumeroPli.ToString("d6");
			solicitarGeracaoDebitoVM.ExtratoEstrangeiro.ValorMercadoriaEmReal = pli.TaxaPli.ValorTotalMercadoriaReais == null ? 0 : pli.TaxaPli.ValorTotalMercadoriaReais.Value;
			solicitarGeracaoDebitoVM.ExtratoEstrangeiro.ValorTCIFBasePLI = pli.TaxaPli.ValorBaseFatoGerador.Value;
			solicitarGeracaoDebitoVM.ExtratoEstrangeiro.LimitadorPLI = pli.TaxaPli.ValorPercentualLimitadorPli.Value;
			solicitarGeracaoDebitoVM.ExtratoEstrangeiro.ValorTCIFRealPLI = pli.TaxaPli.ValorPrevalenciaPLI.Value;
			if (pli.TaxaPli.DataEnvioPLI != null)
				solicitarGeracaoDebitoVM.ExtratoEstrangeiro.DataCotacaoDolar = pli.TaxaPli.DataEnvioPLI.Value.ToShortDateString();
			if (pli.TaxaPli.TaxaPliMercadoria.Any())
				solicitarGeracaoDebitoVM.ExtratoEstrangeiro.TaxaDolar = pli.TaxaPli.TaxaPliMercadoria.FirstOrDefault().ParidadeValor.Valor;


			solicitarGeracaoDebitoVM.ExtratoEstrangeiro.ListaNCM = new List<ListaNCM>();

			foreach (var itemMercadoria in pli.PLIMercadoria)
			{
				ListaNCM ncm = new ListaNCM();
				ncm.NumeroNCM = Convert.ToInt64(itemMercadoria.CodigoNCMMercadoria);
				if (itemMercadoria.TaxaPliMercadoria != null)
				{
					ncm.ValorMercadoriaMoedaNegociada = itemMercadoria.TaxaPliMercadoria.ValorMercadoriaMoedaNegociada == null ? 0 : itemMercadoria.TaxaPliMercadoria.ValorMercadoriaMoedaNegociada.Value;
					ncm.ValorMercadoriaEmReal = itemMercadoria.TaxaPliMercadoria.ValorMercadoriaReais == null ? 0 : itemMercadoria.TaxaPliMercadoria.ValorMercadoriaReais.Value;
					ncm.MoedaCambio = itemMercadoria.TaxaPliMercadoria.ParidadeValor.Moeda.CodigoMoeda;
					ncm.DataCambio = itemMercadoria.TaxaPliMercadoria.DataParidade.Value.ToShortDateString();
					ncm.TaxaCambio = itemMercadoria.TaxaPliMercadoria.ParidadeValor.Valor;
				}
				ncm.QtdItens = itemMercadoria.PliDetalheMercadoria.Count();

				ncm.Itens = new List<Itens>();

				foreach (var itemDetalhe in itemMercadoria.PliDetalheMercadoria)
				{
					Itens itens = new Itens();
					itens.Numero = itemMercadoria.CodigoNCMMercadoria;
					if (itemDetalhe.TaxaPliDetalheMercadoria != null)
					{
						itens.NomeItem = itemDetalhe.TaxaPliDetalheMercadoria.DescricaoDetalhe;
						itens.ValorItemEmReal = itemDetalhe.TaxaPliDetalheMercadoria.ValorUnidadeReais == null ? 0 : itemDetalhe.TaxaPliDetalheMercadoria.ValorUnidadeReais.Value;
						itens.ValorTCIFBaseItem = itemDetalhe.TaxaPliDetalheMercadoria.ValorBaseFatoGeradorItem == null ? 0 : itemDetalhe.TaxaPliDetalheMercadoria.ValorBaseFatoGeradorItem.Value;
						itens.LimitadorItem = itemDetalhe.TaxaPliDetalheMercadoria.ValorPercentualLimitadorItem == null ? 0 : itemDetalhe.TaxaPliDetalheMercadoria.ValorPercentualLimitadorItem.Value;
						itens.ValorTCIFRealItem = itemDetalhe.TaxaPliDetalheMercadoria.ValorPrevalenciaItem == null ? 0 : itemDetalhe.TaxaPliDetalheMercadoria.ValorPrevalenciaItem.Value;
						itens.ValorTCIFRealItem = itemDetalhe.TaxaPliDetalheMercadoria.ValorPrevalenciaItem == null ? 0 : itemDetalhe.TaxaPliDetalheMercadoria.ValorPrevalenciaItem.Value;
						itens.Isencao = itemDetalhe.TaxaPliDetalheMercadoria.Isencao == null ? "0" : itemDetalhe.TaxaPliDetalheMercadoria.Isencao.ToString();
						itens.Reducao = itemDetalhe.TaxaPliDetalheMercadoria.ValorReducaoItem == null ? 0 : itemDetalhe.TaxaPliDetalheMercadoria.ValorReducaoItem.Value;
					}

					ncm.Itens.Add(itens);
				}

				solicitarGeracaoDebitoVM.ExtratoEstrangeiro.ListaNCM.Add(ncm);
			}


			return solicitarGeracaoDebitoVM;
		}


		private void CalularTcif()
		{
			_uowSciex.CommandStackSciex.Salvar("EXEC dbo.ST_SCIEX_CALCULAR_TAXA");
		}
	}
}