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
using Suframa.Sciex.CrossCutting.Mensagens;
using Suframa.Sciex.CrossCutting.Compressor;
using System.IO;

namespace Suframa.Sciex.BusinessLogic
{
	public class ProcessoProdutoBll : IProcessoProdutoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IAnaliseInsumoImportadoBll _analiseInsumoImportadoBll;

		public ProcessoProdutoBll(
			IUnitOfWorkSciex uowSciex, 
			IUnitOfWork uowCadsuf,
			IUsuarioPssBll usuarioPssBll, 
			IUsuarioInformacoesBll usuarioInformacoesBll,
			IAnaliseInsumoImportadoBll analiseInsumoImportadoBll
			)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_usuarioPssBll = usuarioPssBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
			_analiseInsumoImportadoBll = analiseInsumoImportadoBll;
		}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}

		public PagedItems<PlanoExportacaoVM> ListarPaginado(ConsultarPlanoExportacaoVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);
			//var usuCpfCnpjEmpresaOuLogado = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj;
			var usuCpfCnpjEmpresaOuLogado = "04817052000106";

			if (pagedFilter == null) { return new PagedItems<PlanoExportacaoVM>(); }

			var retornoConsulta = new PagedItems<PlanoExportacaoVM>();

			long digitoPlano = 0;
			int anoPlano = 0;

			if (!string.IsNullOrEmpty(pagedFilter.NumeroPlano))
			{
				digitoPlano = Convert.ToInt64(pagedFilter.NumeroPlano.Substring(0, 5));
				anoPlano = Convert.ToInt32(pagedFilter.NumeroPlano.Substring(6,4));
			}

			
			retornoConsulta = _uowSciex.QueryStackSciex.PlanoExportacao.ListarPaginado<PlanoExportacaoVM>(o =>
			(
				(digitoPlano == 0 || o.NumeroPlano == digitoPlano && o.AnoPlano == anoPlano)
				&&
				(pagedFilter.StatusPlano == null || o.Situacao == pagedFilter.StatusPlano)
				&&
				(
					(pagedFilter.DataInicio == null) || (dataInicio <= o.DataCadastro && o.DataCadastro <= dataFim)
				)
				&&
				(
					(o.Cnpj == usuCpfCnpjEmpresaOuLogado)
				)
			),
			pagedFilter);

			foreach (var registro in retornoConsulta.Items)
			{
				registro.DataCadastroFormatada = registro.DataCadastro == DateTime.MinValue ? DateTime.MinValue.ToShortDateString() : ((DateTime)registro.DataCadastro).ToShortDateString();
				registro.NumeroAnoPlanoFormatado = registro.NumeroPlano + "/" + registro.AnoPlano;
				registro.SituacaoString = registro.Situacao == 1 ? "EM ELABORAÇÃO"
												: registro.Situacao == 2 ? "ENTREGUE"
												: registro.Situacao == 3 ? "EM ANÁLISE"
												: "-";

				registro.TipoExportacaoString = registro.TipoExportacao == "AP" ? "APROVAÇÃO"
													: registro.TipoExportacao == "CO" ? "COMPROVAÇÃO"
													: "-";

				registro.TipoModalidadeString = registro.TipoModalidade == "S" ? "SUSPENSÃO"
													: "-";
			}

			return retornoConsulta;

		}

		enum enumTipoSolicitacao
		{
			INCLUSÃO_DE_INSUMO =1,
			TRANSFERENCIA_DE_SALDO_ENTRE_INSUMOS = 2,
			MOEDA = 4,
			QUANTIDADE = 5, 
			VALOR_UNITARIO = 6,
			VALOR_DO_FRETE = 7
		}
		public PRCSolicitacaoAlteracaoVM Validar(PRCSolicitacaoAlteracaoVM vm)
		{
			int StatusSolic_EmAnalise = 1;
			int StatusInsumo_Ativo = 1;
			int StatusInsumo_EmAlteracao = 2;
			int StatusInsumo_AlteracaoAprovada = 3;
			int StatusInsumo_Inativo = 4;
			var listaStatusAnalise = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Listar(q => q.IdSolicitacaoAlteracao == vm.IdSolicitacao && q.Status == StatusSolic_EmAnalise);

			try
			{
				if (listaStatusAnalise.Count == 0)
				{
					var listaStatusAprovado = _uowSciex.QueryStackSciex.PRCInsumo.Listar(q => q.IdPrcSolicitacaoAlteracao == vm.IdSolicitacao
																								&&
																								q.StatusInsumo == StatusInsumo_AlteracaoAprovada);
					if (listaStatusAprovado.Count > 0)
					{
						//var regInsumo = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(q => q.IdPrcSolicitacaoAlteracao == vm.IdSolicitacao);
						//var listaSolicDetalhe = regInsumo.PRCSolicDetalhe.Where(q => q.IdSolicitacaoAlteracao == vm.IdSolicitacao).ToList();

						foreach (var regInsumo in listaStatusAprovado)
						{
							//if (listaSolicDetalhe.Count > 0)
							if (regInsumo.PRCSolicDetalhe.Count > 0)
							{
								bool statusInsumoJaAlterado = false;

								foreach (var regSolicDetalhe in regInsumo.PRCSolicDetalhe)
								{

									int? _codigoInsumo;
									int? _idProduto;
									PRCInsumoEntity _regInsumoAtivo;
									PRCInsumoEntity _regInsumoAlteracaoAprovada;
									switch (regSolicDetalhe.IdTipoSolicitacao)
									{
										case (int)enumTipoSolicitacao.MOEDA:
										case (int)enumTipoSolicitacao.QUANTIDADE:
										case (int)enumTipoSolicitacao.VALOR_DO_FRETE:
										case (int)enumTipoSolicitacao.VALOR_UNITARIO:

											_analiseInsumoImportadoBll.CalcularNovosValoresPorTipo(regSolicDetalhe);


											_codigoInsumo = regSolicDetalhe.PrcInsumo.CodigoInsumo;
											_idProduto = regSolicDetalhe.PrcInsumo.IdPrcProduto;

											if (!statusInsumoJaAlterado)
											{
												_regInsumoAtivo = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(q => q.CodigoInsumo == _codigoInsumo
																													&&
																													q.IdPrcProduto == _idProduto
																													&&
																													q.StatusInsumo == StatusInsumo_Ativo);
											
												_regInsumoAtivo.StatusInsumo = StatusInsumo_Inativo;
												_uowSciex.CommandStackSciex.PRCInsumo.Salvar(_regInsumoAtivo);
												statusInsumoJaAlterado = true;
											}
											break;

										case (int)enumTipoSolicitacao.INCLUSÃO_DE_INSUMO:
											regSolicDetalhe.PrcInsumo.StatusInsumo = StatusInsumo_Ativo;
											break;

										case (int)enumTipoSolicitacao.TRANSFERENCIA_DE_SALDO_ENTRE_INSUMOS:

											_codigoInsumo = Convert.ToInt32(regSolicDetalhe.DescricaoDe);
											_idProduto = regSolicDetalhe.PrcInsumo.IdPrcProduto;
											_regInsumoAtivo = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(q => q.CodigoInsumo == _codigoInsumo
																													&&
																													q.IdPrcProduto == _idProduto
																													&&
																													q.StatusInsumo == StatusInsumo_Ativo);

											if (_regInsumoAtivo.QuantidadeSaldo != 0 && _regInsumoAtivo.ValorDolarSaldo != 0)
											{
												_regInsumoAtivo.QuantidadeSaldo = 0;
												_regInsumoAtivo.ValorDolarSaldo = 0;

												_uowSciex.CommandStackSciex.PRCInsumo.Salvar(_regInsumoAtivo); 
											}

											regSolicDetalhe.PrcInsumo.StatusInsumo = StatusInsumo_Ativo;
											
											break;
									}
								}

							}

							_uowSciex.CommandStackSciex.PRCInsumo.Salvar(regInsumo);
						}

						_uowSciex.CommandStackSciex.Save();
					}
					_uowSciex.CommandStackSciex.DetachEntries();

					var listaInsumosStatusAlteracao = _uowSciex.QueryStackSciex.PRCInsumo.Listar(q => q.IdPrcSolicitacaoAlteracao == vm.IdSolicitacao
																								&&
																								q.StatusInsumo == StatusInsumo_EmAlteracao);
					if (listaInsumosStatusAlteracao.Count > 0)
					{
						foreach (var regInsumo in listaInsumosStatusAlteracao)
						{
							regInsumo.StatusInsumo = StatusInsumo_Inativo;
							_uowSciex.CommandStackSciex.PRCInsumo.Salvar(regInsumo);
						}

						_uowSciex.CommandStackSciex.Save();
					}

					_uowSciex.CommandStackSciex.DetachEntries();
					PRCStatusEntity prcStatusEntity = new PRCStatusEntity();

					prcStatusEntity.Tipo = "AL";
					prcStatusEntity.Data = GetDateTimeNowUtc();

					var usuarioLogado = _usuarioPssBll.PossuiUsuarioLogado();

					if (usuarioLogado != null)
					{
						prcStatusEntity.CpfResponsavel = usuarioLogado.usuarioLogadoCpfCnpj.CnpjCpfUnformat();
						prcStatusEntity.NomeResponsavel = usuarioLogado.usuarioLogadoNome;
					}
					else
					{
						Exception n;
					}
					prcStatusEntity.IdSolicitacaoAlteracao = vm.IdSolicitacao;

					var regSolicAlteracao = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(q => q.Id == vm.IdSolicitacao);
					regSolicAlteracao.Status = 4;
					_uowSciex.CommandStackSciex.PRCSolicitacaoAlteracao.Salvar(regSolicAlteracao);
					_uowSciex.CommandStackSciex.Save();

					prcStatusEntity.IdProcesso = (int)regSolicAlteracao.IdProcesso;

					//Salvando na Entity PRCStatus
					_uowSciex.CommandStackSciex.PRCStatus.Salvar(prcStatusEntity);
					_uowSciex.CommandStackSciex.Save();



					_uowSciex.QueryStackSciex.IniciarStoreProcedureGerarHistoricoInsumo(prcStatusEntity.IdProcesso, vm.IdSolicitacao, usuarioLogado.usuarioLogadoNome);
					_uowSciex.QueryStackSciex.IniciarStoreProcedureParecerSuspensaoAlterado(prcStatusEntity.IdProcesso, vm.IdSolicitacao);

					try
					{
						vm.dadosParecer = _uowSciex.QueryStackSciex.ParecerTecnico.ListarGrafo(q => new ParecerTecnicoVM()
						{
							IdProcesso = q.IdProcesso,
							NumeroControle = q.NumeroControle,
							AnoControle = q.AnoControle,
							TipoStatus = q.TipoStatus
						},
						q => q.IdProcesso == regSolicAlteracao.IdProcesso
						&&
						q.TipoStatus == "AL").LastOrDefault();


						if (vm.dadosParecer != null)
						{
							return vm;
						}
						else
						{
							vm.MensagemErro = $"Não foi localizado parecer técnico. IdParecer: {vm.IdProcesso}";
							return vm;
						}


					}
					catch (Exception e)
					{
						vm.MensagemErro = "Erro ao consultar dados de parecer.";
						return vm;
					}

				}
				else
				{
					vm.MensagemErro = "Existe análises não concluidas.";
					return vm;
				}
			}
			catch (Exception n)
			{
				vm.MensagemErro = $"FALHA DE PROCESSAMENTO: {n.Message} / {n.StackTrace}";
				return vm;
			
			}

		}


		public PRCProdutoVM Selecionar(int idProduto)
		{

			var prod = _uowSciex.QueryStackSciex.PRCProduto.SelecionarGrafo(q => new PRCProdutoVM()
			{
				IdProduto = q.IdProduto,
				IdProcesso = q.IdProcesso,
				CodigoProdutoExportacao = q.CodigoProdutoExportacao,
				CodigoProdutoSuframa = q.CodigoProdutoSuframa,
				CodigoNCM = q.CodigoNCM,
				TipoProduto = q.TipoProduto,
				DescricaoModelo = q.DescricaoModelo,
				QuantidadeAprovado = q.QuantidadeAprovado,
				CodigoUnidade = q.CodigoUnidade,
				ValorDolarAprovado = q.ValorDolarAprovado,
				ValorFluxoCaixa = q.ValorFluxoCaixa,
				QuantidadeComprovado = q.QuantidadeComprovado,
				ValorDolarComprovado = q.ValorDolarComprovado,
				ValorNacionalComprovado = q.ValorNacionalComprovado,
				Processo = new ProcessoExportacaoVM()
				{
					IdProcesso = q.Processo.IdProcesso,
					NumeroProcesso = q.Processo.NumeroProcesso,
					AnoProcesso = q.Processo.AnoProcesso,
					InscricaoSuframa = q.Processo.InscricaoSuframa,
					RazaoSocial = q.Processo.RazaoSocial,
					TipoModalidade = q.Processo.TipoModalidade,
					TipoStatus = q.Processo.TipoStatus,
					DataValidade = q.Processo.DataValidade,
					ValorPremio = q.Processo.ValorPremio,
					ValorPercentualIndImportado = q.Processo.ValorPercentualIndImportado,
					ValorPercentualIndNacional = q.Processo.ValorPercentualIndNacional,
					Cnpj = q.Processo.Cnpj,
					ListaStatus = q.Processo.ListaStatus.Select(w => new PRCStatusVM()
					{
						IdStatus = w.IdStatus,
						IdProcesso = w.IdProcesso,
						Tipo = w.Tipo,
						Data = w.Data,
						DataValidade = w.DataValidade,
						CpfResponsavel = w.CpfResponsavel,
						NomeResponsavel = w.NomeResponsavel,
						NumeroPlano = w.NumeroPlano,
						AnoPlano = w.AnoPlano
					}
					).ToList(),
				},
				ListaInsumos = q.ListaInsumos.Select(w => new PRCInsumoVM()
				{
					IdInsumo = w.IdInsumo,
					IdPrcProduto = w.IdPrcProduto,
					CodigoInsumo = w.CodigoInsumo,
					CodigoUnidade = w.CodigoUnidade,
					TipoInsumo = w.TipoInsumo,
					CodigoNCM = w.CodigoNCM,
					ValorPercentualPerda = w.ValorPercentualPerda,
					CodigoDetalhe = w.CodigoDetalhe,
					DescricaoPartNumber = w.DescricaoPartNumber,
					DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
					ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
					ValorDolarAprovado = w.ValorDolarAprovado,
					QuantidadeAprovado = w.QuantidadeAprovado,
					ValorNacionalAprovado = w.ValorNacionalAprovado,
					ValorDolarFOBAprovado = w.ValorDolarFOBAprovado,
					ValorDolarCFRAprovado = w.ValorDolarCFRAprovado,
					ValorFreteAprovado = w.ValorFreteAprovado,
					ValorDolarComp = w.ValorDolarComp,
					QuantidadeComp = w.QuantidadeComp,
					ValorDolarSaldo = w.ValorDolarSaldo,
					QuantidadeSaldo = w.QuantidadeSaldo,
				}).ToList()
			}
			,
			o => o.IdProduto == idProduto);
	
			var codTipoProdSuf = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Listar(o => o.CodigoProduto == prod.CodigoProdutoSuframa 
																		&& o.CodigoTipoProduto == prod.TipoProduto 
																		&& o.CodigoNCM == prod.CodigoNCM).FirstOrDefault();
			prod.DescCodigoTipoProduto = codTipoProdSuf != null ? codTipoProdSuf.CodigoTipoProduto.ToString("D3") + " | " + codTipoProdSuf.DescricaoTipoProduto : "-";
			prod.DescCodigoProdutoSuframa = codTipoProdSuf.CodigoProduto.ToString("D4") + " | " + codTipoProdSuf.DescricaoProduto;
			var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == prod.CodigoUnidade);
			prod.DescCodigoUnidade = undMed != null ? undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao : "-";
			prod.DescricaoNCM = codTipoProdSuf.DescricaoNCM;


			//TODO 
			prod.Processo.NumeroAnoProcessoFormatado = Convert.ToInt32(prod.Processo.NumeroProcesso).ToString("D4") + "/" + prod.Processo.AnoProcesso;

			var ultimoStatus = prod.Processo.ListaStatus.Where(q=> q.Tipo == "AP").LastOrDefault();

			prod.Processo.NumeroAnoPlanoFormatado = ultimoStatus != null ? Convert.ToInt32(ultimoStatus.NumeroPlano).ToString("D5") + "/" + ultimoStatus.AnoPlano : "-";

			prod.Processo.DataValidadeFormatada = prod.Processo.DataValidade == DateTime.MinValue ? DateTime.MinValue.ToShortDateString() : ((DateTime)prod.Processo.DataValidade).ToShortDateString();

			prod.Processo.TipoModalidadeString = prod.Processo.TipoModalidade == "S" ? "SUSPENSÃO"
																		: prod.Processo.TipoModalidade == "I" ? "ISENÇÃO"
																		: "-"
																		;

			prod.Processo.TipoStatusString =	prod.Processo.TipoStatus.Equals("AP") ? "APROVADO": 
												prod.Processo.TipoStatus.Equals("CO") ? "COMPROVADO" : "-";
			var pagedFilter = new PagedOptions();
			prod.ListaProdutoPaisPaginada = _uowSciex.QueryStackSciex.PRCProdutoPais.ListarPaginadoGrafo(w => new PRCProdutoPaisVM()
			{
				IdProdutoPais = w.IdProdutoPais,
				IdPrcProduto = w.IdPrcProduto,
				QuantidadeAprovado = w.QuantidadeAprovado,
				ValorDolarAprovado = w.ValorDolarAprovado,
				CodigoPais = w.CodigoPais,
				ValorDolarComprovado = w.ValorDolarComprovado,
				QuantidadeComprovado = w.QuantidadeComprovado
			},
			q=> q.IdPrcProduto == idProduto
			,
			pagedFilter);

			foreach (var item in prod.ListaProdutoPaisPaginada.Items)
			{
				string codigoPais = Convert.ToInt32(item.CodigoPais).ToString("D3");
				var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

				item.DescricaoPais = pais.Descricao;
			}


			prod.ExisteSolicAlteracaoEmAnalise = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Existe(q =>
																	q.IdProcesso == prod.Processo.IdProcesso
																	&&
																	q.Status == 3
																	);



			return prod;
		}
		public PRCProdutoVM SelecionarProdutoEmAnalisePorIdProcesso(int idProcesso)
		{
			int EmAnalise = 3;
			var listaProdutosEmAnalise = _uowSciex.QueryStackSciex.PRCProduto.ListarGrafo(q => new PRCProdutoVM()
			{
				IdProduto = q.IdProduto,
				IdProcesso = q.IdProcesso,
				CodigoProdutoExportacao = q.CodigoProdutoExportacao,
				CodigoProdutoSuframa = q.CodigoProdutoSuframa,
				CodigoNCM = q.CodigoNCM,
				TipoProduto = q.TipoProduto,
				DescricaoModelo = q.DescricaoModelo,
				QuantidadeAprovado = q.QuantidadeAprovado,
				CodigoUnidade = q.CodigoUnidade,
				ValorDolarAprovado = q.ValorDolarAprovado,
				ValorFluxoCaixa = q.ValorFluxoCaixa,
				Processo = new ProcessoExportacaoVM()
				{
					IdProcesso = q.Processo.IdProcesso,
					NumeroProcesso = q.Processo.NumeroProcesso,
					AnoProcesso = q.Processo.AnoProcesso,
					InscricaoSuframa = q.Processo.InscricaoSuframa,
					RazaoSocial = q.Processo.RazaoSocial,
					TipoModalidade = q.Processo.TipoModalidade,
					TipoStatus = q.Processo.TipoStatus,
					DataValidade = q.Processo.DataValidade,
					ValorPremio = q.Processo.ValorPremio,
					ValorPercentualIndImportado = q.Processo.ValorPercentualIndImportado,
					ValorPercentualIndNacional = q.Processo.ValorPercentualIndNacional,
					Cnpj = q.Processo.Cnpj,
					ListaStatus = q.Processo.ListaStatus.Select(w => new PRCStatusVM()
					{
						IdStatus = w.IdStatus,
						IdProcesso = w.IdProcesso,
						Tipo = w.Tipo,
						Data = w.Data,
						DataValidade = w.DataValidade,
						CpfResponsavel = w.CpfResponsavel,
						NomeResponsavel = w.NomeResponsavel,
						NumeroPlano = w.NumeroPlano,
						AnoPlano = w.AnoPlano
					}
					).ToList(),
				},
				ListaInsumos = q.ListaInsumos.Select(w => new PRCInsumoVM()
				{
					IdInsumo = w.IdInsumo,
					IdPrcProduto = w.IdPrcProduto,
					CodigoInsumo = w.CodigoInsumo,
					CodigoUnidade = w.CodigoUnidade,
					TipoInsumo = w.TipoInsumo,
					CodigoNCM = w.CodigoNCM,
					ValorPercentualPerda = w.ValorPercentualPerda,
					CodigoDetalhe = w.CodigoDetalhe,
					DescricaoPartNumber = w.DescricaoPartNumber,
					DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
					ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
					ValorDolarAprovado = w.ValorDolarAprovado,
					QuantidadeAprovado = w.QuantidadeAprovado,
					ValorNacionalAprovado = w.ValorNacionalAprovado,
					ValorDolarFOBAprovado = w.ValorDolarFOBAprovado,
					ValorDolarCFRAprovado = w.ValorDolarCFRAprovado,
					ValorFreteAprovado = w.ValorFreteAprovado,
					ValorDolarComp = w.ValorDolarComp,
					QuantidadeComp = w.QuantidadeComp,
					ValorDolarSaldo = w.ValorDolarSaldo,
					QuantidadeSaldo = w.QuantidadeSaldo,
					//PrcSolicitacaoAlteracao = new PRCSolicitacaoAlteracaoVM()
					//{
					//	Id = w.PrcSolicitacaoAlteracao.Id,
					//	IdProcesso = w.PrcSolicitacaoAlteracao.IdProcesso,
					//	Status = w.PrcSolicitacaoAlteracao.Status,
					//	//ListaSolicDetalhe = w.PRCSolicDetalhe.Select(o => new PRCSolicDetalheVM()
					//	//{
					//	//	Id = o.Id,
					//	//	IdInsumo = w.IdInsumo,
					//	//	IdSolicitacaoAlteracao = w.IdPrcSolicitacaoAlteracao,
					//	//	IdTipoSolicitacao = o.IdTipoSolicitacao,
					//	//	Status = o.Status,
					//	//	TipoSolicAlteracao = new TipoSolicAlteracaoVM()
					//	//	{
					//	//		Id = o.TipoSolicAlteracao.Id,
					//	//		Descricao = o.TipoSolicAlteracao.Descricao
					//	//	}
					//	//}).ToList()
					//}
				}).ToList()
			}
			,
			o => 
			o.IdProcesso == idProcesso
			);

			var prod = listaProdutosEmAnalise.FirstOrDefault();

			//var codProdSuf = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Listar(o => o.CodigoProduto == prod.CodigoProdutoSuframa).FirstOrDefault();
			//prod.DescCodigoProdutoSuframa = codProdSuf.CodigoProduto.ToString("D4") + " | " + codProdSuf.DescricaoProduto;	
			var codTipoProdSuf = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Listar(o => o.CodigoProduto == prod.CodigoProdutoSuframa
																		&& o.CodigoTipoProduto == prod.TipoProduto
																		&& o.CodigoNCM == prod.CodigoNCM).FirstOrDefault();
			prod.DescCodigoTipoProduto = codTipoProdSuf != null ? codTipoProdSuf.CodigoTipoProduto.ToString("D3") + " | " + codTipoProdSuf.DescricaoTipoProduto : "-";
			prod.DescCodigoProdutoSuframa = codTipoProdSuf.CodigoProduto.ToString("D4") + " | " + codTipoProdSuf.DescricaoProduto;
			var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == prod.CodigoUnidade);
			prod.DescCodigoUnidade = undMed != null ? undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao : "-";
			prod.DescricaoNCM = codTipoProdSuf.DescricaoNCM;


			//TODO 
			prod.Processo.NumeroAnoProcessoFormatado = Convert.ToInt32(prod.Processo.NumeroProcesso).ToString("D4") + "/" + prod.Processo.AnoProcesso;

			var ultimoStatus = prod.Processo.ListaStatus.Where(q => q.Tipo == "AP").LastOrDefault();

			prod.Processo.NumeroAnoPlanoFormatado = ultimoStatus != null ? Convert.ToInt32(ultimoStatus.NumeroPlano).ToString("D5") + "/" + ultimoStatus.AnoPlano : "-";

			prod.Processo.DataValidadeFormatada = prod.Processo.DataValidade == DateTime.MinValue ? DateTime.MinValue.ToShortDateString() : ((DateTime)prod.Processo.DataValidade).ToShortDateString();

			prod.Processo.TipoModalidadeString = prod.Processo.TipoModalidade == "S" ? "SUSPENSÃO"
																		: prod.Processo.TipoModalidade == "I" ? "ISENÇÃO"
																		: "-"
																		;

			prod.Processo.TipoStatusString = prod.Processo.TipoStatus.Equals("AP") ? "APROVADO"
																			: "-";
			var pagedFilter = new PagedOptions();
			prod.ListaProdutoPaisPaginada = _uowSciex.QueryStackSciex.PRCProdutoPais.ListarPaginadoGrafo(w => new PRCProdutoPaisVM()
			{
				IdProdutoPais = w.IdProdutoPais,
				IdPrcProduto = w.IdPrcProduto,
				QuantidadeAprovado = w.QuantidadeAprovado,
				ValorDolarAprovado = w.ValorDolarAprovado,
				CodigoPais = w.CodigoPais
			},
			q => q.IdPrcProduto == prod.IdProduto
			,
			pagedFilter);

			foreach (var item in prod.ListaProdutoPaisPaginada.Items)
			{
				string codigoPais = Convert.ToInt32(item.CodigoPais).ToString("D3");
				var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

				item.DescricaoPais = pais.Descricao;
			}


			prod.ExisteSolicAlteracaoEmAnalise = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Existe(q =>
																	q.IdProcesso == prod.Processo.IdProcesso
																	&&
																	q.Status == 3
																	);



			return prod;
		}

		public PEProdutoVM Salvar(PEProdutoVM vm)
		{
			if (vm == null) { return null; }

			var peProduto = new PEProdutoEntity();
			if (vm.IdPEProduto > 0)
			{
				peProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(x => x.IdPEProduto == vm.IdPEProduto);

				peProduto = AutoMapper.Mapper.Map(vm, peProduto);
				vm.MensagemErro = "";
			}
			else
			{
				var objLEProduto = _uowSciex.QueryStackSciex.LEProduto.Selecionar(o => o.IdLe == vm.IdLEProduto);

				peProduto.CodigoProdutoExportacao = objLEProduto.CodigoProduto;
				peProduto.CodigoProdutoSuframa = objLEProduto.CodigoProdutoSuframa;
				peProduto.CodigoNCM = objLEProduto.CodigoNCM;
				peProduto.CodigoTipoProduto = objLEProduto.CodigoTipoProduto;
				peProduto.DescricaoModelo = objLEProduto.DescricaoModelo;
				peProduto.CodigoUnidade = objLEProduto.CodigoUnidadeMedida;
				peProduto.IdPlanoExportacao = vm.IdPlanoExportacao;

				var peProdutoValida = _uowSciex.QueryStackSciex.PlanoExportacaoProduto
					.Listar(o => o.IdPlanoExportacao == peProduto.IdPlanoExportacao && o.CodigoProdutoExportacao == objLEProduto.CodigoProduto);

				vm.MensagemErro = "";
				if (peProdutoValida.Count != 0)
				{
					vm.MensagemErro = "O produto selecionado já foi cadastrado.";
					return vm;
				}
				_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(peProduto, true);
				_uowSciex.CommandStackSciex.Save();
			}

			return vm;
		}

		public void Deletar(int id)
		{
			var leProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(s => s.IdPEProduto == id);

			if (leProduto != null)
			{
				var pePaises = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == leProduto.IdPEProduto);

				foreach (var item in pePaises)
				{
					_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Apagar(item.IdPEProdutoPais);
					_uowSciex.CommandStackSciex.Save();
				}

				_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Apagar(leProduto.IdPEProduto);
				_uowSciex.CommandStackSciex.Save();
			}
		}

	}
}