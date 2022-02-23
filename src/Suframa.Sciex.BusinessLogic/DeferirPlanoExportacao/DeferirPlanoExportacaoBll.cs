using Suframa.Sciex.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.BusinessLogic.DeferirPlanoExportacao;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;

namespace Suframa.Sciex.BusinessLogic
{
	public class DeferirPlanoExportacaoBll : IDeferirPlanoExportacaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public DeferirPlanoExportacaoBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioPssBll = usuarioPssBll;
		}

		public ResultadoProcessamentoVM DeferirPlano(PlanoExportacaoVM vm)
		{
			var resultadoExec = new ResultadoProcessamentoVM()
			{
				Resultado = true,
				PossuiTodosRegistros = true
			};

			int numeroNovoProcesso = 0;
			int IdNovoProcesso = 0;

			// Passos para o Deferimento do Plano:

			// 1 - Somente Planos que não possuem vinculos reprovados podem ser Deferidos
			// Validar o campo ST_ANALISE na Entidade : PEProdutoPaisVM
			// Relacionamento - PlanoExportacao => Produto => ProdutoPais
			// Carregar Plano de Exportação
			

			try
			{
				if (!vm.TipoExportacao.Equals("CO"))
				{
					var objPlanoExportacao = _uowSciex.QueryStackSciex.PlanoExportacao.SelecionarGrafo(o => new PlanoExportacaoVM()
					{
						IdPlanoExportacao = o.IdPlanoExportacao,
						NumeroPlano = o.NumeroPlano,
						AnoPlano = o.AnoPlano,
						NumeroInscricaoCadastral = o.NumeroInscricaoCadastral,
						Cnpj = o.Cnpj,
						RazaoSocial = o.RazaoSocial,
						TipoModalidade = o.TipoModalidade,
						TipoExportacao = o.TipoExportacao,
						Situacao = o.Situacao,
						DataEnvio = o.DataEnvio,
						DataCadastro = o.DataCadastro,
						DataStatus = o.DataStatus,
						CpfResponsavel = o.CpfResponsavel,
						NomeResponsavel = o.NomeResponsavel,
						ListaPEProdutos = o.ListaPEProdutos.Select(q => new PEProdutoVM()
						{
							IdPEProduto = q.IdPEProduto,
							IdPlanoExportacao = q.IdPlanoExportacao,
							CodigoProdutoExportacao = q.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.CodigoProdutoSuframa,
							CodigoNCM = q.CodigoNCM,
							CodigoTipoProduto = q.CodigoTipoProduto,
							DescricaoModelo = q.DescricaoModelo,
							Qtd = q.Qtd,
							ValorDolar = q.ValorDolar,
							ValorFluxoCaixa = q.ValorFluxoCaixa,
							CodigoUnidade = q.CodigoUnidade
						}).ToList()
					},
					o => o.IdPlanoExportacao == vm.IdPlanoExportacao);

					// Lista Produtos pelo Plano Exportacao
					foreach (var itemProdutos in objPlanoExportacao.ListaPEProdutos)
					{
						_uowSciex.CommandStackSciex.DetachEntries();

						var objProdutosPais = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(l => l.IdPEProduto == itemProdutos.IdPEProduto);

						//if (objProdutosPais.Count > 0)
						//{
						//validar se todos os itens produto país estão aprovados
						//int iQuantItensAprovados = objProdutosPais.Count(f => f.SituacaoAnalise.Equals((int)EnumSituacaoAnaliseProduto.APROVADO));

						//if (objProdutosPais.Count == iQuantItensAprovados)
						//{
						// Qtd Insumos Aprovados
						var objPEInsumo = _uowSciex.QueryStackSciex.PEInsumo.Listar(i => i.IdPEProduto == itemProdutos.IdPEProduto);

						int iQtPEInsumoAprovadosOuInativos = objPEInsumo.Count(f => f.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.APROVADO
																		||
																		f.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.INATIVO);

						int qtdPEInsumosCorrigidos = objPEInsumo.Count(f => f.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO);

						if (iQtPEInsumoAprovadosOuInativos == (objPEInsumo.Count - qtdPEInsumosCorrigidos))
						{
							objPEInsumo = objPEInsumo.Where(f => f.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.APROVADO
																		||
																		f.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.INATIVO).ToList();

							//int iQtPEDInsumos = 0;
							//int iQtPEDInsumoAprovados = 0;

							bool existeDetalheInsumoReprovadoOuNulo = false;

							foreach (var itemInsumo in objPEInsumo)
							{
								// Qtd Detalhe Insumos Aprovados
								var objPEInsumoDetalhe = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(i => i.IdPEInsumo == itemInsumo.IdPEInsumo);

								ValidarParidadeCambial(ref resultadoExec, objPEInsumoDetalhe);

								if (resultadoExec.CamposNaoValidos != null)
								{
									return resultadoExec;
								}


								foreach (var detalhe in objPEInsumoDetalhe)
								{
									if (detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO
										||
										detalhe.SituacaoAnalise == null)
									{
										existeDetalheInsumoReprovadoOuNulo = true;
									}
								}

								//if (iQtPEDInsumoAprovados == 0)
								//{
								//	iQtPEDInsumos = objPEInsumoDetalhe.Count;

								//	iQtPEDInsumoAprovados = objPEInsumoDetalhe.Count(f => f.SituacaoAnalise.Equals((int)EnumSituacaoAnalisePlanoExportacao.APROVADO)
								//																||
								//																f.SituacaoAnalise.Equals((int)EnumSituacaoAnalisePlanoExportacao.INATIVO));
								//}
								//else
								//{
								//	iQtPEDInsumos = iQtPEDInsumos + objPEInsumoDetalhe.Count;
								//	iQtPEDInsumoAprovados = iQtPEDInsumoAprovados + objPEInsumoDetalhe.Count(f => f.SituacaoAnalise.Equals((int)EnumSituacaoAnalisePlanoExportacao.APROVADO)
								//																						||
								//																f.SituacaoAnalise.Equals((int)EnumSituacaoAnalisePlanoExportacao.INATIVO));
								//}
							}

							if (!existeDetalheInsumoReprovadoOuNulo)
							{
								// Deferir itens
								if (IdNovoProcesso == 0)
								{
									// 2 - Buscar proximo sequencial do numero do processo
									var ultimoRegistroNumSeq = _uowSciex.QueryStackSciex.Processo.
															Listar(l => l.AnoProcesso == DateTime.Now.Year);

									var numeroSeqProcesso = ultimoRegistroNumSeq.Count > 0 ? ultimoRegistroNumSeq.
															OrderBy(o => o.IdProcesso).Last().NumeroProcesso : 0;

									numeroNovoProcesso = int.Parse(numeroSeqProcesso.ToString()) + 1;

									// Entidades para inserir novos registros
									// 1 - PROCESSO
									#region PROCESSO

									// Valor premio dos produtos do plano selecionado
									var valorTotalPremio = objPlanoExportacao.ListaPEProdutos.Sum(s => s.ValorDolar);

									var processoVM = new ProcessoExportacaoVM()
									{
										NumeroProcesso = numeroNovoProcesso,
										AnoProcesso = DateTime.Now.Year,
										InscricaoSuframa = objPlanoExportacao.NumeroInscricaoCadastral,
										RazaoSocial = objPlanoExportacao.RazaoSocial,
										Cnpj = objPlanoExportacao.Cnpj.CnpjCpfUnformat(),
										TipoModalidade = objPlanoExportacao.TipoModalidade,
										TipoStatus = objPlanoExportacao.TipoExportacao,
										DataValidade = DateTime.Now.AddYears(1),
										ValorPremio = valorTotalPremio
									};
									this.InserirProcesso(processoVM);

									var anoCorrente = DateTime.Now.Year;
									var IdUltimoProcesso = _uowSciex.QueryStackSciex.Processo.Selecionar(p =>
																p.NumeroProcesso == numeroNovoProcesso && p.AnoProcesso == anoCorrente).IdProcesso;
									IdNovoProcesso = IdUltimoProcesso;

									#endregion
								}
								// 2 - PRCSTATUS
								#region PRCSTATUS

								var CPFUsuario = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj;
								var NomeUsuario = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome;

								var prcStatusVM = new PRCStatusVM()
								{
									Tipo = "AP",
									Data = DateTime.Now,
									DataValidade = DateTime.Now.AddYears(1),
									CpfResponsavel = CPFUsuario,
									NomeResponsavel = NomeUsuario,
									IdProcesso = IdNovoProcesso,
									NumeroPlano = int.Parse(objPlanoExportacao.NumeroPlano.ToString()),
									AnoPlano = objPlanoExportacao.AnoPlano
								};
								InserirPRCStatus(prcStatusVM);
								#endregion

								// 3 - PRCPRODUTO
								#region PRCPRODUTO
								this.InserirPRCProduto(itemProdutos, IdNovoProcesso);
								var IdUltimoPrcProduto = _uowSciex.QueryStackSciex.PRCProduto.Listar().OrderBy(o => o.IdProduto).Last().IdProduto;

								#endregion

								//// 4 - PRCPRODUTOPAIS
								#region PRCPRODUTOPAIS
								_uowSciex.CommandStackSciex.DetachEntries();
								this.InserirPRCProdutoPais(objProdutosPais, IdUltimoPrcProduto);
								#endregion

								// 5 - PRCINSUMO
								#region PRCINSUMO
								var listaPEInsumo = _uowSciex.QueryStackSciex.PEInsumo.Listar(
																l => l.IdPEProduto == itemProdutos.IdPEProduto
																&&
																l.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.APROVADO);

								_uowSciex.CommandStackSciex.DetachEntries();

								foreach (var itemInsumo in listaPEInsumo)
								{
									this.InserirPRCInsumo(itemInsumo, IdUltimoPrcProduto);

									var IdUltimpPRCInsumo = _uowSciex.QueryStackSciex.PRCInsumo.Listar().OrderBy(o => o.IdInsumo).Last().IdInsumo;

									// 6 - PRCDETALHEINSUMO
									#region PRCDETALHEINSUMO
									decimal? somatorioValorNacionalAprov = null;

									somatorioValorNacionalAprov = this.InserirPRCDetalheInsumo(itemInsumo.IdPEInsumo, IdUltimpPRCInsumo);

									if (itemInsumo.TipoInsumo.Equals("N") || itemInsumo.TipoInsumo.Equals("R"))
									{
										_uowSciex.CommandStackSciex.DetachEntries();

										var regUltimoPRCInsumo = _uowSciex.QueryStackSciex.PRCInsumo.Listar().OrderBy(o => o.IdInsumo).Last();

										regUltimoPRCInsumo.ValorNacionalAprovado = somatorioValorNacionalAprov;

										_uowSciex.CommandStackSciex.PRCInsumo.Salvar(regUltimoPRCInsumo);
										_uowSciex.CommandStackSciex.Save();
									}
									#endregion


								}
								#endregion
							}
							else
							{
								resultadoExec.PossuiTodosRegistros = false;
								return resultadoExec;
							}
						}
						else
						{
							resultadoExec.PossuiTodosRegistros = false;
							return resultadoExec;
						}
						//}
						//else
						//{
						//	resultadoExec.PossuiTodosRegistros = false;
						//	return resultadoExec;
						//}
						//}
						//else
						//{
						//	resultadoExec.PossuiTodosRegistros = false;
						//	return resultadoExec;
						//}
					}
					// 3 - Calcular valores Importados
					#region Calcular valores Importados
					// Entidades para atualizar
					// 1 - PRCDETALHEINSUMO
					// 2 - PRCINSUMO
					if (IdNovoProcesso > 0)
					{
						this.CalcularValoresImportados(IdNovoProcesso);
						#endregion

						// 3 - PLANO EXPORTACAO
						#region PLANO EXPORTACAO
						this.SalvarPlanoExportacao(objPlanoExportacao, numeroNovoProcesso);
						#endregion
					}

					var idProcesso = _uowSciex.QueryStackSciex.Processo.Listar(q => q.NumeroProcesso == numeroNovoProcesso).LastOrDefault().IdProcesso;

					_uowSciex.QueryStackSciex.IniciarStoreProcedureParecerTecnico(idProcesso);

					var regNovoParecer = _uowSciex.QueryStackSciex.ParecerTecnico.Listar(q => q.IdProcesso == idProcesso).LastOrDefault();

					resultadoExec.Mensagem = $"Foi gerado o parecer de nº {regNovoParecer.NumeroControle}/{regNovoParecer.AnoControle}";
				}
				else
				{
					var objPlanoExportacao = _uowSciex.QueryStackSciex.PlanoExportacao.SelecionarGrafo(o => new PlanoExportacaoVM()
					{
						IdPlanoExportacao = o.IdPlanoExportacao,
						NumeroPlano = o.NumeroPlano,
						AnoPlano = o.AnoPlano,
						NumeroProcesso = o.NumeroProcesso,
						NumeroAnoProcesso = o.NumeroAnoProcesso,
						NumeroInscricaoCadastral = o.NumeroInscricaoCadastral,
						Cnpj = o.Cnpj,
						RazaoSocial = o.RazaoSocial,
						TipoModalidade = o.TipoModalidade,
						TipoExportacao = o.TipoExportacao,
						Situacao = o.Situacao,
						DataEnvio = o.DataEnvio,
						DataCadastro = o.DataCadastro,
						DataStatus = o.DataStatus,
						CpfResponsavel = o.CpfResponsavel,
						NomeResponsavel = o.NomeResponsavel,
						ListaPEProdutosComplemento = o.ListaPEProdutos.Select(q => new PEProdutoComplementoVM()
						{
							IdPEProduto = q.IdPEProduto,
							IdPlanoExportacao = q.IdPlanoExportacao,
							CodigoProdutoExportacao = q.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.CodigoProdutoSuframa,
							CodigoNCM = q.CodigoNCM,
							CodigoTipoProduto = q.CodigoTipoProduto,
							DescricaoModelo = q.DescricaoModelo,
							Qtd = q.Qtd,
							ValorDolar = q.ValorDolar,
							ValorFluxoCaixa = q.ValorFluxoCaixa,
							CodigoUnidade = q.CodigoUnidade,
							ValorNacional = q.ValorNacional,
							ListaPEProdutoPais = q.ListaPEProdutoPais.Select(w => new PEProdutoPaisComplementoVM()
							{
								IdPEProdutoPais = w.IdPEProdutoPais,
								IdPEProduto = w.IdPEProduto,
								CodigoPais = w.CodigoPais,
								ListaPEDue = w.ListaPEDue.Select(e => new PlanoExportacaoDUEComplementoVM()
								{
									IdDue = e.IdDue,
									IdPEProdutoPais = e.IdPEProdutoPais,
									SituacaoAnalise = e.SituacaoAnalise,
									CodigoPais = e.CodigoPais
								}).ToList()
							}).ToList()
						}).ToList()
					},
					o => o.IdPlanoExportacao == vm.IdPlanoExportacao);

					bool existePendenciaAnalise = false;
					foreach (var Produto in objPlanoExportacao.ListaPEProdutosComplemento)
					{
						foreach (var ProdutoPais in Produto.ListaPEProdutoPais)
						{
							foreach (var Due in ProdutoPais.ListaPEDue)
							{
								if (Due.SituacaoAnalise != (int)EnumSituacaoAnaliseDUE.APROVADO
									&&
									Due.SituacaoAnalise != (int)EnumSituacaoAnaliseDUE.INATIVO)
								{
									existePendenciaAnalise = true;
								}
							}
						}
					}

					if (existePendenciaAnalise)
					{
						resultadoExec.PossuiTodosRegistros = false;
						return resultadoExec;
					}

					#region RN30-PROCESSO
					var regProcesso = _uowSciex.QueryStackSciex.Processo.Selecionar(q => q.NumeroProcesso == objPlanoExportacao.NumeroProcesso
																								&&
																								q.AnoProcesso == objPlanoExportacao.NumeroAnoProcesso);
					#endregion


					foreach (var registroPRCProduto in regProcesso.ListaProduto)
					{
						#region RN30 - PRODUTO
						var regPEProduto = objPlanoExportacao.ListaPEProdutosComplemento.Where(q => q.CodigoProdutoExportacao == registroPRCProduto.CodigoProdutoExportacao).FirstOrDefault();

						registroPRCProduto.QuantidadeComprovado = regPEProduto.Qtd;
						registroPRCProduto.ValorDolarComprovado = regPEProduto.ValorDolar;
						registroPRCProduto.ValorNacionalComprovado = regPEProduto.ValorNacional;
						#endregion


						var listaPRCProdutoPais = _uowSciex.QueryStackSciex.PRCProdutoPais.Listar(q => q.IdPrcProduto == registroPRCProduto.IdProduto);

						#region RN30 - PRODUTOPAIS, DUE e Status
						if (listaPRCProdutoPais.Count == 0)
						{

							if (regPEProduto.ListaPEProdutoPais.Count > 0)
							{
								foreach (var regPEPais in regPEProduto.ListaPEProdutoPais)
								{

									var novoPRCProdutoPais = new PRCProdutoPaisEntity()
									{
										IdPrcProduto = registroPRCProduto.IdProduto,
										QuantidadeComprovado = regPEPais.Quantidade,
										ValorDolarComprovado = regPEPais.ValorDolar,
										CodigoPais = regPEPais.CodigoPais
									};

									_uowSciex.CommandStackSciex.PRCProdutoPais.Salvar(novoPRCProdutoPais);

									var PEDue = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar<PlanoExportacaoDUEVM>(q =>
																												q.IdPEProdutoPais == regPEPais.IdPEProdutoPais
																												&&
																												q.CodigoPais == regPEPais.CodigoPais).FirstOrDefault();

									var novoPRCDue = new PRCDueEntity()
									{
										IdPRCProdutoPais = novoPRCProdutoPais.IdProdutoPais,
										Numero = PEDue.Numero,
										DataAverbacao = PEDue.DataAverbacao,
										ValorDolar = PEDue.ValorDolar,
										Quantidade = PEDue.Quantidade,
										CodigoPais = PEDue.CodigoPais,
									};

									_uowSciex.CommandStackSciex.PRCDue.Salvar(novoPRCDue);
									_uowSciex.CommandStackSciex.Save();

									RegistrarNovoStatus(objPlanoExportacao, regProcesso);
								}
							}

						}
						else
						{
							foreach (var regPRCProdutoPais in listaPRCProdutoPais)
							{
								var regPEPais = regPEProduto.ListaPEProdutoPais.Where(q => q.CodigoPais == regPRCProdutoPais.CodigoPais).FirstOrDefault();

								regPRCProdutoPais.QuantidadeComprovado = regPEPais.Quantidade;
								regPRCProdutoPais.ValorDolarComprovado = regPEPais.ValorDolar;

								_uowSciex.CommandStackSciex.PRCProdutoPais.Salvar(regPRCProdutoPais);


								var PEDue = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar<PlanoExportacaoDUEVM>(q =>
																												q.IdPEProdutoPais == regPEPais.IdPEProdutoPais
																												&&
																												q.CodigoPais == regPEPais.CodigoPais).FirstOrDefault();

								var regPRCDue = regPRCProdutoPais.PrcDue.Where(q => q.CodigoPais == regPEPais.CodigoPais).FirstOrDefault();


								regPRCDue.Numero = PEDue.Numero;
								regPRCDue.DataAverbacao = PEDue.DataAverbacao;
								regPRCDue.ValorDolar = PEDue.ValorDolar;
								regPRCDue.Quantidade = PEDue.Quantidade;


								_uowSciex.CommandStackSciex.PRCDue.Salvar(regPRCDue);
								_uowSciex.CommandStackSciex.Save();

								RegistrarNovoStatus(objPlanoExportacao, regProcesso);
							}
						}
						#endregion

						var listaInsumosParaCancelamento = registroPRCProduto.ListaInsumos.Where(q =>
																								(
																									q.QuantidadeSaldoCancelado != null
																									&&
																									q.ValorDolarSaldoCancelado != null
																								)
																								&&
																								(
																									q.QuantidadeSaldoCancelado
																									+
																									q.ValorDolarSaldoCancelado
																									!=
																									0
																								)).ToList();


						foreach (var Insumo in listaInsumosParaCancelamento)
						{
							Insumo.ValorDolarSaldo = 0;
							Insumo.QuantidadeSaldo = 0;

							_uowSciex.CommandStackSciex.PRCInsumo.Salvar(Insumo);

						}

						_uowSciex.CommandStackSciex.Save();

					};




					regProcesso.TipoStatus = "CO";

					_uowSciex.CommandStackSciex.Processo.Salvar(regProcesso);

					_uowSciex.CommandStackSciex.Save();


					_uowSciex.QueryStackSciex.IniciarStoreProcedureParecerTecnico(regProcesso.IdProcesso, true);

					var regNovoParecer = _uowSciex.QueryStackSciex.ParecerTecnico.Listar(q => q.IdProcesso == regProcesso.IdProcesso).LastOrDefault();

					if (regNovoParecer == null)
					{
						resultadoExec.Resultado = false;
						resultadoExec.Mensagem = $"Erro ao gerar parecer (ID Processo: {regProcesso.IdProcesso})";
					}
					else
					{
						resultadoExec.Mensagem = $"Foi gerado o parecer de nº {regNovoParecer.NumeroControle}/{regNovoParecer.AnoControle}";
					}

				}

			}
			catch (Exception ex)
			{
				resultadoExec.Resultado = false;
				resultadoExec.Mensagem = $"Mensagem: {ex.Message} / StackTrace: {ex.StackTrace}";
				return resultadoExec;
			}

			return resultadoExec;
		}

		private void RegistrarNovoStatus(PlanoExportacaoVM objPlanoExportacao, ProcessoEntity regProcesso)
		{
			// 2 - PRCSTATUS
			#region PRCSTATUS

			var CPFUsuario = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj;
			var NomeUsuario = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome;

			var prcStatusVM = new PRCStatusVM()
			{
				Tipo = "CO",
				Data = DateTime.Now,
				DataValidade = DateTime.Now.AddYears(1),
				CpfResponsavel = CPFUsuario,
				NomeResponsavel = NomeUsuario,
				IdProcesso = regProcesso.IdProcesso,
				NumeroPlano = int.Parse(objPlanoExportacao.NumeroPlano.ToString()),
				AnoPlano = objPlanoExportacao.AnoPlano
			};
			InserirPRCStatus(prcStatusVM);
			#endregion
		}

		private void ValidarParidadeCambial(ref ResultadoProcessamentoVM retorno, ICollection<PEDetalheInsumoEntity> listaPEDetalheInsumo)
		{
			var dataHoje = DateTime.Now.Date;
			foreach (var regDetalheInsumo in listaPEDetalheInsumo)
			{
				ParidadeValorEntity regParidadeCambial = null;
				ParidadeValorEntity regParidadeCambialEstrangeira = null;

				int codigoMoedaDolar = 220;
				if (regDetalheInsumo.IdMoeda != codigoMoedaDolar)
				{
					regParidadeCambial = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.CodigoMoeda == codigoMoedaDolar && q.ParidadeCambial.DataParidade == dataHoje);
					regParidadeCambialEstrangeira = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.IdMoeda == regDetalheInsumo.IdMoeda && q.ParidadeCambial.DataParidade == dataHoje);

					if (regParidadeCambial == null || regParidadeCambialEstrangeira == null)
					{
						if (regParidadeCambial == null)
						{
							if (retorno.CamposNaoValidos == null)
								retorno.CamposNaoValidos = new CamposNaoValidadosVM();

							retorno.CamposNaoValidos.NaoExisteParidadeCambial = true;
							break;
						}
						else
						{
							if (retorno.CamposNaoValidos == null)
								retorno.CamposNaoValidos = new CamposNaoValidadosVM();

							retorno.CamposNaoValidos.NaoExisteParidadeCambialEstrangeira = true;
							break;
						}


					}
				}
				else
				{
					regParidadeCambial = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.IdMoeda == codigoMoedaDolar && q.ParidadeCambial.DataParidade == dataHoje);

					if (regParidadeCambial == null)
					{
						if (retorno.CamposNaoValidos == null)
							retorno.CamposNaoValidos = new CamposNaoValidadosVM();

						retorno.CamposNaoValidos.NaoExisteParidadeCambial = true;
						break;
					}
				}


			}
		}

		private void InserirProcesso(ProcessoExportacaoVM obj)
		{
			var processoEntity = new ProcessoEntity()
			{
				NumeroProcesso = obj.NumeroProcesso,
				AnoProcesso = obj.AnoProcesso,
				InscricaoSuframa = obj.InscricaoSuframa,
				RazaoSocial = obj.RazaoSocial,
				TipoModalidade = obj.TipoModalidade,
				TipoStatus = obj.TipoStatus,
				DataValidade = obj.DataValidade,
				ValorPremio = obj.ValorPremio,
				Cnpj = obj.Cnpj
			};
			_uowSciex.CommandStackSciex.DetachEntries();
			_uowSciex.CommandStackSciex.Processo.Salvar(processoEntity);
			_uowSciex.CommandStackSciex.Save();
		}

		private void InserirPRCStatus(PRCStatusVM obj)
		{
			var prcStatusEntity = new PRCStatusEntity()
			{
				Tipo = obj.Tipo,
				Data = obj.Data,
				DataValidade = obj.DataValidade,
				CpfResponsavel = obj.CpfResponsavel.CnpjCpfUnformat(),
				NomeResponsavel = obj.NomeResponsavel,
				IdProcesso = obj.IdProcesso,
				NumeroPlano = obj.NumeroPlano,
				AnoPlano = obj.AnoPlano
			};

			_uowSciex.CommandStackSciex.DetachEntries();
			_uowSciex.CommandStackSciex.PRCStatus.Salvar(prcStatusEntity);
			_uowSciex.CommandStackSciex.Save();
		}

		private void InserirPRCProduto(PEProdutoVM obj, int IdProcesso)
		{
			var prcProdutoEntity = new PRCProdutoEntity()
			{

				CodigoProdutoExportacao = obj.CodigoProdutoExportacao,
				CodigoProdutoSuframa = obj.CodigoProdutoSuframa,
				CodigoNCM = obj.CodigoNCM,
				TipoProduto = obj.CodigoTipoProduto,
				DescricaoModelo = obj.DescricaoModelo,
				QuantidadeAprovado = obj.Qtd,
				CodigoUnidade = obj.CodigoUnidade,
				ValorDolarAprovado = obj.ValorDolar,
				ValorFluxoCaixa = obj.ValorFluxoCaixa,
				IdProcesso = IdProcesso
			};

			_uowSciex.CommandStackSciex.DetachEntries();
			_uowSciex.CommandStackSciex.PRCProduto.Salvar(prcProdutoEntity);
			_uowSciex.CommandStackSciex.Save();
		}

		private void InserirPRCProdutoPais(List<PEProdutoPaisEntity> listObj, int IdPrcProduto)
		{
			foreach (var item in listObj)
			{
				var prcProdutoPaisEntity = new PRCProdutoPaisEntity()
				{
					QuantidadeAprovado = item.Quantidade,
					ValorDolarAprovado = item.ValorDolar,
					CodigoPais = item.CodigoPais,
					IdPrcProduto = IdPrcProduto,
				};
				_uowSciex.CommandStackSciex.PRCProdutoPais.Salvar(prcProdutoPaisEntity);
			}
			_uowSciex.CommandStackSciex.Save();
		}

		private void InserirPRCInsumo(PEInsumoEntity obj, int IdPrcProduto)
		{
			var prcInsumoEntity = new PRCInsumoEntity()
			{
				CodigoInsumo = obj.CodigoInsumo,
				CodigoUnidade = obj.CodigoUnidade,
				TipoInsumo = obj.TipoInsumo,
				CodigoNCM = obj.CodigoNcm,
				ValorPercentualPerda = obj.ValorPercentualPerda,
				CodigoDetalhe = obj.CodigoDetalhe,
				DescricaoInsumo = obj.DescricaoInsumo,
				DescricaoPartNumber = obj.DescricaoPartNumber,
				DescricaoEspecificacaoTecnica = obj.DescricaoEspecificacaoTecnica,
				ValorCoeficienteTecnico = obj.ValorCoeficienteTecnico,
				IdPrcProduto = IdPrcProduto,
				StatusInsumo = (int)EnumSituacaoInsumo.ATIVO_OU_SIM
			};

			_uowSciex.CommandStackSciex.PRCInsumo.Salvar(prcInsumoEntity);
			_uowSciex.CommandStackSciex.Save();
		}

		private decimal? InserirPRCDetalheInsumo(int IdPEInsumo, int IdPrcInsumo)
		{
			var listDetlheInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(l => l.IdPEInsumo == IdPEInsumo
																						&&
																						l.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.APROVADO);

			decimal? somatorioValorUnitario = 0;
			foreach (var item in listDetlheInsumo)
			{
				var prcDetalheInsumoEntity = new PRCDetalheInsumoEntity()
				{
					NumeroSequencial = item.NumeroSequencial,
					CodigoPais = item.CodigoPais,
					Quantidade = item.Quantidade,
					ValorUnitario = item.ValorUnitario,
					ValorFrete = item.ValorFrete,
					IdMoeda = item.IdMoeda,
					IdPrcInsumo = IdPrcInsumo
				};

				somatorioValorUnitario += prcDetalheInsumoEntity.ValorUnitario;

				_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(prcDetalheInsumoEntity);
			}
			_uowSciex.CommandStackSciex.Save();

			return somatorioValorUnitario;
		}

		private void CalcularValoresImportados(int idProcesso)
		{
			var listaProdutos = _uowSciex.QueryStackSciex.PRCProduto.Listar(q => q.IdProcesso == idProcesso).ToList();

			if (listaProdutos.Count > 0)
			{
				decimal fatorConvEmDolar = 0;
				decimal somatorioValorDolar = 0;
				decimal somatorioValorDolarFOB = 0;

				decimal somatorioQtAprovada = 0;
				decimal somatorioValorDolarCFR = 0;
				decimal somatorioValorFrete = 0;

				try
				{
					foreach (var regProduto in listaProdutos)
					{
						_uowSciex.CommandStackSciex.DetachEntries();

						var listaInsumos = _uowSciex.QueryStackSciex.PRCInsumo.Listar(q => q.IdPrcProduto == regProduto.IdProduto).ToList();

						if (listaInsumos.Count > 0)
						{
							foreach (var regInsumo in listaInsumos)
							{
								_uowSciex.CommandStackSciex.DetachEntries();

								var listaDetalhesInsumo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Listar(q => q.IdPrcInsumo == regInsumo.IdInsumo).ToList();

								if (listaDetalhesInsumo.Count > 0)
								{
									foreach (var regDetalheInsumo in listaDetalhesInsumo)
									{
										_uowSciex.CommandStackSciex.DetachEntries();

										decimal valorInsDolar = 0;

										if (regDetalheInsumo.Moeda.CodigoMoeda != 220)
										{
											CalcularFatorConversao(regDetalheInsumo.Moeda.CodigoMoeda, ref fatorConvEmDolar);

											valorInsDolar = regDetalheInsumo.ValorUnitario == null 
																		? 0 
																		:decimal.Parse(regDetalheInsumo.ValorUnitario.ToString()) 

															* regDetalheInsumo.Quantidade 
															* fatorConvEmDolar;
										}
										else
										{
											valorInsDolar = regDetalheInsumo.ValorUnitario == null
																		? 0
																		: decimal.Parse(regDetalheInsumo.ValorUnitario.ToString())
															* regDetalheInsumo.Quantidade;
										}

										regDetalheInsumo.ValorDolar = valorInsDolar + (regDetalheInsumo.ValorFrete != null ? regDetalheInsumo.ValorFrete : 0);
										regDetalheInsumo.ValorDolarFOB = valorInsDolar;
										regDetalheInsumo.ValorDolarCFR = valorInsDolar + (regDetalheInsumo.ValorFrete != null ? regDetalheInsumo.ValorFrete : 0);

										_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(regDetalheInsumo);
										_uowSciex.CommandStackSciex.Save();

										somatorioValorDolar += regDetalheInsumo.ValorDolar != null ? (decimal)regDetalheInsumo.ValorDolar : 0;
										somatorioValorDolarFOB += regDetalheInsumo.ValorDolarFOB != null ? (decimal)regDetalheInsumo.ValorDolarFOB : 0;

										somatorioQtAprovada += regDetalheInsumo.Quantidade;
										somatorioValorDolarCFR += regDetalheInsumo.ValorDolarCFR == null ? 0 :
															decimal.Parse(regDetalheInsumo.ValorDolarCFR.ToString());

										somatorioValorFrete += regDetalheInsumo.ValorFrete == null ? 0 : decimal.Parse(regDetalheInsumo.ValorFrete.ToString());

										_uowSciex.CommandStackSciex.DetachEntries();
									}

									var regInsumoAtualizado = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(q => q.IdInsumo == regInsumo.IdInsumo);

									regInsumoAtualizado.ValorDolarAprovado = regInsumoAtualizado.ValorDolarSaldo = somatorioValorDolar;
									regInsumoAtualizado.QuantidadeAprovado = regInsumoAtualizado.QuantidadeSaldo = somatorioQtAprovada;
									regInsumoAtualizado.ValorDolarFOBAprovado = somatorioValorDolarFOB;
									regInsumoAtualizado.ValorDolarCFRAprovado = somatorioValorDolarCFR;
									regInsumoAtualizado.ValorFreteAprovado = somatorioValorFrete;

									_uowSciex.CommandStackSciex.PRCInsumo.Salvar(regInsumoAtualizado);
									_uowSciex.CommandStackSciex.Save();

									somatorioValorFrete = 0;
									somatorioValorDolarCFR = 0;
									somatorioValorDolarFOB = 0;
									somatorioQtAprovada = 0;
									somatorioValorDolar = 0;
									
									//#region CalculoFluxoCaixa
									//var valorTotInsFOB = somatorioValorDolarFOB;
									//var valorProduto = regProduto.ValorDolar;

									//var fluxoCaixa = (1 - valorTotInsFOB / valorProduto) * 100;

									//var regProdutoAtualizado = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(q => q.IdPEProduto == regProduto.IdPEProduto);
									//regProdutoAtualizado.ValorFluxoCaixa = fluxoCaixa;
									//#endregion

									//_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regProdutoAtualizado);
									//_uowSciex.CommandStackSciex.Save();

								}

								_uowSciex.CommandStackSciex.DetachEntries();
							}
						}

						_uowSciex.CommandStackSciex.DetachEntries();
					}
				}
				catch (Exception e)
				{
					throw new Exception("A soma dos valores resultou em um valor acima do permitido. Por favor, revise os valores informados");
				}
			}
		}

		private void CalcularFatorConversao(int? codMoeda, ref decimal fatorConvEmDolar)
		{
			decimal fatorMoedaEstrangeira = 0;
			decimal fatorMoedaDolar = 0;
			var dataHoje = DateTime.Now.Date;

			fatorMoedaEstrangeira = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar
									(q => q.Moeda.CodigoMoeda == codMoeda && q.ParidadeCambial.DataParidade == dataHoje).Valor;

			int codigoDolar = 220;
			fatorMoedaDolar = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar
								(q => q.Moeda.CodigoMoeda == codigoDolar && q.ParidadeCambial.DataParidade == dataHoje).Valor;

			fatorConvEmDolar = fatorMoedaEstrangeira / fatorMoedaDolar;
		}

		private void SalvarPlanoExportacao(PlanoExportacaoVM obj, int numeroNovoProcesso)
		{
			var planoExportacaoEntity = new PlanoExportacaoEntity()
			{
				IdPlanoExportacao = obj.IdPlanoExportacao,
				NumeroPlano = obj.NumeroPlano,
				AnoPlano = obj.AnoPlano,
				NumeroInscricaoCadastral = obj.NumeroInscricaoCadastral,
				Cnpj = obj.Cnpj,
				RazaoSocial = obj.RazaoSocial,
				TipoModalidade = obj.TipoModalidade,
				TipoExportacao = obj.TipoExportacao,
				Situacao = (int)EnumSituacaoPlanoExportacao.DEFERIDO,
				DataStatus = DateTime.Now,
				NumeroAnoProcesso = DateTime.Now.Year,
				NumeroProcesso = numeroNovoProcesso,
				DataEnvio = obj.DataEnvio,
				DataCadastro = obj.DataCadastro,
				CpfResponsavel = obj.CpfResponsavel,
				NomeResponsavel = obj.NomeResponsavel

			};

			_uowSciex.CommandStackSciex.DetachEntries();
			_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(planoExportacaoEntity);
			_uowSciex.CommandStackSciex.Save();
		}
	}
}
