using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.BusinessLogic.Pss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Suframa.Sciex.BusinessLogic
{
	public class AnaliseInsumoImportadoBll : IAnaliseInsumoImportadoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;
		private readonly IUsuarioPssBll _usuarioPss;


		public AnaliseInsumoImportadoBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPss)
		{
			_uowSciex = uowSciex;

		}

		public List<PRCInsumoVM> ListarInsumosImportados(BuscarValoresInsumoVM parametros)
		{
			List<PRCInsumoVM> listaValores = new List<PRCInsumoVM>();
			try
			{
				var insumoStatusUm = _uowSciex.QueryStackSciex.PRCInsumo.SelecionarGrafo(w => new PRCInsumoVM()
				{
					IdInsumo = w.IdInsumo,
					StatusInsumo = w.StatusInsumo,
					IdPrcProduto = w.IdPrcProduto,
					CodigoInsumo = w.CodigoInsumo,
					CodigoUnidade = w.CodigoUnidade,
					TipoInsumo = w.TipoInsumo,
					CodigoNCM = w.CodigoNCM,
					DescricaoInsumo = w.DescricaoInsumo,
					ValorPercentualPerda = w.ValorPercentualPerda != null ? w.ValorPercentualPerda : 0,
					CodigoDetalhe = w.CodigoDetalhe,
					DescricaoPartNumber = w.DescricaoPartNumber,
					DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
					ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
					ValorDolarAprovado = w.ValorDolarAprovado == null ? 0 : w.ValorDolarAprovado,
					QuantidadeAprovado = w.QuantidadeAprovado == null ? 0 : w.QuantidadeAprovado,
					ValorNacionalAprovado = w.ValorNacionalAprovado,
					ValorDolarFOBAprovado = w.ValorDolarFOBAprovado == null ? 0 : w.ValorDolarFOBAprovado,
					ValorDolarCFRAprovado = w.ValorDolarCFRAprovado,
					ValorFreteAprovado = w.ValorFreteAprovado == null ? 0 : w.ValorFreteAprovado,
					ValorDolarComp = w.ValorDolarComp == null ? 0 : w.ValorDolarComp,
					QuantidadeComp = w.QuantidadeComp,
					ValorDolarSaldo = w.ValorDolarSaldo == null ? 0 : w.ValorDolarSaldo,
					QuantidadeSaldo = w.QuantidadeSaldo == null ? 0 : w.QuantidadeSaldo,
					ValorAdicional = w.ValorAdicional == null ? 0 : w.ValorAdicional,
					ValorAdicionalFrete = w.ValorAdicionalFrete == null ? 0 : w.ValorAdicionalFrete,
					QuantidadeAdicional = w.QuantidadeAdicional == null ? 0 : w.QuantidadeAdicional,
					DescricaoTipoInsumo = w.TipoInsumo == "N" ? "Nacional" : w.TipoInsumo == "R" ? "Regional" : "Padrão",
					StatusInsumoNovo = w.StatusInsumoNovo,
					Produto = new PRCProdutoVM()
					{
						IdProduto = w.PrcProduto.IdProduto,
						IdProcesso = w.PrcProduto.IdProcesso,
						CodigoProdutoExportacao = w.PrcProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = w.PrcProduto.CodigoProdutoSuframa,
						CodigoNCM = w.PrcProduto.CodigoNCM,
						TipoProduto = w.PrcProduto.TipoProduto,
						DescricaoModelo = w.PrcProduto.DescricaoModelo,
						QuantidadeAprovado = w.PrcProduto.QuantidadeAprovado,
						CodigoUnidade = w.PrcProduto.CodigoUnidade,
						ValorDolarAprovado = w.PrcProduto.ValorDolarAprovado,
						ValorFluxoCaixa = w.PrcProduto.ValorFluxoCaixa
					},
					ListaDetalheInsumos = w.ListaDetalheInsumos.Select(q => new PRCDetalheInsumoVM()
					{
						IdPrcInsumo = q.IdPrcInsumo,
						IdMoeda = q.IdMoeda,
						IdDetalheInsumo = q.IdDetalheInsumo,
						CodigoPais = q.CodigoPais,
						NumeroSequencial = q.NumeroSequencial,
						Quantidade = q.Quantidade,
						ValorDolar = q.ValorDolar,
						ValorDolarCFR = q.ValorDolarCFR,
						ValorDolarFOB = q.ValorDolarFOB,
						ValorFrete = q.ValorFrete,
						ValorUnitario = q.ValorUnitario == null ? 0 : q.ValorUnitario,
						Moeda = new MoedaVM()
						{
							CodigoMoeda = q.Moeda.CodigoMoeda,
							Descricao = q.Moeda.Descricao,
							IdMoeda = q.Moeda.IdMoeda
						}
					}
					).ToList(),
					ListaSolicDetalhe = w.PRCSolicDetalhe.Select(q => new PRCSolicDetalheVM()
					{
						Id = q.Id,
						Status = q.Status,
						DescricaoDe = q.DescricaoDe,
						DescricaoPara = q.DescricaoPara,
						IdInsumo = q.IdInsumo,
						IdDetalheInsumo = q.IdDetalheInsumo,
						IdSolicitacaoAlteracao = q.IdSolicitacaoAlteracao,
						IdTipoSolicitacao = q.IdTipoSolicitacao,
						DescricaoTipoAlteracao = q.TipoSolicAlteracao.Descricao,
						Justificativa = q.Justificativa
					}).ToList(),

				},
				q => q.CodigoInsumo == parametros.codigoInsumo && q.StatusInsumo == 1 && q.IdPrcProduto == parametros.idProduto);

				listaValores.Add(insumoStatusUm);

				var insumoStatusDois = _uowSciex.QueryStackSciex.PRCInsumo.SelecionarGrafo(w => new PRCInsumoVM()
				{
					IdInsumo = w.IdInsumo,
					IdPrcSolicitacaoAlteracao = w.IdPrcSolicitacaoAlteracao,
					IdPrcProduto = w.IdPrcProduto,
					StatusInsumoNovo = w.StatusInsumoNovo,
					StatusInsumo = w.StatusInsumo,
					CodigoInsumo = w.CodigoInsumo,
					CodigoUnidade = w.CodigoUnidade,
					TipoInsumo = w.TipoInsumo,
					CodigoNCM = w.CodigoNCM,
					DescricaoInsumo = w.DescricaoInsumo,
					ValorPercentualPerda = w.ValorPercentualPerda != null ? w.ValorPercentualPerda : 0,
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
					ValorDolarComp = w.ValorDolarComp == null ? 0 : w.ValorDolarComp,
					QuantidadeComp = w.QuantidadeComp,
					ValorDolarSaldo = w.ValorDolarSaldo,
					QuantidadeSaldo = w.QuantidadeSaldo,
					ValorAdicional = w.ValorAdicional == null ? 0 : w.ValorAdicional,
					ValorAdicionalFrete = w.ValorAdicionalFrete == null ? 0 : w.ValorAdicionalFrete,
					QuantidadeAdicional = w.QuantidadeAdicional == null ? 0 : w.QuantidadeAdicional,
					DescricaoTipoInsumo = w.TipoInsumo == "N" ? "Nacional" : w.TipoInsumo == "R" ? "Regional" : "Padrão",
					Produto = new PRCProdutoVM()
					{
						IdProduto = w.PrcProduto.IdProduto,
						IdProcesso = w.PrcProduto.IdProcesso,
						CodigoProdutoExportacao = w.PrcProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = w.PrcProduto.CodigoProdutoSuframa,
						CodigoNCM = w.PrcProduto.CodigoNCM,
						TipoProduto = w.PrcProduto.TipoProduto,
						DescricaoModelo = w.PrcProduto.DescricaoModelo,
						QuantidadeAprovado = w.PrcProduto.QuantidadeAprovado,
						CodigoUnidade = w.PrcProduto.CodigoUnidade,
						ValorDolarAprovado = w.PrcProduto.ValorDolarAprovado,
						ValorFluxoCaixa = w.PrcProduto.ValorFluxoCaixa
					},
					ListaDetalheInsumos = w.ListaDetalheInsumos.Select(q => new PRCDetalheInsumoVM()
					{
						IdPrcInsumo = q.IdPrcInsumo,
						IdMoeda = q.IdMoeda,
						IdDetalheInsumo = q.IdDetalheInsumo,
						CodigoPais = q.CodigoPais,
						NumeroSequencial = q.NumeroSequencial,
						Quantidade = q.Quantidade,
						ValorDolar = q.ValorDolar,
						ValorDolarCFR = q.ValorDolarCFR,
						ValorDolarFOB = q.ValorDolarFOB,
						ValorFrete = q.ValorFrete,
						ValorUnitario = q.ValorUnitario == null ? 0 : q.ValorUnitario,
						Moeda = new MoedaVM()
						{
							CodigoMoeda = q.Moeda.CodigoMoeda,
							Descricao = q.Moeda.Descricao,
							IdMoeda = q.Moeda.IdMoeda
						}
					}
					).ToList(),
					ListaSolicDetalhe = w.PRCSolicDetalhe.Select(q => new PRCSolicDetalheVM()
					{
						Id = q.Id,
						Status = q.Status,
						DescricaoStatus = q.Status == 1 ? "EM ANÁLISE" : q.Status == 2 ? "APROVADO" : "REPROVADO",
						DescricaoDe = q.DescricaoDe,
						DescricaoPara = q.DescricaoPara,
						IdInsumo = q.IdInsumo,
						IdDetalheInsumo = q.IdDetalheInsumo,
						IdSolicitacaoAlteracao = q.IdSolicitacaoAlteracao,
						IdTipoSolicitacao = q.IdTipoSolicitacao,
						DescricaoTipoAlteracao = q.TipoSolicAlteracao.Descricao,
						Justificativa = q.Justificativa,
						TipoSolicAlteracao = new TipoSolicAlteracaoVM()
						{
							Id = q.TipoSolicAlteracao.Id,
							Descricao = q.TipoSolicAlteracao.Descricao
						}
					}).ToList(),
					
				},
				
				q => q.CodigoInsumo == parametros.codigoInsumo && (q.StatusInsumo == 2 || q.StatusInsumo == 3) && q.IdPrcProduto == parametros.idProduto); ;

				foreach (var itemSolicDetalhe in insumoStatusDois.ListaSolicDetalhe )
				{
					if (itemSolicDetalhe.IdTipoSolicitacao == 2 || insumoStatusDois.StatusInsumoNovo == 1)
					{

						itemSolicDetalhe.IsTransferenciaOuInsumoNovo = true;

					}
					else
					{
						itemSolicDetalhe.IsTransferenciaOuInsumoNovo = false;
					}
				}

				
				listaValores.Add(insumoStatusDois);


			return listaValores;
			}
			catch(Exception err)
			{
				return null;
			}
		}

		public decimal? CalcularParidadeValorPara()
		{
			int codigoDolar = 220;
			var dataHoje = DateTime.Now.Date;

			var paridadeValor = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.CodigoMoeda == codigoDolar && q.ParidadeCambial.DataParidade == dataHoje)?.Valor;

			return paridadeValor != null ? paridadeValor : 0;


		}

		public ResultadoMensagemProcessamentoVM AprovarInsumo(PRCSolicDetalheVM filtroDetalheInsumo)
		{
			var retorno = new ResultadoMensagemProcessamentoVM()
			{
				Resultado = true
			};

			var existeParidadeHoje = _uowSciex.QueryStackSciex.ConsultarExistenciaParidadePorData(DateTime.Today);
			
			if (existeParidadeHoje != null)
			{
				int EmAlteracao = 2;
				int aprovada = 2;
				int alteracaoAprovada = 3;

				var regSolicDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Selecionar(q => q.IdDetalheInsumo == filtroDetalheInsumo.IdDetalheInsumo && q.Id == filtroDetalheInsumo.Id) ;

				//foreach (var item in regSolicDetalhe.PrcInsumo.ListaDetalheInsumos)
				//{
				//	item.ValorUnitario = 10;
				//	_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(item);
				//}
				
				////_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(regSolicDetalhe);
				////_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(teste);
				//_uowSciex.CommandStackSciex.Save();
				try
				{
					//Alteração na tabela PRC_SOLIC_DETALHE
					if (regSolicDetalhe != null && regSolicDetalhe.PrcInsumo != null)
					{

						regSolicDetalhe.IdInsumo = filtroDetalheInsumo.IdInsumo;
						regSolicDetalhe.IdDetalheInsumo = filtroDetalheInsumo.IdDetalheInsumo;
						regSolicDetalhe.Status = aprovada;
						regSolicDetalhe.PrcInsumo.StatusInsumo = alteracaoAprovada;

						CalcularNovosValoresPorTipo(regSolicDetalhe);

					}

					else
					{
						retorno.Resultado = false;
						retorno.Mensagem = $"Não foi localizado registro de solicitação detalhe ou insumo associado a esta solicitação.";
						return retorno;
					}

					_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(regSolicDetalhe);
					_uowSciex.CommandStackSciex.Save();

					return retorno;
				}
				catch (Exception err)
				{
					retorno.Resultado = false;
					retorno.Mensagem = $"Exception: {err.Message} / Stack: {err.StackTrace}";
					return retorno;
				} 
			}
			else
			{
				retorno.Resultado = false;
				retorno.CodigoErro = 1;
				retorno.Mensagem = "Não há paridade para o dia de hoje.";
				return retorno;
			}

		}

		public void CalcularNovosValoresPorTipo(PRCSolicDetalheEntity regSolicDetalhe)
		{
			decimal? _qtdSaldoInsumo = 0;
			decimal? _somatorio = 0;
			decimal? _valorDolarAprovado = 0;
			decimal? _valorAdicional = 0;
			decimal? _valorDolarComp = 0;

			try
			{
				switch (regSolicDetalhe.IdTipoSolicitacao)
				{
					case (int)EnumTipoAlteracaoDetalhe.MOEDA:


						_qtdSaldoInsumo = regSolicDetalhe.PrcInsumo.QuantidadeSaldo ?? 0;
						_somatorio = 0;
						foreach (var regDetalhe in regSolicDetalhe.PrcInsumo.ListaDetalheInsumos)
						{

							var idMoeda = regDetalhe.IdMoeda ?? 0;
							var valorUnitario = regDetalhe.ValorUnitario ?? 0;
							var qtdDetalhe = regDetalhe.Quantidade;
							var valorParidade = CalcularParidadeBll.CalcularFatorConversao(regDetalhe.Moeda.CodigoMoeda, _uowSciex);
							var valorFrete = regDetalhe.ValorFrete ?? 0;

							regDetalhe.IdMoeda = idMoeda;
							regDetalhe.ValorDolar = (valorUnitario * qtdDetalhe * valorParidade) + valorFrete;
							regDetalhe.ValorDolarCFR = (valorUnitario * qtdDetalhe * valorParidade) + valorFrete;
							regDetalhe.ValorDolarFOB = (valorUnitario * qtdDetalhe * valorParidade);

							_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(regDetalhe);

							_somatorio += regDetalhe.ValorUnitario * _qtdSaldoInsumo * valorParidade;
						}

						_valorDolarAprovado = regSolicDetalhe.PrcInsumo.ValorDolarAprovado ?? 0;
						//_valorAdicional = regSolicDetalhe.PrcInsumo.ValorAdicional ?? 0;
						_valorDolarComp = regSolicDetalhe.PrcInsumo.ValorDolarComp ?? 0;

						regSolicDetalhe.PrcInsumo.ValorAdicional = _somatorio;
						regSolicDetalhe.PrcInsumo.ValorDolarSaldo = (_valorDolarAprovado + regSolicDetalhe.PrcInsumo.ValorAdicional + regSolicDetalhe.PrcInsumo.ValorAdicionalFrete) - _valorDolarComp;

						_uowSciex.CommandStackSciex.PRCInsumo.Salvar(regSolicDetalhe.PrcInsumo);

						break;

					case (int)EnumTipoAlteracaoDetalhe.QUANTIDADE:

						var qtdAdicional = regSolicDetalhe.PrcInsumo.QuantidadeAdicional ?? 0;
						var valorDolarAprovado = regSolicDetalhe.PrcInsumo.ValorDolarAprovado ?? 0;
						_somatorio = 0;

						foreach (var regDetalhe in regSolicDetalhe.PrcInsumo.ListaDetalheInsumos)
						{
							var qtd = regDetalhe.Quantidade;
							var valorUnitario = regDetalhe.ValorUnitario ?? 0;
							var qtdDetalhe = regDetalhe.Quantidade;
							var valorParidade = CalcularParidadeBll.CalcularFatorConversao(regDetalhe.Moeda.CodigoMoeda, _uowSciex);
							var valorFrete = regDetalhe.ValorFrete ?? 0;

							regDetalhe.Quantidade = qtd;
							regDetalhe.ValorDolar = (valorUnitario * qtdDetalhe * valorParidade) + valorFrete;
							regDetalhe.ValorDolarCFR = (valorUnitario * qtdDetalhe * valorParidade) + valorFrete;
							regDetalhe.ValorDolarFOB = (valorUnitario * qtdDetalhe * valorParidade);

							_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(regDetalhe);

							_somatorio += (regDetalhe.ValorUnitario * qtdAdicional * valorParidade);
						}


						//var valorAdicional = regSolicDetalhe.PrcInsumo.ValorAdicional ?? 0;
						var valorDolarComp = regSolicDetalhe.PrcInsumo.ValorDolarComp ?? 0;
						var quantidadeAprovado = regSolicDetalhe.PrcInsumo.QuantidadeAprovado ?? 0;
						var quantidadeAdicional = regSolicDetalhe.PrcInsumo.QuantidadeAdicional ?? 0;
						var quantidadeComprovado = regSolicDetalhe.PrcInsumo.QuantidadeComp ?? 0;

						regSolicDetalhe.PrcInsumo.ValorAdicional = _somatorio;
						regSolicDetalhe.PrcInsumo.ValorDolarSaldo = (valorDolarAprovado + regSolicDetalhe.PrcInsumo.ValorAdicional + regSolicDetalhe.PrcInsumo.ValorAdicionalFrete) - valorDolarComp;
						regSolicDetalhe.PrcInsumo.QuantidadeSaldo = (quantidadeAprovado + quantidadeAdicional) - quantidadeComprovado;
						_uowSciex.CommandStackSciex.PRCInsumo.Salvar(regSolicDetalhe.PrcInsumo);

						break;

					case (int)EnumTipoAlteracaoDetalhe.VALOR_UNITÁRIO:

						_qtdSaldoInsumo = regSolicDetalhe.PrcInsumo.QuantidadeSaldo ?? 0;
						_somatorio = 0;
						foreach (var regDetalhe in regSolicDetalhe.PrcInsumo.ListaDetalheInsumos)
						{

							var valorUnitario = regDetalhe.ValorUnitario ?? 0;
							var qtdDetalhe = regDetalhe.Quantidade;
							var valorParidade = CalcularParidadeBll.CalcularFatorConversao(regDetalhe.Moeda.CodigoMoeda, _uowSciex);
							var valorFrete = regDetalhe.ValorFrete ?? 0;

							regDetalhe.ValorUnitario = valorUnitario;
							regDetalhe.ValorDolar = (valorUnitario * qtdDetalhe * valorParidade) + valorFrete;
							regDetalhe.ValorDolarCFR = (valorUnitario * qtdDetalhe * valorParidade) + valorFrete;
							regDetalhe.ValorDolarFOB = (valorUnitario * qtdDetalhe * valorParidade);
							regDetalhe.ValorUnitarioCFR = (regDetalhe.ValorDolarCFR / valorParidade) / qtdDetalhe;

							_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(regDetalhe);

							_somatorio += regDetalhe.ValorUnitario * _qtdSaldoInsumo * valorParidade;
						}

						_valorDolarAprovado = regSolicDetalhe.PrcInsumo.ValorDolarAprovado ?? 0;
						//_valorAdicional = regSolicDetalhe.PrcInsumo.ValorAdicional ?? 0;
						_valorDolarComp = regSolicDetalhe.PrcInsumo.ValorDolarComp ?? 0;

						regSolicDetalhe.PrcInsumo.ValorAdicional = _somatorio;
						regSolicDetalhe.PrcInsumo.ValorDolarSaldo = (_valorDolarAprovado + regSolicDetalhe.PrcInsumo.ValorAdicional + regSolicDetalhe.PrcInsumo.ValorAdicionalFrete) - _valorDolarComp;
						_uowSciex.CommandStackSciex.PRCInsumo.Salvar(regSolicDetalhe.PrcInsumo);

						break;

					case (int)EnumTipoAlteracaoDetalhe.VALOR_FRETE:

						_qtdSaldoInsumo = regSolicDetalhe.PrcInsumo.QuantidadeSaldo ?? 0;
						_somatorio = 0;
						decimal? _acrescimo = 0;
						foreach (var regDetalhe in regSolicDetalhe.PrcInsumo.ListaDetalheInsumos)
						{

							decimal? valorFrete = regDetalhe.ValorFrete ?? 0;
							_acrescimo = regDetalhe.ValorFrete - ((regDetalhe.PrcInsumo.ValorFreteAprovado ?? 0) + (regDetalhe.PrcInsumo.ValorAdicionalFrete ?? 0));
							_somatorio += _acrescimo;
							var qtdDetalhe = regDetalhe.Quantidade;
							var valorParidade = CalcularParidadeBll.CalcularFatorConversao(regDetalhe.Moeda.CodigoMoeda, _uowSciex);
							var valorDolar = regDetalhe.ValorDolar ?? 0;

							regDetalhe.ValorFrete = valorFrete;
							regDetalhe.ValorDolar = valorDolar + _acrescimo;
							regDetalhe.ValorDolarCFR = valorDolar + valorFrete + _acrescimo;
							regDetalhe.ValorUnitarioCFR = (regDetalhe.ValorDolarCFR / valorParidade) / qtdDetalhe;

							_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(regDetalhe);

						}

						regSolicDetalhe.PrcInsumo.ValorAdicionalFrete += _acrescimo;
						regSolicDetalhe.PrcInsumo.ValorDolarSaldo += _acrescimo;

						_uowSciex.CommandStackSciex.PRCInsumo.Salvar(regSolicDetalhe.PrcInsumo);

						break;
				}
			}
			catch (Exception e)
			{

				throw new Exception(e.Message+" - "+e.StackTrace);
			}

		}

		enum  EnumTipoAlteracaoDetalhe
		{	
			PAÍS=3,
			MOEDA=4,
			QUANTIDADE=5,
			VALOR_UNITÁRIO=6,
			VALOR_FRETE=7
		}

		public ResultadoMensagemProcessamentoVM ReprovarInsumo(PRCSolicDetalheVM filtro)
		{

			var retorno = new ResultadoMensagemProcessamentoVM()
			{
				Resultado = true
			};
			
			var registroPrcInsumo = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(x => x.IdInsumo == filtro.IdInsumo);

			var codigoInsumo = registroPrcInsumo.CodigoInsumo;
			var IdSolicitacao = registroPrcInsumo.IdPrcSolicitacaoAlteracao;
			int StatusSolicDetalheEmAnalise = 1;
			int StatusInsumoAtivo = 1;
			int StatusAlteracao = 2;

			var registroReprovacao = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Selecionar(x => x.IdInsumo == filtro.IdInsumo
			&& x.IdSolicitacaoAlteracao == IdSolicitacao 
			&& x.IdTipoSolicitacao == filtro.IdTipoSolicitacao);
			
			var registroInsumoAtivos = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(x => x.CodigoInsumo== codigoInsumo 
			&& x.StatusInsumo == StatusInsumoAtivo
			&& x.IdPrcProduto == registroPrcInsumo.IdPrcProduto );
			
			var registroInsumoAlteracao = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(x => x.CodigoInsumo == codigoInsumo 
			&& x.StatusInsumo == StatusAlteracao 
			&& x.IdPrcProduto == registroPrcInsumo.IdPrcProduto);

		

			var registroDetalheAtivo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar(x => x.IdPrcInsumo ==registroInsumoAtivos.IdInsumo);
			var registroDetalheAlteracao = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar(x => x.IdDetalheInsumo == registroReprovacao.IdDetalheInsumo);


			try
			{
				int Reprovado = 3;
				if (registroReprovacao != null)
				{

					registroReprovacao.IdInsumo = filtro.IdInsumo;
					registroReprovacao.IdDetalheInsumo = filtro.IdDetalheInsumo;
					registroReprovacao.Status = Reprovado;
					registroReprovacao.Justificativa = filtro.Justificativa;

					switch (registroReprovacao.IdTipoSolicitacao)
					{
						case (int) EnumTipoAlteracaoDetalhe.PAÍS:
							registroDetalheAlteracao.CodigoPais = registroDetalheAtivo.CodigoPais;
							break;
						case (int)EnumTipoAlteracaoDetalhe.MOEDA:
							registroDetalheAlteracao.IdMoeda = registroDetalheAtivo.IdMoeda;
							break;
						case (int)EnumTipoAlteracaoDetalhe.QUANTIDADE:
							registroDetalheAlteracao.Quantidade = registroDetalheAtivo.Quantidade;
							break;
						case (int)EnumTipoAlteracaoDetalhe.VALOR_UNITÁRIO:
							registroDetalheAlteracao.ValorUnitario = registroDetalheAtivo.ValorUnitario;
							break;
						case (int)EnumTipoAlteracaoDetalhe.VALOR_FRETE:
							registroDetalheAlteracao.ValorFrete = registroDetalheAtivo.ValorFrete;
							break;
					
					}
					//CalcularNovosValoresPorTipo(ref registroReprovacao);

					_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(registroReprovacao);
					_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(registroDetalheAlteracao);
					_uowSciex.CommandStackSciex.Save();

					return retorno;
				}
				else
				{
					retorno.Resultado = false;
					retorno.Mensagem = $"Não foi localizado registro de solicitação detalhe ou insumo associado a esta solicitação.";
					return retorno;
				}
			}
			catch (Exception err)
			{
				retorno.Resultado = false;
				retorno.Mensagem = $"Exception: {err.Message} / Stack: {err.StackTrace}";
				return retorno;
			}

		}
	}



}