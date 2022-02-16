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
using System.Linq.Expressions;

namespace Suframa.Sciex.BusinessLogic
{
	public class PEDetalheInsumoBll : IPEDetalheInsumoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;

		public PEDetalheInsumoBll(
			IUnitOfWorkSciex uowSciex, 
			IUnitOfWork uowCadsuf,
			IUsuarioPssBll usuarioPssBll, 
			IUsuarioInformacoesBll usuarioInformacoesBll
			)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_usuarioPssBll = usuarioPssBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
		}

		public DadosDetalhesInsumosVM ListarDetalhePorIdInsumo(SalvarDetalheVM vm)
		{
			if (vm == null || vm.IdPEInsumo == 0)
				return null;

			List<PEDetalheInsumoImportadoVM> listaDetalhes = new List<PEDetalheInsumoImportadoVM>();

			if (vm.IsQuadroNacional)
			{
				listaDetalhes = _uowSciex.QueryStackSciex.PEDetalheInsumo.ListarGrafo(q => new PEDetalheInsumoImportadoVM()
				{
					IdPEDetalheInsumo = q.IdPEDetalheInsumo,
					IdPEInsumo = q.IdPEInsumo,
					IdMoeda = q.IdMoeda,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCRF = q.ValorDolarCRF,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario,
					SituacaoAnalise = q.SituacaoAnalise,
					DescricaoJustificativaErro = q.DescricaoJustificativaErro
				}
					,
					q => q.IdPEInsumo == vm.IdPEInsumo).ToList(); 
			}
			else
			{
				listaDetalhes = _uowSciex.QueryStackSciex.PEDetalheInsumo.ListarGrafo(q => new PEDetalheInsumoImportadoVM()
				{
					IdPEDetalheInsumo = q.IdPEDetalheInsumo,
					IdPEInsumo = q.IdPEInsumo,
					IdMoeda = q.IdMoeda,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCRF = q.ValorDolarCRF,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario,
					DescricaoMoeda = q.Moeda.Descricao,
					CodigoMoeda = q.Moeda.CodigoMoeda,
					SituacaoAnalise = q.SituacaoAnalise,
					DescricaoJustificativaErro = q.DescricaoJustificativaErro
				}
					,
					q => q.IdPEInsumo == vm.IdPEInsumo).ToList();
			}

			var dados = new DadosDetalhesInsumosVM()
			{
				listaDetalhesInsumos = listaDetalhes
			};

			if (listaDetalhes.Count > 0)
			{
				dados.QtdTotalInsumos = dados.listaDetalhesInsumos?.Sum(q => q.Quantidade) ?? 0;
				dados.QtdTotalInsumosFormatada = dados.QtdTotalInsumos != null ? Convert.ToDecimal(dados.QtdTotalInsumos).ToString("N5") : "0";

				foreach (var registroDetalhe in listaDetalhes)
				{
					dados.ValorTotalInsumos += registroDetalhe.Quantidade * registroDetalhe.ValorUnitario;
					dados.ValorTotalInsumosFormatada = dados.ValorTotalInsumos != null ? Convert.ToDecimal(dados.ValorTotalInsumos).ToString("N7") : "0";


					registroDetalhe.ValorInsumo = registroDetalhe.Quantidade * registroDetalhe.ValorUnitario;
					registroDetalhe.ValorUnitarioFormatada = registroDetalhe.ValorUnitario != 0 ? Convert.ToDecimal(registroDetalhe.ValorUnitario).ToString("N7") : "0";

					registroDetalhe.QuantidadeFormatada = registroDetalhe.Quantidade.ToString("N5");

					registroDetalhe.SituacaoAnaliseString = registroDetalhe.SituacaoAnalise != null ? Enum.GetName(typeof(EnumSituacaoAnalisePlanoExportacao), registroDetalhe.SituacaoAnalise).Replace("_", " ") : "-";


					if (!vm.IsQuadroNacional)
					{
						var codigoPais = registroDetalhe.CodigoPais.ToString("D3");
						registroDetalhe.DescricaoPais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(q => q.CodigoPais == codigoPais).Descricao;
						registroDetalhe.ValorDolarFOBFormatada = registroDetalhe.ValorDolarFOB != null ? Convert.ToDecimal(registroDetalhe.ValorDolarFOB).ToString("N7") : "0";
						registroDetalhe.ValorFreteFormatada = registroDetalhe.ValorFrete != null ? Convert.ToDecimal(registroDetalhe.ValorFrete).ToString("N7") : "0";

					}
					
				}
			}
			

			return dados;
		}

		public PagedItems<PEDetalheInsumoImportadoVM> ListarPaginadoDetalhePorIdInsumo(SalvarDetalheVM vm)
		{
			if (vm == null || vm.IdPEInsumo == 0)
				return null;

			PagedItems<PEDetalheInsumoImportadoVM> listaDetalhes = new PagedItems<PEDetalheInsumoImportadoVM>();

			string filtroPosterior = null;
			if (vm.Sort != null)
			{
				if (vm.Sort.Equals("ValorTotal"))
				{
					filtroPosterior = vm.Sort;
					vm.Sort = null;
				}
			}

			if (vm.IsQuadroNacional)
			{
				
				listaDetalhes = _uowSciex.QueryStackSciex.PEDetalheInsumo.ListarPaginadoGrafo(q => new PEDetalheInsumoImportadoVM()
				{
					IdPEDetalheInsumo = q.IdPEDetalheInsumo,
					IdPEInsumo = q.IdPEInsumo,
					IdMoeda = q.IdMoeda,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCRF = q.ValorDolarCRF,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario,
					SituacaoAnalise = q.SituacaoAnalise,
					DescricaoJustificativaErro = q.DescricaoJustificativaErro,
					ValorTotal = q.Quantidade * q.ValorDolar
				}
				,
				q => q.IdPEInsumo == vm.IdPEInsumo
				,
				vm
				);
				
			}
			else
			{
				listaDetalhes = _uowSciex.QueryStackSciex.PEDetalheInsumo.ListarPaginadoGrafo(q => new PEDetalheInsumoImportadoVM()
				{
					IdPEDetalheInsumo = q.IdPEDetalheInsumo,
					IdPEInsumo = q.IdPEInsumo,
					IdMoeda = q.IdMoeda,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCRF = q.ValorDolarCRF,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario,
					DescricaoMoeda = q.Moeda.Descricao,
					CodigoMoeda = q.Moeda.CodigoMoeda,
					SituacaoAnalise = q.SituacaoAnalise,
					DescricaoJustificativaErro = q.DescricaoJustificativaErro
				}
				,
				q => q.IdPEInsumo == vm.IdPEInsumo
				, 
				vm);
			}

			if (listaDetalhes.Total > 0)
			{

				foreach (var registroDetalhe in listaDetalhes.Items)
				{
					registroDetalhe.ValorInsumo = registroDetalhe.Quantidade * registroDetalhe.ValorUnitario;
					registroDetalhe.ValorUnitarioFormatada = registroDetalhe.ValorUnitario != 0 ? Convert.ToDecimal(registroDetalhe.ValorUnitario).ToString("N7") : "0";

					registroDetalhe.QuantidadeFormatada = registroDetalhe.Quantidade.ToString("N5");

					if (!vm.IsQuadroNacional)
					{
						var codigoPais = registroDetalhe.CodigoPais.ToString("D3");
						registroDetalhe.DescricaoPais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(q => q.CodigoPais == codigoPais).Descricao;
						registroDetalhe.ValorDolarFOBFormatada = registroDetalhe.ValorDolarFOB != null ? Convert.ToDecimal(registroDetalhe.ValorDolarFOB).ToString("N7") : "0";
						registroDetalhe.ValorFreteFormatada = registroDetalhe.ValorFrete != null ? Convert.ToDecimal(registroDetalhe.ValorFrete).ToString("N7") : "0";

					}

				}
			}

			if (!string.IsNullOrEmpty(filtroPosterior))
			{
				if (filtroPosterior.Equals("ValorTotal"))
				{
					if (!vm.Reverse)
					{
						listaDetalhes.Items = listaDetalhes.Items.OrderBy(q => q.ValorTotal).ToList();
					}
					else
					{
						listaDetalhes.Items = listaDetalhes.Items.OrderByDescending(q => q.ValorTotal).ToList();
					} 
				}
			}


			return listaDetalhes;
		}

		enum EnumStatusSalvarDetalhe
		{
			ERRO = 1,
			DUPLICIDADE = 2,
			SUCESSO = 3,
			ACIMA_LIMITE = 4
		}

		public int AtualizarDetalhe(SalvarDetalheVM vm)
		{
			if (vm == null || vm.IdPEDetalheInsumo == 0) { return 3; }

			var regDetalhe = _uowSciex.QueryStackSciex.PEDetalheInsumo.Selecionar(q => q.IdPEDetalheInsumo == vm.IdPEDetalheInsumo);

			if (regDetalhe == null)
			{
				return 3;
			}
			else
			{
				try
				{
					var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == vm.IdPEInsumo);

					var qtdProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(q => q.IdPEProduto == vm.IdPEProduto).Qtd;
					var vlrCoefTecnico = regInsumo.ValorCoeficienteTecnico;
					var vlrPercPerda = regInsumo.ValorPercentualPerda != null ? regInsumo.ValorPercentualPerda : 0;

					var qtMaxima = qtdProduto * vlrCoefTecnico;

					qtMaxima = qtMaxima + (qtMaxima * (vlrPercPerda / 100));

					var totalJaIncluido = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == vm.IdPEInsumo)?.Sum(q => q.Quantidade) ?? 0;

					var qtdPermitida = qtMaxima - totalJaIncluido;

					if (vm.Quantidade > qtdPermitida)
					{
						return 4;
					}
					else
					{
						regDetalhe.Quantidade = vm.Quantidade;
						regDetalhe.ValorUnitario = vm.ValorUnitario;

						if (!vm.IsQuadroNacional)
						{
							regDetalhe.ValorFrete = vm.ValorFrete;
						}


						_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(regDetalhe);
						_uowSciex.CommandStackSciex.Save();

						return 1;
					}
					
				}
				catch (Exception e)
				{
					return 3;
				}
			}


			
		}

		public ResultadoCorrigirDetalheInsumoVM CorrigirDetalhe(SalvarDetalheVM vm)
		{
			if (vm == null || vm.IdPEDetalheInsumo == 0) { return new ResultadoCorrigirDetalheInsumoVM() { Resultado = false, Mensagem="Necessário ID detalhe insumo." }; }

			var regDetalhe = _uowSciex.QueryStackSciex.PEDetalheInsumo.Selecionar(q => q.IdPEDetalheInsumo == vm.IdPEDetalheInsumo);

			if (regDetalhe == null)
			{
				return new ResultadoCorrigirDetalheInsumoVM() { Resultado = false, Mensagem = "Registro de detalhe não encontrado." };
			}
			else
			{
				var result = new ResultadoCorrigirDetalheInsumoVM()
				{
					Resultado = true
				};

				try
				{
					var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == vm.IdPEInsumo);

					var qtdProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(q => q.IdPEProduto == vm.IdPEProduto).Qtd;
					var vlrCoefTecnico = regInsumo.ValorCoeficienteTecnico;
					var vlrPercPerda = regInsumo.ValorPercentualPerda != null ? regInsumo.ValorPercentualPerda : 0;

					var qtMaxima = qtdProduto * vlrCoefTecnico;

					qtMaxima = qtMaxima + (qtMaxima * (vlrPercPerda / 100));

					//var totalJaIncluido = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == vm.IdPEInsumo)?.Sum(q => q.Quantidade) ?? 0;
					var listaDetalhes = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == vm.IdPEInsumo);

					decimal quantidadeTotalJaIncluidos = 0;

					foreach (var detalhe in listaDetalhes)
					{
						if (detalhe.IdPEDetalheInsumo != vm.IdPEDetalheInsumo)
						{
							quantidadeTotalJaIncluidos += detalhe.Quantidade;
						}
						
					}

					var qtdPermitida = qtMaxima - quantidadeTotalJaIncluidos;

					if (vm.Quantidade > qtdPermitida)
					{
						result.Resultado = false;
						result.Mensagem = $"Quantidade informada é maior do que a permitida.";
						return result;
					}
					else
					{
						if (vm.IsQuadroNacional)
						{
							var dadosJaExistem = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.Quantidade == vm.Quantidade && q.ValorUnitario == vm.ValorUnitario).Count;


							if (dadosJaExistem > 0)
							{
								result.Resultado = false;
								result.Mensagem = "Detalhe já cadastrado.";
								return result; 
							}
						}
						
						_uowSciex.CommandStackSciex.DetachEntries();

						var regInsumoDE = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == vm.IdPEInsumo);

						if (regInsumoDE == null)
						{
							result.Resultado = false;
							result.Mensagem = $"Não foi encontrado insumo com o id {vm.IdPEInsumo}";
							return result;
						}
						else
						{
							if (regInsumoDE.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO)
							{
								regInsumoDE.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO;

								var regInsumoPARA = new PEInsumoEntity()
								{
									IdPEProduto = regInsumoDE.IdPEProduto,
									CodigoDetalhe = regInsumoDE.CodigoDetalhe,
									CodigoInsumo = regInsumoDE.CodigoInsumo,
									CodigoNcm = regInsumoDE.CodigoNcm,
									CodigoUnidade = regInsumoDE.CodigoUnidade,
									DescricaoEspecificacaoTecnica = regInsumoDE.DescricaoEspecificacaoTecnica,
									DescricaoInsumo = regInsumoDE.DescricaoInsumo,
									DescricaoJustificativaErro = regInsumoDE.DescricaoJustificativaErro,
									DescricaoPartNumber = regInsumoDE.DescricaoPartNumber,
									SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO,
									TipoInsumo = regInsumoDE.TipoInsumo,
									ValorCoeficienteTecnico = regInsumoDE.ValorCoeficienteTecnico,
									ValorDolar = regInsumoDE.ValorDolar,
									ValorPercentualPerda = regInsumoDE.ValorPercentualPerda
								};

								if (regInsumoDE.ListaPEDetalheInsumo.Count > 0)
								{
									regInsumoPARA.ListaPEDetalheInsumo = new List<PEDetalheInsumoEntity>();

									foreach (var detalheDE in regInsumoDE.ListaPEDetalheInsumo)
									{
										if (detalheDE.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.APROVADO)
										{
											regInsumoPARA.ListaPEDetalheInsumo.Add(detalheDE);
										}
										else if (detalheDE.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO 
													||
												detalheDE.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO)
										{
											PEDetalheInsumoEntity copiaDetalhe = null;

											if (detalheDE.IdPEDetalheInsumo == vm.IdPEDetalheInsumo)
											{
												if (vm.IsQuadroNacional)
												{
													copiaDetalhe = new PEDetalheInsumoEntity()
													{
														IdPEInsumo = detalheDE.IdPEInsumo,
														IdMoeda = detalheDE.IdMoeda,
														Quantidade = vm.Quantidade,
														ValorUnitario = vm.ValorUnitario,
														ValorFrete = detalheDE.ValorFrete,
														CodigoPais = detalheDE.CodigoPais,
														DescricaoJustificativaErro = detalheDE.DescricaoJustificativaErro,
														NumeroSequencial = detalheDE.NumeroSequencial,
														SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO,
														ValorDolar = detalheDE.ValorDolar,
														ValorDolarCRF = detalheDE.ValorDolarCRF,
														ValorDolarFOB = detalheDE.ValorDolarFOB,
													}; 
												}
												else
												{
													copiaDetalhe = new PEDetalheInsumoEntity()
													{
														IdPEInsumo = detalheDE.IdPEInsumo,
														IdMoeda = detalheDE.IdMoeda,
														Quantidade = vm.Quantidade,
														ValorUnitario = vm.ValorUnitario,
														ValorFrete = vm.ValorFrete,
														CodigoPais = detalheDE.CodigoPais,
														DescricaoJustificativaErro = detalheDE.DescricaoJustificativaErro,
														NumeroSequencial = detalheDE.NumeroSequencial,
														SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO,
														ValorDolar = detalheDE.ValorDolar,
														ValorDolarCRF = detalheDE.ValorDolarCRF,
														ValorDolarFOB = detalheDE.ValorDolarFOB,
													};
												}
											}
											else
											{
												copiaDetalhe = new PEDetalheInsumoEntity()
												{
													IdPEInsumo = detalheDE.IdPEInsumo,
													IdMoeda = detalheDE.IdMoeda,
													Quantidade = detalheDE.Quantidade,
													ValorUnitario = detalheDE.ValorUnitario,
													ValorFrete = detalheDE.ValorFrete,
													CodigoPais = detalheDE.CodigoPais,
													DescricaoJustificativaErro = detalheDE.DescricaoJustificativaErro,
													NumeroSequencial = detalheDE.NumeroSequencial,
													SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO,
													ValorDolar = detalheDE.ValorDolar,
													ValorDolarCRF = detalheDE.ValorDolarCRF,
													ValorDolarFOB = detalheDE.ValorDolarFOB,
												};
											}

											regInsumoPARA.ListaPEDetalheInsumo.Add(copiaDetalhe);

											detalheDE.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO;
											_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalheDE);
										}

									}


								}

								_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoDE);
								_uowSciex.CommandStackSciex.Save();

								//_uowSciex.CommandStackSciex.DetachEntries();

								_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoPARA);
								_uowSciex.CommandStackSciex.Save();

								result.IdNovoInsumo = regInsumoPARA.IdPEInsumo;

							}
							else if (regInsumoDE.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO)
							{
								
								regDetalhe.Quantidade = vm.Quantidade;
								regDetalhe.ValorUnitario = vm.ValorUnitario;
								regDetalhe.ValorFrete = vm.ValorFrete;
								_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(regDetalhe);
								_uowSciex.CommandStackSciex.Save();
								result.IdNovoInsumo = regDetalhe.IdPEInsumo;
							}
						}


						return result;
					}

				}
				catch (Exception e)
				{
					result.Resultado = false;
					result.Mensagem = $"Erro ao processar. (MESSAGE: {e.Message} / INNEREXCEPTION: {e.InnerException} / STACKTRACE: {e.StackTrace})";
					return result;
				}
			}



		}

		public ResultadoMensagemProcessamentoVM InativarDetalheInsumo(PEDetalheInsumoVM vm)
		{
			if (vm == null || vm.IdPEDetalheInsumo == 0)
			{
				return new ResultadoMensagemProcessamentoVM()
				{
					Resultado = false,
					Mensagem = "Id do insumo inválido"
				};
			}

			var result = new ResultadoMensagemProcessamentoVM()
			{
				Resultado = true
			};

			var regDetalheInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Selecionar(q => q.IdPEDetalheInsumo == vm.IdPEDetalheInsumo);

			if (regDetalheInsumo == null)
			{
				result.Resultado = false;
				result.Mensagem = $"Não foi encontrado insumo com o id {vm.IdPEDetalheInsumo}";
				return result;
			}
			else
			{
				regDetalheInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
				_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(regDetalheInsumo);
				_uowSciex.CommandStackSciex.Save();
			}

			return result;
		}
		public int SalvarNovoDetalhe(SalvarDetalheVM vm)
		{
			if (vm == null || vm.IdPEInsumo == 0) { return (int)EnumStatusSalvarDetalhe.ERRO; }

			var regDetalhe = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.Quantidade == vm.Qtd && q.ValorUnitario == vm.ValorUnitario && q.IdPEInsumo == vm.IdPEInsumo).ToList();

			if (regDetalhe.Count > 0)
			{
				return (int)EnumStatusSalvarDetalhe.DUPLICIDADE;
			}
			else
			{
				var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == vm.IdPEInsumo);

				var qtdProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(q => q.IdPEProduto == vm.IdPEProduto).Qtd;
				var vlrCoefTecnico = regInsumo.ValorCoeficienteTecnico;
				var vlrPercPerda = regInsumo.ValorPercentualPerda != null ? regInsumo.ValorPercentualPerda : 0;

				var qtMaxima = qtdProduto * vlrCoefTecnico;

				qtMaxima = qtMaxima + (qtMaxima * (vlrPercPerda / 100));

				var totalJaIncluido = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == vm.IdPEInsumo)?.Sum(q=> q.Quantidade) ?? 0;

				var qtdPermitida = qtMaxima - totalJaIncluido;

				if (vm.Qtd > qtdPermitida)
				{
					return (int)EnumStatusSalvarDetalhe.ACIMA_LIMITE;
				}
				else
				{

					try
					{

						regInsumo.DescricaoPartNumber = vm.DescricaoPartNumber;
						regInsumo.ValorPercentualPerda = vm.ValorPercentualPerda;


						if (vm.IsQuadroNacional)
						{
							var idMoedaNacional = _uowSciex.QueryStackSciex.Moeda.Selecionar(q => q.CodigoMoeda == 790).IdMoeda;
							var numeroSeqAtual = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == vm.IdPEInsumo)?.LastOrDefault()?.NumeroSequencial ?? 0;

							var regNovoDetalhe = new PEDetalheInsumoEntity()
							{
								NumeroSequencial = numeroSeqAtual == 0 ? 1 : numeroSeqAtual + 1,
								Quantidade = vm.Qtd,
								ValorUnitario = vm.ValorUnitario,
								IdPEInsumo = vm.IdPEInsumo,
								IdMoeda = idMoedaNacional
							};

							_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(regNovoDetalhe);
							_uowSciex.CommandStackSciex.Save();
						}
						else
						{
							var numeroSeqAtual = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == vm.IdPEInsumo)?.LastOrDefault()?.NumeroSequencial ?? 0;

							var regNovoDetalhe = new PEDetalheInsumoEntity()
							{
								NumeroSequencial = numeroSeqAtual == 0 ? 1 : numeroSeqAtual + 1,
								Quantidade = vm.Qtd,
								ValorUnitario = vm.ValorUnitarioFOB,
								CodigoPais = vm.CodigoPais,
								IdMoeda = vm.IdMoeda,
								ValorFrete = vm.ValorFrete,
								IdPEInsumo = vm.IdPEInsumo,
							};

							_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(regNovoDetalhe);
							_uowSciex.CommandStackSciex.Save();
						}

						return (int)EnumStatusSalvarDetalhe.SUCESSO;
					}

					catch (Exception e)
					{
						return (int)EnumStatusSalvarDetalhe.ERRO;
					}
				}
				
			}

		}

		public bool Deletar(int id)
		{
			if (id == 0)
				return false;

			try
			{
				var regDetalheInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Selecionar(s => s.IdPEDetalheInsumo == id);

				if (regDetalheInsumo != null)
				{

					_uowSciex.CommandStackSciex.PEDetalheInsumo.Apagar(regDetalheInsumo.IdPEDetalheInsumo);
					_uowSciex.CommandStackSciex.Save();
				}

				return true;
			}
			catch (Exception e)
			{
				return false;
			}

		}
		public bool AdicionarInsumoAoProduto(LEInsumoVM vm)
		{
			if (vm == null || vm.IdPEProduto == 0)
				return false;

			try
			{
				foreach (var dadosInsumo in vm.ListaInsumosSelecionados)
				{

					var novoInsumo = new PEInsumoEntity()
					{
						CodigoInsumo = dadosInsumo.CodigoInsumo,
						CodigoUnidade = dadosInsumo.CodigoUnidadeMedida,
						TipoInsumo = dadosInsumo.TipoInsumo,
						CodigoNcm = dadosInsumo.CodigoNCM,
						CodigoDetalhe = dadosInsumo.CodigoDetalhe,
						DescricaoInsumo = dadosInsumo.DescricaoInsumo,
						DescricaoEspecificacaoTecnica = dadosInsumo.DescricaoEspecTecnica,
						IdPEProduto = vm.IdPEProduto
					};

					_uowSciex.CommandStackSciex.PEInsumo.Salvar(novoInsumo);
				}

				_uowSciex.CommandStackSciex.Save();

				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}
		public PagedItems<LEInsumoVM> ListarInsumosPorCodigoPENacionalOuImportado(ListarInsumosNacionalImportadosVM vm)
		{

			if (vm == null || vm.CodigoProdutoExportacao == 0)
				return null;

			var listaCodigoInsumosIncluidos = _uowSciex.QueryStackSciex.PEInsumo.Listar(q => q.IdPEProduto == vm.IdPEProduto).Select(q => q.CodigoInsumo).ToList();

			PagedItems<LEInsumoVM> listaInsumo = null;

			Expression<Func<LEInsumoEntity, bool>> predicado;

			if (listaCodigoInsumosIncluidos.Count > 0)
			{
				predicado = q => q.CodigoProduto == vm.CodigoProdutoExportacao
									&&
									q.SituacaoInsumo == 1
									&&
									(q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R"))
									&&
									!listaCodigoInsumosIncluidos.Contains(q.CodigoInsumo)
									;
}
			else
			{
				predicado = q => q.CodigoProduto == vm.CodigoProdutoExportacao
									&&
									q.SituacaoInsumo == 1
									&&
									(q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R"));
			}

			if (vm.isQuadroNacional)
			{
				listaInsumo = _uowSciex.QueryStackSciex.LEInsumo.ListarPaginado<LEInsumoVM>(
																		predicado
																		, vm);



			}
			else
			{
				listaInsumo = _uowSciex.QueryStackSciex.LEInsumo.ListarPaginado<LEInsumoVM>(q =>
																		q.CodigoProduto == vm.CodigoProdutoExportacao
																		&&
																		q.SituacaoInsumo == 1
																		&&
																		(q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E"))
																		&&
																		!vm.ListaCodigoInsumosIncluidos.Contains(q.CodigoInsumo)

																		, vm);
			}


			return listaInsumo;
		}

		public PagedItems<PEInsumoVM> ListarInsumosNacionalImportadosPorIdProduto(ListarInsumosNacionalImportadosVM vm)
		{
			if (vm == null || vm.IdPEProduto == 0)
				return null;

			if (vm.isQuadroNacional)
			{
				var listaInsumos = _uowSciex.QueryStackSciex.PEInsumo.ListarPaginadoGrafo(w => 

				new PEInsumoVM()
				{
					IdPEInsumo = w.IdPEInsumo,
					IdPEProduto = w.IdPEProduto,
					CodigoDetalhe = w.CodigoDetalhe,
					CodigoInsumo = w.CodigoInsumo,
					CodigoNcm = w.CodigoNcm,
					CodigoUnidade = w.CodigoUnidade,
					DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
					DescricaoInsumo = w.DescricaoInsumo,
					DescricaoPartNumber = w.DescricaoPartNumber,
					TipoInsumo = w.TipoInsumo,
					ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
					ValorDolar = w.ValorDolar,
					ValorPercentualPerda = w.ValorPercentualPerda,
					ListaPEDetalheInsumo = w.ListaPEDetalheInsumo.Select(q=> new PEDetalheInsumoVM()
					{
						IdPEInsumo = q.IdPEInsumo,
						IdMoeda = q.IdMoeda,
						IdPEDetalheInsumo = q.IdPEDetalheInsumo,
						CodigoPais = q.CodigoPais,
						NumeroSequencial = q.NumeroSequencial,
						Quantidade = q.Quantidade,
						ValorDolar = q.ValorDolar,
						ValorDolarCRF = q.ValorDolarCRF,
						ValorDolarFOB = q.ValorDolarFOB,
						ValorFrete = q.ValorFrete,
						ValorUnitario = q.ValorUnitario
					}).ToList()

				}, 
				q=> q.IdPEProduto == vm.IdPEProduto && (q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R")),vm);

				foreach (var regInsumo in listaInsumos.Items)
				{
					regInsumo.QtdSomatorioDetalheInsumo = regInsumo.ListaPEDetalheInsumo?.Sum(q => q.Quantidade) ?? 0;

					decimal valorSomatorio = 0;

					foreach (var regDetalheInsumo in regInsumo.ListaPEDetalheInsumo)
					{
						valorSomatorio = regDetalheInsumo.Quantidade * regDetalheInsumo.ValorUnitario;
					}

					regInsumo.ValorInsumo += valorSomatorio;
				}

				return listaInsumos;

			}
			else
			{
				var listaInsumos = _uowSciex.QueryStackSciex.PEInsumo.ListarPaginadoGrafo(w =>

				new PEInsumoVM()
				{
					IdPEInsumo = w.IdPEInsumo,
					IdPEProduto = w.IdPEProduto,
					CodigoDetalhe = w.CodigoDetalhe,
					CodigoInsumo = w.CodigoInsumo,
					CodigoNcm = w.CodigoNcm,
					CodigoUnidade = w.CodigoUnidade,
					DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
					DescricaoInsumo = w.DescricaoInsumo,
					DescricaoPartNumber = w.DescricaoPartNumber,
					TipoInsumo = w.TipoInsumo,
					ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
					ValorDolar = w.ValorDolar,
					ValorPercentualPerda = w.ValorPercentualPerda
				},
				q => q.IdPEProduto == vm.IdPEProduto && (q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R")), vm);

				return listaInsumos;
			}

			return null;
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

		public PEProdutoVM Selecionar(int idPEProduto)
		{
			var pe = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar<PEProdutoVM>(o => o.IdPEProduto == idPEProduto);
			return pe;
		}

		public PagedItems<PEDetalheInsumoImportadoVM> ListarPaginadoDetalhePorIdInsumoParaAnalise(SalvarDetalheVM vm)
		{
			if (vm == null || vm.IdPEInsumo == 0)
				return null;

			PagedItems<PEDetalheInsumoImportadoVM> listaDetalhes = null;
			List<int?> statusAnalise = new List<int?>()
			{
				null,
				(int)EnumSituacaoAnalisePlanoExportacao.APROVADO,
				(int)EnumSituacaoAnalisePlanoExportacao.REPROVADO,
				(int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO
			};

			string filtroPosterior = null;
			if (vm.Sort != null)
			{
				if (vm.Sort.Equals("ValorTotal"))
				{
					filtroPosterior = vm.Sort;
					vm.Sort = null;
				}
			}


			if (vm.IsQuadroNacional)
			{

				listaDetalhes = _uowSciex.QueryStackSciex.PEDetalheInsumo.ListarPaginadoGrafo(q => new PEDetalheInsumoImportadoVM()
				{
					IdPEDetalheInsumo = q.IdPEDetalheInsumo,
					IdPEInsumo = q.IdPEInsumo,
					IdMoeda = q.IdMoeda,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCRF = q.ValorDolarCRF,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario,
					SituacaoAnalise = q.SituacaoAnalise,
					DescricaoJustificativaErro = q.DescricaoJustificativaErro,
					ValorTotal = q.Quantidade * q.ValorDolar,
					PEInsumo = new PEInsumoVM()
					{
						IdPEInsumo = q.PEInsumo.IdPEInsumo,
						IdPEProduto = q.PEInsumo.IdPEProduto,
						CodigoDetalhe = q.PEInsumo.CodigoDetalhe,
						CodigoInsumo = q.PEInsumo.CodigoInsumo,
						CodigoNcm = q.PEInsumo.CodigoNcm,
						CodigoUnidade = q.PEInsumo.CodigoUnidade,
						DescricaoEspecificacaoTecnica = q.PEInsumo.DescricaoEspecificacaoTecnica,
						DescricaoInsumo = q.PEInsumo.DescricaoInsumo,
						DescricaoPartNumber = q.PEInsumo.DescricaoPartNumber,
						TipoInsumo = q.PEInsumo.TipoInsumo,
						ValorCoeficienteTecnico = q.PEInsumo.ValorCoeficienteTecnico,
						ValorDolar = q.PEInsumo.ValorDolar,
						ValorPercentualPerda = q.PEInsumo.ValorPercentualPerda
					}
				}
				,
				q => q.IdPEInsumo == vm.IdPEInsumo
					&&
					(q.PEInsumo.TipoInsumo.Equals("N") || q.PEInsumo.TipoInsumo.Equals("R"))
					&&
					statusAnalise.Contains(q.SituacaoAnalise)
				,
				vm
				);

			}
			else
			{
				listaDetalhes = _uowSciex.QueryStackSciex.PEDetalheInsumo.ListarPaginadoGrafo(q => new PEDetalheInsumoImportadoVM()
				{
					IdPEDetalheInsumo = q.IdPEDetalheInsumo,
					IdPEInsumo = q.IdPEInsumo,
					IdMoeda = q.IdMoeda,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCRF = q.ValorDolarCRF,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario,
					DescricaoMoeda = q.Moeda.Descricao,
					CodigoMoeda = q.Moeda.CodigoMoeda,
					SituacaoAnalise = q.SituacaoAnalise,
					DescricaoJustificativaErro = q.DescricaoJustificativaErro,
					PEInsumo = new PEInsumoVM()
					{
						IdPEInsumo = q.PEInsumo.IdPEInsumo,
						IdPEProduto = q.PEInsumo.IdPEProduto,
						CodigoDetalhe = q.PEInsumo.CodigoDetalhe,
						CodigoInsumo = q.PEInsumo.CodigoInsumo,
						CodigoNcm = q.PEInsumo.CodigoNcm,
						CodigoUnidade = q.PEInsumo.CodigoUnidade,
						DescricaoEspecificacaoTecnica = q.PEInsumo.DescricaoEspecificacaoTecnica,
						DescricaoInsumo = q.PEInsumo.DescricaoInsumo,
						DescricaoPartNumber = q.PEInsumo.DescricaoPartNumber,
						TipoInsumo = q.PEInsumo.TipoInsumo,
						ValorCoeficienteTecnico = q.PEInsumo.ValorCoeficienteTecnico,
						ValorDolar = q.PEInsumo.ValorDolar,
						ValorPercentualPerda = q.PEInsumo.ValorPercentualPerda
					}
				}
				,
				q => q.IdPEInsumo == vm.IdPEInsumo
					&&
					(q.PEInsumo.TipoInsumo.Equals("P") || q.PEInsumo.TipoInsumo.Equals("E"))
					&&
					statusAnalise.Contains(q.SituacaoAnalise)
				,
				vm);
			}

			if (listaDetalhes.Total > 0)
			{

				foreach (var registroDetalhe in listaDetalhes.Items)
				{
					registroDetalhe.ValorInsumo = registroDetalhe.Quantidade * registroDetalhe.ValorUnitario;
					registroDetalhe.ValorUnitarioFormatada = registroDetalhe.ValorUnitario != 0 ? Convert.ToDecimal(registroDetalhe.ValorUnitario).ToString("N7") : "0";

					registroDetalhe.QuantidadeFormatada = registroDetalhe.Quantidade.ToString("N5");

					if (!vm.IsQuadroNacional)
					{
						var codigoPais = registroDetalhe.CodigoPais.ToString("D3");
						registroDetalhe.DescricaoPais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(q => q.CodigoPais == codigoPais).Descricao;
						registroDetalhe.ValorDolarFOBFormatada = registroDetalhe.ValorDolarFOB != null ? Convert.ToDecimal(registroDetalhe.ValorDolarFOB).ToString("N7") : "0";
						registroDetalhe.ValorFreteFormatada = registroDetalhe.ValorFrete != null ? Convert.ToDecimal(registroDetalhe.ValorFrete).ToString("N7") : "0";

					}

				}
			}

			if (!string.IsNullOrEmpty(filtroPosterior))
			{
				if (filtroPosterior.Equals("ValorTotal"))
				{
					if (!vm.Reverse)
					{
						listaDetalhes.Items = listaDetalhes.Items.OrderBy(q => q.ValorTotal).ToList();
					}
					else
					{
						listaDetalhes.Items = listaDetalhes.Items.OrderByDescending(q => q.ValorTotal).ToList();
					}
				}
			}


			return listaDetalhes;
		}

		public PagedItems<PEDetalheInsumoImportadoVM> ListarPaginadoDetalheAnterioresPorIdInsumoParaAnalise(SalvarDetalheVM vm)
		{
			if (vm == null || vm.IdPEInsumo == 0)
				return null;

			PagedItems<PEDetalheInsumoImportadoVM> listaDetalhesAnteriores = null;
			List<int?> statusAnalise = new List<int?>()
			{
				(int)EnumSituacaoAnalisePlanoExportacao.ALTERADO
			};

			string filtroPosterior = null;
			if (vm.Sort != null)
			{
				if (vm.Sort.Equals("ValorTotal"))
				{
					filtroPosterior = vm.Sort;
					vm.Sort = null;
				}
			}

			if (vm.IsQuadroNacional)
			{

				listaDetalhesAnteriores = _uowSciex.QueryStackSciex.PEDetalheInsumo.ListarPaginadoGrafo(q => new PEDetalheInsumoImportadoVM()
				{
					IdPEDetalheInsumo = q.IdPEDetalheInsumo,
					IdPEInsumo = q.IdPEInsumo,
					IdMoeda = q.IdMoeda,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCRF = q.ValorDolarCRF,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario,
					SituacaoAnalise = q.SituacaoAnalise,
					DescricaoJustificativaErro = q.DescricaoJustificativaErro,
					ValorTotal = q.Quantidade * q.ValorDolar,
					PEInsumo = new PEInsumoVM()
					{
						IdPEInsumo = q.PEInsumo.IdPEInsumo,
						IdPEProduto = q.PEInsumo.IdPEProduto,
						CodigoDetalhe = q.PEInsumo.CodigoDetalhe,
						CodigoInsumo = q.PEInsumo.CodigoInsumo,
						CodigoNcm = q.PEInsumo.CodigoNcm,
						CodigoUnidade = q.PEInsumo.CodigoUnidade,
						DescricaoEspecificacaoTecnica = q.PEInsumo.DescricaoEspecificacaoTecnica,
						DescricaoInsumo = q.PEInsumo.DescricaoInsumo,
						DescricaoPartNumber = q.PEInsumo.DescricaoPartNumber,
						TipoInsumo = q.PEInsumo.TipoInsumo,
						ValorCoeficienteTecnico = q.PEInsumo.ValorCoeficienteTecnico,
						ValorDolar = q.PEInsumo.ValorDolar,
						ValorPercentualPerda = q.PEInsumo.ValorPercentualPerda
					}
				}
				,
				q => q.IdPEInsumo == vm.IdPEInsumo
					&&
					(q.PEInsumo.TipoInsumo.Equals("N") || q.PEInsumo.TipoInsumo.Equals("R"))
					&&
					statusAnalise.Contains(q.SituacaoAnalise)
				,
				vm
				);

			}
			else
			{
				listaDetalhesAnteriores = _uowSciex.QueryStackSciex.PEDetalheInsumo.ListarPaginadoGrafo(q => new PEDetalheInsumoImportadoVM()
				{
					IdPEDetalheInsumo = q.IdPEDetalheInsumo,
					IdPEInsumo = q.IdPEInsumo,
					IdMoeda = q.IdMoeda,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCRF = q.ValorDolarCRF,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario,
					DescricaoMoeda = q.Moeda.Descricao,
					CodigoMoeda = q.Moeda.CodigoMoeda,
					SituacaoAnalise = q.SituacaoAnalise,
					DescricaoJustificativaErro = q.DescricaoJustificativaErro,
					PEInsumo = new PEInsumoVM()
					{
						IdPEInsumo = q.PEInsumo.IdPEInsumo,
						IdPEProduto = q.PEInsumo.IdPEProduto,
						CodigoDetalhe = q.PEInsumo.CodigoDetalhe,
						CodigoInsumo = q.PEInsumo.CodigoInsumo,
						CodigoNcm = q.PEInsumo.CodigoNcm,
						CodigoUnidade = q.PEInsumo.CodigoUnidade,
						DescricaoEspecificacaoTecnica = q.PEInsumo.DescricaoEspecificacaoTecnica,
						DescricaoInsumo = q.PEInsumo.DescricaoInsumo,
						DescricaoPartNumber = q.PEInsumo.DescricaoPartNumber,
						TipoInsumo = q.PEInsumo.TipoInsumo,
						ValorCoeficienteTecnico = q.PEInsumo.ValorCoeficienteTecnico,
						ValorDolar = q.PEInsumo.ValorDolar,
						ValorPercentualPerda = q.PEInsumo.ValorPercentualPerda
					}
				}
				,
				q => q.IdPEInsumo == vm.IdPEInsumo
					&&
					(q.PEInsumo.TipoInsumo.Equals("P") || q.PEInsumo.TipoInsumo.Equals("E"))
					&&
					statusAnalise.Contains(q.SituacaoAnalise)
				,
				vm);
			}

			if (listaDetalhesAnteriores.Total > 0)
			{

				foreach (var registroDetalhe in listaDetalhesAnteriores.Items)
				{
					registroDetalhe.ValorInsumo = registroDetalhe.Quantidade * registroDetalhe.ValorUnitario;
					registroDetalhe.ValorInsumoFormatada = registroDetalhe.ValorInsumo != 0 ? Convert.ToDecimal(registroDetalhe.ValorInsumo).ToString("N7") : "0";

					registroDetalhe.ValorUnitarioFormatada = registroDetalhe.ValorUnitario != 0 ? Convert.ToDecimal(registroDetalhe.ValorUnitario).ToString("N7") : "0";

					registroDetalhe.QuantidadeFormatada = registroDetalhe.Quantidade.ToString("N5");

					if (!vm.IsQuadroNacional)
					{
						var codigoPais = registroDetalhe.CodigoPais.ToString("D3");
						registroDetalhe.DescricaoPais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(q => q.CodigoPais == codigoPais).Descricao;
						registroDetalhe.ValorDolarFOBFormatada = registroDetalhe.ValorDolarFOB != null ? Convert.ToDecimal(registroDetalhe.ValorDolarFOB).ToString("N7") : "0";
						registroDetalhe.ValorFreteFormatada = registroDetalhe.ValorFrete != null ? Convert.ToDecimal(registroDetalhe.ValorFrete).ToString("N7") : "0";
						registroDetalhe.ValorDolarCRFFormatada = registroDetalhe.ValorDolarCRF != null ? Convert.ToDecimal(registroDetalhe.ValorDolarCRF).ToString("N7") : "0";

					}

				}
			}

			if (!string.IsNullOrEmpty(filtroPosterior))
			{
				if (filtroPosterior.Equals("ValorTotal"))
				{
					if (!vm.Reverse)
					{
						listaDetalhesAnteriores.Items = listaDetalhesAnteriores.Items.OrderBy(q => q.ValorTotal).ToList();
					}
					else
					{
						listaDetalhesAnteriores.Items = listaDetalhesAnteriores.Items.OrderByDescending(q => q.ValorTotal).ToList();
					}
				}
			}


			return listaDetalhesAnteriores;
		}


	}
}