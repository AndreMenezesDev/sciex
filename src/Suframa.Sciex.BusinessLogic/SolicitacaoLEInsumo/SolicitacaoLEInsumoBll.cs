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
using System.IO;
using System.Text;
using System.Security.Cryptography;
using NLog.Common;
using Suframa.Sciex.BusinessLogic.Pss;

namespace Suframa.Sciex.BusinessLogic
{
	public class SolicitacaoLEInsumoBll : ISolicitacaoLEInsumoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _IUsuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;

		public SolicitacaoLEInsumoBll(
			IUnitOfWorkSciex uowSciex,
			IUsuarioPssBll usuarioLogado,
			IViewImportadorBll viewImportadorBll,
			IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			_uowSciex = uowSciex;
			_IUsuarioLogado = usuarioLogado;
			_IViewImportadorBll = viewImportadorBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;

		}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}

		public string LeituraArquivoInserirDados()
		{
			//try
			//{
			//RN01
			List<EstruturaPropriaPliEntity> listaArquivosProprios =
				_uowSciex.QueryStackSciex.EstruturaPropriaPLI.Listar<EstruturaPropriaPliEntity>(
					o => o.StatusProcessamentoArquivo == (byte)EnumEstruturaPropriaArquivoProcessamento.ENVIADO_A_SUFRAMA && o.TipoArquivo == 2
					).OrderByDescending(o => o.StatusProcessamentoArquivo).ToList();
			//FIM RN01

			if (listaArquivosProprios.Any())
			{
				foreach (EstruturaPropriaPliEntity item in listaArquivosProprios)
				{
					var estruturaPropriaAtual = _uowSciex.QueryStackSciex.EstruturaPropriaPLI.Selecionar(o => o.IdEstruturaPropria == item.IdEstruturaPropria);

					int inscricaoCadastral = item.InscricaoCadastral.Value;

					ViewImportadorEntity viewImportador =
						_uowSciex.QueryStackSciex.ViewImportador.Selecionar(x => x.InscricaoCadastral == inscricaoCadastral);

					//inicio RN02
					if (viewImportador != null && viewImportador.DescricaoSituacaoInscricao.ToUpper() == "ATIVA")
					{
						//RN04 Inicioo Controle de Extração
						estruturaPropriaAtual.DataInicioProcessamento = GetDateTimeNowUtc();
						estruturaPropriaAtual.StatusProcessamentoArquivo = 2;
						estruturaPropriaAtual.DescricaoPendenciaImportador = null;
						_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(estruturaPropriaAtual);
						// FIM RN04

						#region Controle de execução

						ControleExecucaoServicoEntity objControle = new ControleExecucaoServicoEntity();
						objControle.DataHoraExecucaoInicio = GetDateTimeNowUtc();
						objControle.StatusExecucao = 0;
						objControle.IdListaServico = 22;
						objControle.MemoObjetoEnvio = "Tabela: SCIEX_ESTRUTURA_PROPRIA_ARQUIVO: " + item.IdEstruturaPropria.ToString();
						objControle.NomeCPFCNPJInteressado = item.NomeUsuarioEnvio;
						objControle.NumeroCPFCNPJInteressado = item.LoginUsuarioEnvio;
						_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(objControle);

						#endregion

						byte[] arquivo = item.EstruturaPropriaPliArquivo.Arquivo;
						StreamReader leitor = new StreamReader(new MemoryStream(arquivo));

						string linha;

						int linhaCount = 0;

						while ((linha = leitor.ReadLine()) != null)
						{
							linhaCount++;
							var solicLE = new SolicitacaoLeInsumoEntity();
							if (linha.Length == 4037)
							{
								string tipoRegistro = linha.Substring(0, 1);
								solicLE.TipoInsumo = tipoRegistro;

								var ncm = linha.Substring(1, 8);
								solicLE.CodigoNCM = ncm;

								var destaque = linha.Substring(9, 4);
								solicLE.CodigoDestaque = destaque.Trim() != "" ? Convert.ToInt32(destaque) : 0;

								var descricao = linha.Substring(13, 254).Trim();
								solicLE.DescricaoInsumo = descricao;

								var coefTec = linha.Substring(267, 15);
								var ct = coefTec.Substring(0, 7) + "," + coefTec.Substring(7, 8);
								solicLE.ValorCoeficienteTecnico = Convert.ToDecimal(ct);

								var undMed = linha.Substring(282, 2);
								solicLE.CodigoUnidade = Convert.ToDecimal(undMed);

								var partNumber = linha.Substring(284, 30).Trim();
								solicLE.CodigoPartNumber = partNumber;

								var especTec = linha.Substring(314, 3722).Trim();
								solicLE.DescricaoEspecificacaoTecnica = especTec;

								solicLE.IdEstruturaPropriaLe = item.IdEstruturaPropria;
								solicLE.SituacaoInsumo = 1;
								solicLE.NumeroLinha = linhaCount;

								_uowSciex.CommandStackSciex.SolicitacaoLeInsumo.Salvar(solicLE);
								_uowSciex.CommandStackSciex.Save();
							}
						}

						objControle.StatusExecucao = 1;
						objControle.DataHoraExecucaoFim = GetDateTimeNowUtc();
						_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(objControle);
						_uowSciex.CommandStackSciex.Save();
					}
					else
					{
						//RN02 Arquivo com Pendencia
						estruturaPropriaAtual.DescricaoPendenciaImportador = "SITUAÇÃO CADASTRAL DO IMPORTADOR NÃO ESTÁ ATIVA";
						_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(estruturaPropriaAtual);
						_uowSciex.CommandStackSciex.Save();
						// FIM RN02
					}

					//RN04 Fim do Controle de Extração
					//estruturaPropriaAtual.DataFimProcessamento = GetDateTimeNowUtc();
					estruturaPropriaAtual.StatusProcessamentoArquivo = 3;
					_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(estruturaPropriaAtual);
					_uowSciex.CommandStackSciex.Save();
					// FIM RN04
				}
			}

			int aguardandoValidacao = 1;
			List<SolicitacaoLeInsumoEntity> solics = _uowSciex.QueryStackSciex.SolicitacaoLeInsumo.Listar(o => o.SituacaoInsumo == aguardandoValidacao);

			var solicByid = solics.GroupBy(o => o.IdEstruturaPropriaLe);

			Boolean comErro = false;
			foreach (var item in solicByid)
			{
				var listsolic = _uowSciex.QueryStackSciex.SolicitacaoLeInsumo.Listar(o => o.IdEstruturaPropriaLe == item.Key
																							&& o.SituacaoInsumo == aguardandoValidacao);

				var estruturaPropriaPli = _uowSciex.QueryStackSciex.EstruturaPropriaPLI.Selecionar(o => o.IdEstruturaPropria == item.Key);

				ControleExecucaoServicoEntity objContro = new ControleExecucaoServicoEntity();
				objContro.DataHoraExecucaoInicio = GetDateTimeNowUtc();
				objContro.StatusExecucao = 0;
				objContro.IdListaServico = 23;
				objContro.MemoObjetoEnvio = "Tabela: SCIEX_ESTRUTURA_PROPRIA: " + estruturaPropriaPli.IdEstruturaPropria.ToString();
				objContro.NomeCPFCNPJInteressado = estruturaPropriaPli.NomeUsuarioEnvio;
				objContro.NumeroCPFCNPJInteressado = estruturaPropriaPli.LoginUsuarioEnvio;
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(objContro);
				_uowSciex.CommandStackSciex.Save();

				var flagProdutoJaExiste = false;

				foreach (var solic in listsolic)
				{
					var produto = _uowSciex.QueryStackSciex.LEProduto.Listar(o => o.InscricaoCadastral == estruturaPropriaPli.InscricaoCadastral
																					&& o.CodigoProdutoSuframa == estruturaPropriaPli.EstruturaPropriaLE.CodigoProduto
																					&& o.CodigoTipoProduto == estruturaPropriaPli.EstruturaPropriaLE.CodigoTipoProduto
																					&& o.CodigoNCM == estruturaPropriaPli.EstruturaPropriaLE.CodigoNCM
																					&& o.DescricaoModelo == estruturaPropriaPli.EstruturaPropriaLE.DescricaoModelo
																					&& o.CodigoUnidadeMedida == estruturaPropriaPli.EstruturaPropriaLE.CodigoUnidadeMedida
																					&& o.IdLe != solic.IdSolicitacaoLeInsumo
																					)
																				.FirstOrDefault();
					if (produto != null)
					{
						int produtoJaCadastrado = 600;
						comErro = true;
						ErroProcessamentoEntity erro = new ErroProcessamentoEntity();
						erro.IdEstruturaPropriaLE = estruturaPropriaPli.IdEstruturaPropria;
						erro.CodigoNivelErro = 2;
						erro.IdErroMensagem = 600;
						erro.IdPliMercadoriaOuPliDetalheMercadoria = solic.IdSolicitacaoLeInsumo;
						var msgErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == produtoJaCadastrado);
						erro.Descricao = msgErro.Descricao.Replace("lep_co_produto", produto.CodigoProduto.ToString());
						erro.CNPJImportador = estruturaPropriaPli.CNPJImportador;
						_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
						_uowSciex.CommandStackSciex.Save();

						flagProdutoJaExiste = true;
					}

					var solicitacaoDuplicada = _uowSciex.QueryStackSciex.SolicitacaoLeInsumo.Listar(o =>
															o.CodigoUnidade == solic.CodigoUnidade
															&& o.TipoInsumo == solic.TipoInsumo
															&& o.CodigoNCM == solic.CodigoNCM
															&& o.CodigoDestaque == solic.CodigoDestaque
															&& o.DescricaoInsumo == solic.DescricaoInsumo
															&& o.DescricaoEspecificacaoTecnica == solic.DescricaoEspecificacaoTecnica
															&& o.ValorCoeficienteTecnico == solic.ValorCoeficienteTecnico
															&& o.EstruturaPropriaLe.IdEstruturaPropria == solic.IdEstruturaPropriaLe);
					if (solicitacaoDuplicada.Count > 1)
					{
						comErro = true;
						ErroProcessamentoEntity erro = new ErroProcessamentoEntity();
						erro.IdEstruturaPropriaLE = estruturaPropriaPli.IdEstruturaPropria;
						erro.CodigoNivelErro = 3;
						erro.IdErroMensagem = 601;
						erro.IdPliMercadoriaOuPliDetalheMercadoria = solic.IdSolicitacaoLeInsumo;
						var msgErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 601);
						erro.Descricao = msgErro.Descricao.Replace("esp_nu_protocolo", estruturaPropriaPli.NumeroProtocolo.ToString());
						erro.CNPJImportador = estruturaPropriaPli.CNPJImportador;
						_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
						_uowSciex.CommandStackSciex.Save();
					}

					var unidadeMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == solic.CodigoUnidade);
					if (unidadeMed == null)
					{
						comErro = true;
						ErroProcessamentoEntity erro = new ErroProcessamentoEntity();
						erro.IdEstruturaPropriaLE = estruturaPropriaPli.IdEstruturaPropria;
						erro.CodigoNivelErro = 3;
						erro.IdErroMensagem = 602;
						erro.IdPliMercadoriaOuPliDetalheMercadoria = solic.IdSolicitacaoLeInsumo;
						var msgErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 602);
						erro.Descricao = msgErro.Descricao.Replace("sli_nu_linha", solic.NumeroLinha.ToString());
						erro.CNPJImportador = estruturaPropriaPli.CNPJImportador;
						_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
						_uowSciex.CommandStackSciex.Save();
					}

					var ncm = _uowSciex.QueryStackSciex.Ncm.Selecionar(o => o.CodigoNCM == solic.CodigoNCM);
					if (ncm == null)
					{
						comErro = true;
						ErroProcessamentoEntity erro = new ErroProcessamentoEntity();
						erro.IdEstruturaPropriaLE = estruturaPropriaPli.IdEstruturaPropria;
						erro.CodigoNivelErro = 3;
						erro.IdErroMensagem = 603;
						erro.IdPliMercadoriaOuPliDetalheMercadoria = solic.IdSolicitacaoLeInsumo;
						var msgErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 603);
						erro.Descricao = msgErro.Descricao.Replace("sli_nu_linha", solic.NumeroLinha.ToString());
						erro.CNPJImportador = estruturaPropriaPli.CNPJImportador;
						_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
						_uowSciex.CommandStackSciex.Save();
					}

					if (solic.TipoInsumo == "P")
					{
						var vwInsumoPadrao = _uowSciex.QueryStackSciex.ViewInsumoPadrao.Listar(o => o.CodigoProduto == estruturaPropriaPli.EstruturaPropriaLE.CodigoProduto
																													&& o.CodigoNCMMercadoria == solic.CodigoNCM);
						if (vwInsumoPadrao.Count() == 0)
						{
							comErro = true;
							ErroProcessamentoEntity erro = new ErroProcessamentoEntity();
							erro.IdEstruturaPropriaLE = estruturaPropriaPli.IdEstruturaPropria;
							erro.CodigoNivelErro = 3;
							erro.IdErroMensagem = 604;
							erro.IdPliMercadoriaOuPliDetalheMercadoria = solic.IdSolicitacaoLeInsumo;
							var msgErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 604);
							erro.Descricao = msgErro.Descricao.Replace("sli_nu_linha", solic.NumeroLinha.ToString());
							erro.CNPJImportador = estruturaPropriaPli.CNPJImportador;
							_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
							_uowSciex.CommandStackSciex.Save();
						}

						var destaquevalidation = _uowSciex.QueryStackSciex.ViewInsumoPadrao.Listar(o => o.CodigoProduto == estruturaPropriaPli.EstruturaPropriaLE.CodigoProduto
																									&& o.CodigoNCMMercadoria == solic.CodigoNCM
																									&& o.CodigoDetalheMercadoria == solic.CodigoDestaque);
						if (destaquevalidation.Count() == 0)
						{
							comErro = true;
							ErroProcessamentoEntity erro = new ErroProcessamentoEntity();
							erro.IdEstruturaPropriaLE = estruturaPropriaPli.IdEstruturaPropria;
							erro.CodigoNivelErro = 3;
							erro.IdErroMensagem = 605;
							erro.IdPliMercadoriaOuPliDetalheMercadoria = solic.IdSolicitacaoLeInsumo;
							var msgErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 605);
							erro.Descricao = msgErro.Descricao.Replace("sli_nu_linha", solic.NumeroLinha.ToString());
							erro.CNPJImportador = estruturaPropriaPli.CNPJImportador;
							_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
							_uowSciex.CommandStackSciex.Save();
						}
					}

					if (!comErro)
					{
						var saveSolic = _uowSciex.QueryStackSciex.SolicitacaoLeInsumo.Selecionar(o => o.IdSolicitacaoLeInsumo == solic.IdSolicitacaoLeInsumo);
						saveSolic.SituacaoInsumo = 2;
						_uowSciex.CommandStackSciex.DetachEntries();
						_uowSciex.CommandStackSciex.SolicitacaoLeInsumo.Salvar(saveSolic);
						_uowSciex.CommandStackSciex.Save();
					}
					else
					{
						var saveSolic = _uowSciex.QueryStackSciex.SolicitacaoLeInsumo.Selecionar(o => o.IdSolicitacaoLeInsumo == solic.IdSolicitacaoLeInsumo);
						saveSolic.SituacaoInsumo = 3;
						_uowSciex.CommandStackSciex.DetachEntries();
						_uowSciex.CommandStackSciex.SolicitacaoLeInsumo.Salvar(saveSolic);
						_uowSciex.CommandStackSciex.Save();

					}

					comErro = false;
				}

				if (flagProdutoJaExiste)
				{
					List<SolicitacaoLeInsumoEntity> _situacao2 = new List<SolicitacaoLeInsumoEntity>();
					List<SolicitacaoLeInsumoEntity> _situacao3 = _uowSciex.QueryStackSciex.SolicitacaoLeInsumo.Listar(o =>
																						o.IdEstruturaPropriaLe == item.Key && o.SituacaoInsumo == 3);

					FinalizarProcessamento(estruturaPropriaPli, objContro, _situacao2, _situacao3);

					break;
				}


				var situacao2 = _uowSciex.QueryStackSciex.SolicitacaoLeInsumo.Listar(o => o.IdEstruturaPropriaLe == item.Key && o.SituacaoInsumo == 2);
				var situacao3 = _uowSciex.QueryStackSciex.SolicitacaoLeInsumo.Listar(o => o.IdEstruturaPropriaLe == item.Key && o.SituacaoInsumo == 3);

				//if(situacao2.Count > 0)
				//{
				var ultimoProdEmpresa = _uowSciex.QueryStackSciex.LEProduto.Listar(o => o.Cnpj == estruturaPropriaPli.CNPJImportador).LastOrDefault();
				var CodigoProdutoSuframa = 0;
				if (ultimoProdEmpresa == null)
					CodigoProdutoSuframa = 1;
				else
					CodigoProdutoSuframa = ultimoProdEmpresa.CodigoProduto + 1;

				var prod = new LEProdutoEntity();
				prod.CodigoProduto = CodigoProdutoSuframa;
				prod.CodigoProdutoSuframa = Convert.ToInt32(estruturaPropriaPli.EstruturaPropriaLE.CodigoProduto);
				prod.CodigoTipoProduto = Convert.ToInt32(estruturaPropriaPli.EstruturaPropriaLE.CodigoTipoProduto);
				prod.CodigoNCM = estruturaPropriaPli.EstruturaPropriaLE.CodigoNCM;
				prod.DescricaoModelo = estruturaPropriaPli.EstruturaPropriaLE.DescricaoModelo;
				prod.CodigoUnidadeMedida = estruturaPropriaPli.EstruturaPropriaLE.CodigoUnidadeMedida;
				prod.StatusLE = situacao3.Count > 0 ? 1 : 2;
				if (prod.StatusLE == 2)
				{
					prod.DataEnvio = GetDateTimeNowUtc();
				}
				prod.DataCadastro = DateTime.Now;
				prod.CodigoModeloEmpresa = estruturaPropriaPli.EstruturaPropriaLE.CodigoModeloEmpresa;
				prod.DescricaoCentroCusto = estruturaPropriaPli.EstruturaPropriaLE.DescricaoCentroCusto;
				prod.Cnpj = estruturaPropriaPli.CNPJImportador;
				prod.InscricaoCadastral = estruturaPropriaPli.InscricaoCadastral.Value;
				prod.RazaoSocial = estruturaPropriaPli.RazaoSocialImportador;

				_uowSciex.CommandStackSciex.LEProduto.Salvar(prod);
				_uowSciex.CommandStackSciex.Save();

				LEProdutoHistoricoEntity histo = new LEProdutoHistoricoEntity();
				histo.DataOcorrencia = GetDateTimeNowUtc();
				histo.SituacaoLE = situacao3.Count > 0 ? 1 : 2; ;
				histo.DescricaoObservacao = "CADASTRO DA LE POR ESTRUTURA PRÓPRIA";
				histo.CpfCnpjResponsavel = "0";
				histo.NomeResponsavel = "VALIDAR ESTRUTURA PROPRIA LE";
				histo.IdLe = prod.IdLe;
				_uowSciex.CommandStackSciex.DetachEntries();
				_uowSciex.CommandStackSciex.LEProdutoHistorico.Salvar(histo, true);
				_uowSciex.CommandStackSciex.Save();

				if (situacao2.Count > 0)
				{
					foreach (var itemSituacao in situacao2)
					{
						LEInsumoEntity insumo = new LEInsumoEntity();
						insumo.SituacaoInsumo = 1; //bloq
						insumo.IdLe = prod.IdLe;
						insumo.CodigoProduto = prod.CodigoProduto;
						insumo.CodigoInsumo = itemSituacao.NumeroLinha;
						insumo.TipoInsumo = itemSituacao.TipoInsumo;
						insumo.CodigoNCM = itemSituacao.CodigoNCM;
						insumo.CodigoDetalhe = itemSituacao.CodigoDestaque;
						insumo.DescricaoInsumo = itemSituacao.TipoInsumo.Equals("P") ? BuscarDescricaoEmListaPadrao(itemSituacao, prod.CodigoProdutoSuframa)
																					: itemSituacao.DescricaoInsumo;
						insumo.CodigoUnidadeMedida = Convert.ToInt32(itemSituacao.CodigoUnidade.Value);
						insumo.ValorCoeficienteTecnico = itemSituacao.ValorCoeficienteTecnico.Value;
						insumo.CodigoPartNumber = itemSituacao.CodigoPartNumber;
						insumo.DescricaoEspecTecnica = itemSituacao.DescricaoEspecificacaoTecnica;

						_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo);
						_uowSciex.CommandStackSciex.Save();
					}
				}

				if (situacao3.Count > 0)
				{
					foreach (var itemSituacao in situacao3)
					{
						LEInsumoEntity insumo = new LEInsumoEntity();
						insumo.SituacaoInsumo = 1; //bloq
						insumo.IdLe = prod.IdLe;
						insumo.CodigoProduto = prod.CodigoProduto;
						insumo.CodigoInsumo = itemSituacao.NumeroLinha;
						insumo.TipoInsumo = itemSituacao.TipoInsumo;
						insumo.CodigoNCM = itemSituacao.CodigoNCM;
						insumo.CodigoDetalhe = itemSituacao.CodigoDestaque;
						insumo.DescricaoInsumo = itemSituacao.TipoInsumo.Equals("P") ? BuscarDescricaoEmListaPadrao(itemSituacao, prod.CodigoProdutoSuframa)
																					: itemSituacao.DescricaoInsumo;
						insumo.CodigoUnidadeMedida = Convert.ToInt32(itemSituacao.CodigoUnidade.Value);
						insumo.ValorCoeficienteTecnico = itemSituacao.ValorCoeficienteTecnico.Value;
						insumo.CodigoPartNumber = itemSituacao.CodigoPartNumber;
						insumo.DescricaoEspecTecnica = itemSituacao.DescricaoEspecificacaoTecnica;

						_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo);
						_uowSciex.CommandStackSciex.Save();
					}
				}


				objContro.StatusExecucao = 1;

				//}
				//else
				//{
				//	objContro.StatusExecucao = 2;
				//}

				FinalizarProcessamento(estruturaPropriaPli, objContro, situacao2, situacao3);
			}
			return "Serviço executado.";
			//}
			//catch (Exception ex)
			//{
			//	return ex.Message.ToString();
			//}
		}

		private void FinalizarProcessamento(EstruturaPropriaPliEntity estruturaPropriaPli, ControleExecucaoServicoEntity objContro, List<SolicitacaoLeInsumoEntity> situacao2, List<SolicitacaoLeInsumoEntity> situacao3)
		{
			estruturaPropriaPli.DataFimProcessamento = GetDateTimeNowUtc();
			estruturaPropriaPli.StatusProcessamentoArquivo = 4;
			estruturaPropriaPli.QuantidadePLIProcessadoSucesso = Convert.ToInt16(situacao2.Count());
			estruturaPropriaPli.QuantidadePLIProcessadoFalha = Convert.ToInt16(situacao3.Count());
			_uowSciex.CommandStackSciex.DetachEntries();
			_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(estruturaPropriaPli);
			_uowSciex.CommandStackSciex.Save();

			objContro.DataHoraExecucaoFim = GetDateTimeNowUtc();
			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(objContro);
			_uowSciex.CommandStackSciex.Save();
		}

		private string BuscarDescricaoEmListaPadrao(SolicitacaoLeInsumoEntity dadosInsumo, int codigoProdutoSuframa)
		{
			string descricao = null;

			descricao = _uowSciex.QueryStackSciex.ViewInsumoPadrao.Selecionar(o => o.CodigoNCMMercadoria == dadosInsumo.CodigoNCM
																					&& o.CodigoDetalheMercadoria == dadosInsumo.CodigoDestaque
																					&& o.CodigoProduto == codigoProdutoSuframa)?.DescricaoDetalheMercadoria
																					?? null;

			return descricao;
		}

		public IEnumerable<SolicitacaoPliVM> Listar(SolicitacaoPliVM solicitacaoPliVM)
		{
			var solicitacaopli = _uowSciex.QueryStackSciex.SolicitacaoPli.Listar<SolicitacaoPliVM>();
			return AutoMapper.Mapper.Map<IEnumerable<SolicitacaoPliVM>>(solicitacaopli);
		}

		public SolicitacaoPliVM SalvarDoArquivo(SolicitacaoPliVM solicitacaoPliVM)
		{
			var solicitacaoPliEntity = AutoMapper.Mapper.Map<SolicitacaoPliEntity>(solicitacaoPliVM);
			_uowSciex.CommandStackSciex.SolicitacaoPli.Salvar(solicitacaoPliEntity);

			solicitacaoPliVM = AutoMapper.Mapper.Map<SolicitacaoPliVM>(solicitacaoPliEntity);
			return solicitacaoPliVM;
		}

		public PagedItems<SolicitacaoPliVM> ListarPaginado(SolicitacaoPliVM pagedFilter)
		{
			if (pagedFilter.Sort != null && pagedFilter.Sort.ToUpper() == "QTDERROSPLI")
			{
				pagedFilter.Sort = "";

				try
				{
					if (pagedFilter == null) { return new PagedItems<SolicitacaoPliVM>(); }
					var aladi = _uowSciex.QueryStackSciex.SolicitacaoPli.ListarPaginado<SolicitacaoPliVM>(o =>
						(
							(
								pagedFilter.IdSolicitacaoPli == -1 || o.IdSolicitacaoPli == pagedFilter.IdSolicitacaoPli
							)
							&&
							(
								pagedFilter.IdEstruturaPropriaPli == -1 || o.IdEstruturaPropriaPli == pagedFilter.IdEstruturaPropriaPli
							)
							&&
							(
								pagedFilter.StatusSolicitacao == 0 || o.StatusSolicitacao == pagedFilter.StatusSolicitacao
							)
						),
						pagedFilter);

					//foreach (SolicitacaoPliVM item in aladi.Items)
					//{
					//	try
					//	{
					//		PliEntity obj = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.NumeroPliImportador == item.NumeroPliImportador);

					//		item.NumeroPliSuframa =
					//			obj.Ano.ToString() + "/" + obj.NumeroPli.ToString("D6");
					//	}
					//	catch { }
					//}

					aladi.Items = aladi.Items.OrderBy(o => o.QtdErrosPli).ToList();

					return aladi;
				}
				catch (Exception ex)
				{
					//ChamaErro("Sistema Aladi: Nenhum registro encontrado.");

				}
				return new PagedItems<SolicitacaoPliVM>();
			}
			else
			{
				try
				{
					//var a = Convert.ToInt32("1") / Convert.ToInt32("0");
					if (pagedFilter == null) { return new PagedItems<SolicitacaoPliVM>(); }
					var aladi = _uowSciex.QueryStackSciex.SolicitacaoPli.ListarPaginado<SolicitacaoPliVM>(o =>
						(
							(
								pagedFilter.IdSolicitacaoPli == -1 || o.IdSolicitacaoPli == pagedFilter.IdSolicitacaoPli
							)
							&&
							(
								pagedFilter.IdEstruturaPropriaPli == -1 || o.IdEstruturaPropriaPli == pagedFilter.IdEstruturaPropriaPli
							)
							&&
							(
								pagedFilter.StatusSolicitacao == 0 || o.StatusSolicitacao == pagedFilter.StatusSolicitacao
							)
						),
						pagedFilter);

					return aladi;

				}

				catch (Exception ex)
				{

					//ChamaErro("Sistema Aladi: Nenhum registro encontrado.");

				}
			}

			return new PagedItems<SolicitacaoPliVM>();

		}
	}


}


