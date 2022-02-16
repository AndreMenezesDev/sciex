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
	public class ProcessoInsumoSuframaBll : IProcessoInsumoSuframaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;

		public ProcessoInsumoSuframaBll(
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

		public PagedItems<PRCInsumoVM> ListarProcessoInsumosNacionalOuImportadoPorIdProcessoProduto(ListarProcessoInsumosNacionalImportadosVM vm)
		{

			if (vm == null || vm.IdProcessoProduto == 0)
				return null;

			PagedItems<PRCInsumoVM> listaProcessoInsumosNacionaisOuImportados;
			if (vm.isQuadroNacional)
			{
				listaProcessoInsumosNacionaisOuImportados = _uowSciex.QueryStackSciex.PRCInsumo.ListarPaginadoGrafo(q => new PRCInsumoVM()
				{
					IdInsumo = q.IdInsumo,
					IdPrcProduto = q.IdPrcProduto,
					CodigoInsumo = q.CodigoInsumo,
					CodigoUnidade = q.CodigoUnidade,
					TipoInsumo = q.TipoInsumo,
					CodigoNCM = q.CodigoNCM,
					DescricaoInsumo = q.DescricaoInsumo,
					ValorPercentualPerda = q.ValorPercentualPerda,
					CodigoDetalhe = q.CodigoDetalhe,
					DescricaoPartNumber = q.DescricaoPartNumber,
					DescricaoEspecificacaoTecnica = q.DescricaoEspecificacaoTecnica,
					ValorCoeficienteTecnico = q.ValorCoeficienteTecnico,
					ValorDolarAprovado = q.ValorDolarAprovado,
					QuantidadeAprovado = q.QuantidadeAprovado,
					ValorNacionalAprovado = q.ValorNacionalAprovado,
					ValorDolarFOBAprovado = q.ValorDolarFOBAprovado,
					ValorDolarCFRAprovado = q.ValorDolarCFRAprovado,
					ValorFreteAprovado = q.ValorFreteAprovado,
					ValorDolarComp = q.ValorDolarComp,
					QuantidadeComp = q.QuantidadeComp,
					ValorDolarSaldo = q.ValorDolarSaldo,
					QuantidadeSaldo = q.QuantidadeSaldo,
					DescricaoTipoInsumo = q.TipoInsumo == "N" ? "Nacional" : q.TipoInsumo == "R" ? "Regional" : "Padrão",
					Produto = new PRCProdutoVM()
					{
						IdProduto = q.PrcProduto.IdProduto,
						IdProcesso = q.PrcProduto.IdProcesso,
						CodigoProdutoExportacao = q.PrcProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = q.PrcProduto.CodigoProdutoSuframa,
						CodigoNCM = q.PrcProduto.CodigoNCM,
						TipoProduto = q.PrcProduto.TipoProduto,
						DescricaoModelo = q.PrcProduto.DescricaoModelo,
						QuantidadeAprovado = q.PrcProduto.QuantidadeAprovado,
						CodigoUnidade = q.PrcProduto.CodigoUnidade,
						ValorDolarAprovado = q.PrcProduto.ValorDolarAprovado,
						ValorFluxoCaixa = q.PrcProduto.ValorFluxoCaixa
					}
				}
				,
				q=>
				q.IdPrcProduto == vm.IdProduto
				&&
				(q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R"))
				,
				vm
				);
			}
			else
			{
				if (vm.ExisteSolicAlteracaoEmAnalise)
				{
					string codigoNCM = null;

					if (vm.IdNcm != null)
					{
						codigoNCM = _uowSciex.QueryStackSciex.ViewNcm.Selecionar(o => o.IdNcm == vm.IdNcm).CodigoNCM;
					}

					listaProcessoInsumosNacionaisOuImportados = _uowSciex.QueryStackSciex.PRCInsumo.ListarPaginadoGrafo(q => new PRCInsumoVM()
					{
						IdInsumo = q.IdInsumo,
						IdPrcProduto = q.IdPrcProduto,
						IdPrcSolicitacaoAlteracao = q.IdPrcSolicitacaoAlteracao,
						StatusInsumo = q.StatusInsumo,
						StatusInsumoNovo = q.StatusInsumoNovo,
						CodigoInsumo = q.CodigoInsumo,
						CodigoUnidade = q.CodigoUnidade,
						TipoInsumo = q.TipoInsumo,
						CodigoNCM = q.CodigoNCM,
						DescricaoInsumo = q.DescricaoInsumo,
						ValorPercentualPerda = q.ValorPercentualPerda,
						CodigoDetalhe = q.CodigoDetalhe,
						DescricaoPartNumber = q.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.ValorDolarAprovado,
						QuantidadeAprovado = q.QuantidadeAprovado,
						ValorNacionalAprovado = q.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.ValorFreteAprovado,
						ValorDolarComp = q.ValorDolarComp,
						QuantidadeComp = q.QuantidadeComp,
						ValorDolarSaldo = q.ValorDolarSaldo,
						QuantidadeSaldo = q.QuantidadeSaldo,

						DescricaoTipoInsumo = q.TipoInsumo == "N" ? "Nacional" : q.TipoInsumo == "R" ? "Regional" : "Padrão",
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcProduto.IdProduto,
							IdProcesso = q.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcProduto.ValorFluxoCaixa
						},
						PrcSolicitacaoAlteracao = new PRCSolicitacaoAlteracaoVM()
						{
							Id = q.PrcSolicitacaoAlteracao.Id,
							IdProcesso = q.PrcSolicitacaoAlteracao.IdProcesso,
							
							Status = q.PrcSolicitacaoAlteracao.Status,
							ListaSolicDetalhe = q.PRCSolicDetalhe.Select(o => new PRCSolicDetalheVM()
							{
								Id = o.Id,
								IdInsumo = o.IdInsumo,
								IdDetalheInsumo = o.IdDetalheInsumo,
								IdSolicitacaoAlteracao = o.IdSolicitacaoAlteracao,
								IdTipoSolicitacao = o.IdTipoSolicitacao,
								Status = o.Status,
								TipoSolicAlteracao = new TipoSolicAlteracaoVM()
								{
									Id = o.TipoSolicAlteracao.Id,
									Descricao = o.TipoSolicAlteracao.Descricao
								}
							}).ToList()
						},
					},
					q =>
					q.IdPrcProduto == vm.IdProduto
					&&
					(q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E"))
					&&
					(vm.IdSolicitacaoAnalise == null || vm.IdSolicitacaoAnalise == q.PrcSolicitacaoAlteracao.Id)
					&&
					(vm.CodigoInsumo == null || vm.CodigoInsumo == q.CodigoInsumo)
					&&
					(string.IsNullOrEmpty(codigoNCM) || codigoNCM == q.CodigoNCM)
					&&
					(
						(vm.TipoStatusAnalise == null) ||
						(
							(vm.TipoStatusAnalise == 1 && q.PrcSolicitacaoAlteracao.ListaSolicDetalhe.Any(w => w.Status == 1)) ||
							(vm.TipoStatusAnalise == 2 && q.PrcSolicitacaoAlteracao.ListaSolicDetalhe.Any(w => w.Status != 1))
						)
					)
					&&
					(
						(vm.TipoAlteracao == null) ||
						(
							(vm.TipoAlteracao == 1 && q.PrcSolicitacaoAlteracao.ListaSolicDetalhe.Any(w => w.TipoSolicAlteracao.Id == 1)) ||
							(vm.TipoAlteracao == 2 && q.PrcSolicitacaoAlteracao.ListaSolicDetalhe.Any(w => w.TipoSolicAlteracao.Id == 2))
						)
					)
					,
					vm
					); 
				}
				else
				{
					var a = _uowSciex.QueryStackSciex.ConsultarExistenciaParidadePorData(DateTime.Today);

					listaProcessoInsumosNacionaisOuImportados = _uowSciex.QueryStackSciex.PRCInsumo.ListarPaginadoGrafo(q => new PRCInsumoVM()
					{
						IdInsumo = q.IdInsumo,
						IdPrcProduto = q.IdPrcProduto,
						CodigoInsumo = q.CodigoInsumo,
						StatusInsumo = q.StatusInsumo,

						CodigoUnidade = q.CodigoUnidade,
						TipoInsumo = q.TipoInsumo,
						CodigoNCM = q.CodigoNCM,
						DescricaoInsumo = q.DescricaoInsumo,
						ValorPercentualPerda = q.ValorPercentualPerda,
						CodigoDetalhe = q.CodigoDetalhe,
						DescricaoPartNumber = q.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.ValorDolarAprovado,
						QuantidadeAprovado = q.QuantidadeAprovado,
						ValorNacionalAprovado = q.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.ValorFreteAprovado,
						ValorDolarComp = q.ValorDolarComp,
						QuantidadeComp = q.QuantidadeComp,
						ValorDolarSaldo = q.ValorDolarSaldo,
						QuantidadeSaldo = q.QuantidadeSaldo,
						DescricaoTipoInsumo = q.TipoInsumo == "N" ? "Nacional" : q.TipoInsumo == "R" ? "Regional" : "Padrão",
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcProduto.IdProduto,
							IdProcesso = q.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcProduto.ValorFluxoCaixa
						}
					},
					q =>
					q.IdPrcProduto == vm.IdProduto
					&&
					(q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E"))
					,
					vm
					);
				}
			}

			foreach (var insumo in listaProcessoInsumosNacionaisOuImportados.Items)
			{
				var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == insumo.CodigoUnidade);
				insumo.DescCodigoUnidade = undMed != null ? undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao : "-";

				var qtdProduto = insumo?.Produto?.QuantidadeAprovado ?? 0;
				var coefTec = insumo?.ValorCoeficienteTecnico ?? 0;
				var percPerda = insumo?.ValorPercentualPerda ?? 0;

				insumo.QtdMaxInsumo = qtdProduto * coefTec;

				

				insumo.QtdMaxInsumo = insumo.QtdMaxInsumo + (insumo.QtdMaxInsumo * (percPerda / 100));


				if (vm.ExisteSolicAlteracaoEmAnalise)
				{
					var existeAnalisePendente = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Existe(q => q.IdInsumo == insumo.IdInsumo
																												&&
																												q.IdSolicitacaoAlteracao == vm.IdSolicitacaoAnalise
																												&&
																												q.Status == 1 //1-EM ANALISE
																												);
					insumo.StatusAnalise = existeAnalisePendente ? "Pendente" : "Concluído";

					insumo.TipoTransferencia = insumo.PrcSolicitacaoAlteracao.ListaSolicDetalhe.Exists(o => o.IdTipoSolicitacao == 2);

					if (insumo.TipoTransferencia || insumo.StatusInsumoNovo == 1)
					{
						insumo.ExibirBotaoAprovarReprovar = true;

					}
					else
					{
						insumo.ExibirBotaoAprovarReprovar = false;
					} 
				}
			}

			return listaProcessoInsumosNacionaisOuImportados;
		}

		public PRCProdutoVM BuscarInformacoesAdicionaisProduto(PRCProdutoVM vm)
		{
			if (vm == null )
				return null;	
			var vmRetorno = new PRCProdutoVM();

			var codigoUnidadeMedida = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == vm.CodigoUnidade);
			if (codigoUnidadeMedida != null)
			{
				vmRetorno.DescricaoUnidadeMedida = codigoUnidadeMedida.Descricao;
			}

			return vmRetorno;

		}

		public PagedItems<PRCDetalheInsumoVM> ListarProcessoInsumosNacionalOuImportadoPorIdInsumo(ListarProcessoInsumosNacionalImportadosVM vm)
		{

			if (vm == null || vm.IdProcessoInsumo == 0)
				return null;

			string filtroPosterior = null;

			if (vm.Sort != null)
			{
				if (vm.Sort.Equals("ValorTotal") || vm.Sort.Equals("CodigoPais") || vm.Sort.Equals("CodigoDescricaoMoeda"))
				{
					filtroPosterior = vm.Sort;
					vm.Sort = null;
				}
			}

			PagedItems<PRCDetalheInsumoVM> listaCodigoInsumosIncluidos;
			if (vm.isQuadroNacional)
			{

				listaCodigoInsumosIncluidos = _uowSciex.QueryStackSciex.PRCDetalheInsumo.ListarPaginadoGrafo(q => new PRCDetalheInsumoVM()
				{
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					NumeroSequencial = q.NumeroSequencial,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorUnitario = q.ValorUnitario,
					ValorFrete = q.ValorFrete,
					ValorDolar = q.ValorDolar,
					ValorUnitarioCFR = q.ValorUnitarioCFR,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					PrcInsumo = new PRCInsumoVM()
					{
						IdInsumo = q.PrcInsumo.IdInsumo,
						IdPrcProduto = q.PrcInsumo.IdPrcProduto,
						CodigoInsumo = q.PrcInsumo.CodigoInsumo,
						CodigoUnidade = q.PrcInsumo.CodigoUnidade,
						TipoInsumo = q.PrcInsumo.TipoInsumo,
						CodigoNCM = q.PrcInsumo.CodigoNCM,
						DescricaoInsumo = q.PrcInsumo.DescricaoInsumo,
						ValorPercentualPerda = q.PrcInsumo.ValorPercentualPerda,
						CodigoDetalhe = q.PrcInsumo.CodigoDetalhe,
						DescricaoPartNumber = q.PrcInsumo.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.PrcInsumo.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.PrcInsumo.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.PrcInsumo.ValorDolarAprovado,
						QuantidadeAprovado = q.PrcInsumo.QuantidadeAprovado,
						ValorNacionalAprovado = q.PrcInsumo.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.PrcInsumo.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.PrcInsumo.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.PrcInsumo.ValorFreteAprovado,
						ValorDolarComp = q.PrcInsumo.ValorDolarComp,
						QuantidadeComp = q.PrcInsumo.QuantidadeComp,
						ValorDolarSaldo = q.PrcInsumo.ValorDolarSaldo,
						QuantidadeSaldo = q.PrcInsumo.QuantidadeSaldo,
						DescricaoTipoInsumo = q.PrcInsumo.TipoInsumo == "N" ? "Nacional" : q.PrcInsumo.TipoInsumo == "R" ? "Regional" : "Padrão" ,
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcInsumo.PrcProduto.IdProduto,
							IdProcesso = q.PrcInsumo.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcInsumo.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcInsumo.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcInsumo.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcInsumo.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcInsumo.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcInsumo.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcInsumo.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcInsumo.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcInsumo.PrcProduto.ValorFluxoCaixa
						}
					}
				}
				,
				q =>
				q.IdPrcInsumo == vm.IdProcessoInsumo
				&&
				(q.PrcInsumo.TipoInsumo.Equals("N") || q.PrcInsumo.TipoInsumo.Equals("R"))
				,
				vm
				);

				foreach (var item in listaCodigoInsumosIncluidos.Items)
				{
					var valorUnitario = item.ValorUnitario != null ? item.ValorUnitario : 0;
					item.ValorTotal = item.Quantidade * valorUnitario;
				}

			}
			else
			{

				listaCodigoInsumosIncluidos = _uowSciex.QueryStackSciex.PRCDetalheInsumo.ListarPaginadoGrafo(q => new PRCDetalheInsumoVM()
				{
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					NumeroSequencial = q.NumeroSequencial,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorUnitario = q.ValorUnitario,
					ValorFrete = q.ValorFrete,
					ValorDolar = q.ValorDolar,
					ValorUnitarioCFR = q.ValorUnitarioCFR,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					PrcInsumo = new PRCInsumoVM()
					{
						IdInsumo = q.PrcInsumo.IdInsumo,
						IdPrcProduto = q.PrcInsumo.IdPrcProduto,
						CodigoInsumo = q.PrcInsumo.CodigoInsumo,
						CodigoUnidade = q.PrcInsumo.CodigoUnidade,
						TipoInsumo = q.PrcInsumo.TipoInsumo,
						StatusInsumo  = q.PrcInsumo.StatusInsumo,
						CodigoNCM = q.PrcInsumo.CodigoNCM,
						DescricaoInsumo = q.PrcInsumo.DescricaoInsumo,
						ValorPercentualPerda = q.PrcInsumo.ValorPercentualPerda,
						CodigoDetalhe = q.PrcInsumo.CodigoDetalhe,
						DescricaoPartNumber = q.PrcInsumo.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.PrcInsumo.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.PrcInsumo.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.PrcInsumo.ValorDolarAprovado,
						QuantidadeAprovado = q.PrcInsumo.QuantidadeAprovado,
						ValorNacionalAprovado = q.PrcInsumo.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.PrcInsumo.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.PrcInsumo.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.PrcInsumo.ValorFreteAprovado,
						ValorDolarComp = q.PrcInsumo.ValorDolarComp,
						QuantidadeComp = q.PrcInsumo.QuantidadeComp,
						ValorDolarSaldo = q.PrcInsumo.ValorDolarSaldo,
						QuantidadeSaldo = q.PrcInsumo.QuantidadeSaldo,
						DescricaoTipoInsumo = q.PrcInsumo.TipoInsumo == "N" ? "Nacional" : q.PrcInsumo.TipoInsumo == "R" ? "Regional" : "Padrão",
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcInsumo.PrcProduto.IdProduto,
							IdProcesso = q.PrcInsumo.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcInsumo.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcInsumo.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcInsumo.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcInsumo.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcInsumo.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcInsumo.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcInsumo.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcInsumo.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcInsumo.PrcProduto.ValorFluxoCaixa
						}
					}
				}
				,
				q =>
				q.IdPrcInsumo == vm.IdProcessoInsumo
				&&
				((q.PrcInsumo.TipoInsumo.Equals("P") || q.PrcInsumo.TipoInsumo.Equals("E")) && q.PrcInsumo.StatusInsumo == 1)
				,
				vm
				);

				
			}

			foreach (var item in listaCodigoInsumosIncluidos.Items)
			{
				string codigoPais = Convert.ToInt32(item.CodigoPais).ToString("D3");
				if(codigoPais != "000")
				{
					var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

					item.DescricaoPais = pais.Descricao;
				}


				var regMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == item.IdMoeda);

				item.CodigoDescricaoMoeda = regMoeda.CodigoMoeda + " | " + regMoeda.Descricao;
			}

			if (!string.IsNullOrEmpty(filtroPosterior))
			{
				if (filtroPosterior.Equals("ValorTotal"))
				{
					if (!vm.Reverse)
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderBy(q => q.ValorTotal).ToList();
					}
					else
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderByDescending(q => q.ValorTotal).ToList();
					} 
				}
				else if (filtroPosterior.Equals("CodigoPais"))
				{
					if (!vm.Reverse)
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderBy(q => q.CodigoPais).ToList();
					}
					else
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderByDescending(q => q.CodigoPais).ToList();
					}
				}
				else if (filtroPosterior.Equals("CodigoDescricaoMoeda"))
				{
					if (!vm.Reverse)
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderBy(q => q.CodigoDescricaoMoeda).ToList();
					}
					else
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderByDescending(q => q.CodigoDescricaoMoeda).ToList();
					}
				}
			}

			return listaCodigoInsumosIncluidos;
		}

		public PagedItems<PRCDetalheInsumoVM> ListarDetalhesImportadosPorIdInsumoParaAnalise(ListarProcessoInsumosNacionalImportadosVM vm)
		{

			if (vm == null || vm.IdProcessoInsumo == 0)
				return null;

			string filtroPosterior = null;

			if (vm.Sort != null)
			{
				if (vm.Sort.Equals("ValorTotal") || vm.Sort.Equals("CodigoPais") || vm.Sort.Equals("CodigoDescricaoMoeda"))
				{
					filtroPosterior = vm.Sort;
					vm.Sort = null;
				}
			}

			PagedItems<PRCDetalheInsumoVM> listaCodigoInsumosIncluidos;
			if (vm.isQuadroNacional)
			{

				listaCodigoInsumosIncluidos = _uowSciex.QueryStackSciex.PRCDetalheInsumo.ListarPaginadoGrafo(q => new PRCDetalheInsumoVM()
				{
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					NumeroSequencial = q.NumeroSequencial,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorUnitario = q.ValorUnitario,
					ValorFrete = q.ValorFrete,
					ValorDolar = q.ValorDolar,
					ValorUnitarioCFR = q.ValorUnitarioCFR,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					PrcInsumo = new PRCInsumoVM()
					{
						IdInsumo = q.PrcInsumo.IdInsumo,
						IdPrcProduto = q.PrcInsumo.IdPrcProduto,
						CodigoInsumo = q.PrcInsumo.CodigoInsumo,
						CodigoUnidade = q.PrcInsumo.CodigoUnidade,
						TipoInsumo = q.PrcInsumo.TipoInsumo,
						CodigoNCM = q.PrcInsumo.CodigoNCM,
						DescricaoInsumo = q.PrcInsumo.DescricaoInsumo,
						ValorPercentualPerda = q.PrcInsumo.ValorPercentualPerda,
						CodigoDetalhe = q.PrcInsumo.CodigoDetalhe,
						DescricaoPartNumber = q.PrcInsumo.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.PrcInsumo.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.PrcInsumo.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.PrcInsumo.ValorDolarAprovado,
						QuantidadeAprovado = q.PrcInsumo.QuantidadeAprovado,
						ValorNacionalAprovado = q.PrcInsumo.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.PrcInsumo.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.PrcInsumo.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.PrcInsumo.ValorFreteAprovado,
						ValorDolarComp = q.PrcInsumo.ValorDolarComp,
						QuantidadeComp = q.PrcInsumo.QuantidadeComp,
						ValorDolarSaldo = q.PrcInsumo.ValorDolarSaldo,
						QuantidadeSaldo = q.PrcInsumo.QuantidadeSaldo,
						DescricaoTipoInsumo = q.PrcInsumo.TipoInsumo == "N" ? "Nacional" : q.PrcInsumo.TipoInsumo == "R" ? "Regional" : "Padrão",
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcInsumo.PrcProduto.IdProduto,
							IdProcesso = q.PrcInsumo.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcInsumo.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcInsumo.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcInsumo.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcInsumo.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcInsumo.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcInsumo.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcInsumo.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcInsumo.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcInsumo.PrcProduto.ValorFluxoCaixa
						}
					}
				}
				,
				q =>
				q.IdPrcInsumo == vm.IdProcessoInsumo
				&&
				(q.PrcInsumo.TipoInsumo.Equals("N") || q.PrcInsumo.TipoInsumo.Equals("R"))
				,
				vm
				);

				foreach (var item in listaCodigoInsumosIncluidos.Items)
				{
					var valorUnitario = item.ValorUnitario != null ? item.ValorUnitario : 0;
					item.ValorTotal = item.Quantidade * valorUnitario;
				}

			}
			else
			{

				listaCodigoInsumosIncluidos = _uowSciex.QueryStackSciex.PRCDetalheInsumo.ListarPaginadoGrafo(q => new PRCDetalheInsumoVM()
				{
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					NumeroSequencial = q.NumeroSequencial,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorUnitario = q.ValorUnitario,
					ValorFrete = q.ValorFrete,
					ValorDolar = q.ValorDolar,
					ValorUnitarioCFR = q.ValorUnitarioCFR,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					PrcInsumo = new PRCInsumoVM()
					{
						IdInsumo = q.PrcInsumo.IdInsumo,
						IdPrcSolicitacaoAlteracao= q.PrcInsumo.IdPrcSolicitacaoAlteracao,
						IdPrcProduto = q.PrcInsumo.IdPrcProduto,
						CodigoInsumo = q.PrcInsumo.CodigoInsumo,
						CodigoUnidade = q.PrcInsumo.CodigoUnidade,
						TipoInsumo = q.PrcInsumo.TipoInsumo,
						StatusInsumo = q.PrcInsumo.StatusInsumo,
						CodigoNCM = q.PrcInsumo.CodigoNCM,
						DescricaoInsumo = q.PrcInsumo.DescricaoInsumo,
						ValorPercentualPerda = q.PrcInsumo.ValorPercentualPerda,
						CodigoDetalhe = q.PrcInsumo.CodigoDetalhe,
						DescricaoPartNumber = q.PrcInsumo.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.PrcInsumo.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.PrcInsumo.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.PrcInsumo.ValorDolarAprovado,
						QuantidadeAprovado = q.PrcInsumo.QuantidadeAprovado,
						ValorNacionalAprovado = q.PrcInsumo.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.PrcInsumo.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.PrcInsumo.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.PrcInsumo.ValorFreteAprovado,
						ValorDolarComp = q.PrcInsumo.ValorDolarComp,
						QuantidadeComp = q.PrcInsumo.QuantidadeComp,
						ValorDolarSaldo = q.PrcInsumo.ValorDolarSaldo,
						QuantidadeSaldo = q.PrcInsumo.QuantidadeSaldo,
						DescricaoTipoInsumo = q.PrcInsumo.TipoInsumo == "N" ? "Nacional" : q.PrcInsumo.TipoInsumo == "R" ? "Regional" : "Padrão",
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcInsumo.PrcProduto.IdProduto,
							IdProcesso = q.PrcInsumo.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcInsumo.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcInsumo.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcInsumo.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcInsumo.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcInsumo.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcInsumo.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcInsumo.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcInsumo.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcInsumo.PrcProduto.ValorFluxoCaixa
						}
					}
				}
				,
				q =>
				q.IdPrcInsumo == vm.IdProcessoInsumo
				&&
				((q.PrcInsumo.TipoInsumo.Equals("P") || q.PrcInsumo.TipoInsumo.Equals("E")))
				,
				vm
				);


			}

			foreach (var item in listaCodigoInsumosIncluidos.Items)
			{
				string codigoPais = Convert.ToInt32(item.CodigoPais).ToString("D3");
				if (codigoPais != "000")
				{
					var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

					item.DescricaoPais = pais.Descricao;
				}


				var regMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == item.IdMoeda);

				item.CodigoDescricaoMoeda = regMoeda.CodigoMoeda + " | " + regMoeda.Descricao;

				int EmAnalise = 1;
				var ExisteSolicitacaoDetalheEmAnalise = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Existe(q => q.IdInsumo == item.IdPrcInsumo
																									&&
																									q.IdSolicitacaoAlteracao == item.PrcInsumo.IdPrcSolicitacaoAlteracao
																									&&
																									q.IdDetalheInsumo == item.IdDetalheInsumo
																									&&
																									q.Status == EmAnalise
																									);

				item.StatusAnalise = ExisteSolicitacaoDetalheEmAnalise ? "Pendente" : "Concluído";


			}

			if (!string.IsNullOrEmpty(filtroPosterior))
			{
				if (filtroPosterior.Equals("ValorTotal"))
				{
					if (!vm.Reverse)
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderBy(q => q.ValorTotal).ToList();
					}
					else
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderByDescending(q => q.ValorTotal).ToList();
					}
				}
				else if (filtroPosterior.Equals("CodigoPais"))
				{
					if (!vm.Reverse)
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderBy(q => q.CodigoPais).ToList();
					}
					else
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderByDescending(q => q.CodigoPais).ToList();
					}
				}
				else if (filtroPosterior.Equals("CodigoDescricaoMoeda"))
				{
					if (!vm.Reverse)
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderBy(q => q.CodigoDescricaoMoeda).ToList();
					}
					else
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderByDescending(q => q.CodigoDescricaoMoeda).ToList();
					}
				}
			}

			return listaCodigoInsumosIncluidos;
		}
		public bool Deletar(int id)
		{
			if (id == 0)
				return false;

			try
			{
				var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(s => s.IdPEInsumo == id);

				if (regInsumo != null)
				{
					var regDetalhesInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(o => o.IdPEInsumo == regInsumo.IdPEInsumo);

					if (regDetalhesInsumo.Count > 0)
					{
						foreach (var item in regDetalhesInsumo)
						{
							_uowSciex.CommandStackSciex.PEDetalheInsumo.Apagar(item.IdPEDetalheInsumo);
							_uowSciex.CommandStackSciex.Save();
						} 
					}

					_uowSciex.CommandStackSciex.PEInsumo.Apagar(regInsumo.IdPEInsumo);
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
						IdPEProduto = vm.IdPEProduto,
						ValorCoeficienteTecnico = dadosInsumo.ValorCoeficienteTecnico
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
		

		public PagedItems<PEInsumoVM> ListarInsumosNacionalImportadosPorIdProduto(ListarInsumosNacionalImportadosVM vm)
		{
			if (vm == null || vm.IdPEProduto == 0)
				return null;

			PagedItems<PEInsumoVM> listaInsumos;

			if (vm.isQuadroNacional)
			{
				listaInsumos = _uowSciex.QueryStackSciex.PEInsumo.ListarPaginadoGrafo(w => 

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
				q=> q.IdPEProduto == vm.IdPEProduto && (q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R"))
				,vm);

				

			}
			else
			{
				listaInsumos = _uowSciex.QueryStackSciex.PEInsumo.ListarPaginadoGrafo(w =>

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
					ListaPEDetalheInsumo = w.ListaPEDetalheInsumo.Select(q => new PEDetalheInsumoVM()
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
				q => q.IdPEProduto == vm.IdPEProduto && (q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E")), vm);

				
			}

			foreach (var regInsumo in listaInsumos.Items)
			{
				regInsumo.QtdSomatorioDetalheInsumo = regInsumo.ListaPEDetalheInsumo?.Sum(q => q.Quantidade) ?? 0;
				regInsumo.QtdSomatorioDetalheInsumoFormatada = regInsumo.QtdSomatorioDetalheInsumo != null ? Convert.ToDecimal(regInsumo.QtdSomatorioDetalheInsumo).ToString("N5") : "0" ;

				regInsumo.ValorInsumo = regInsumo.ListaPEDetalheInsumo?.Sum(q => q.Quantidade * q.ValorUnitario) ?? 0;
				regInsumo.ValorInsumoFormatada = regInsumo.ValorInsumo != null ? Convert.ToDecimal(regInsumo.ValorInsumo).ToString("N5") : "0";

			}

			return listaInsumos;

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

		public bool AtualizarInsumo(PEInsumoVM vm)
		{
			if (vm == null || vm.IdPEInsumo == 0) 
				return false;

			var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == vm.IdPEInsumo);

			regInsumo.DescricaoPartNumber = vm.DescricaoPartNumber;
			regInsumo.ValorPercentualPerda = vm.ValorPercentualPerda;

			_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumo);
			_uowSciex.CommandStackSciex.Save();

			return true;
		}

		public string FormatarQtdMax(PEInsumoVM vm)
		{
			var qtdMaxima = vm.QtdProduto.Value * vm.ValorCoeficienteTecnico.Value;

			decimal retorno = qtdMaxima + (qtdMaxima * (vm.ValorPercentualPerda.Value / 100));
			return retorno.ToString("N5");
		}

		public PRCInsumoVM SelecionarPrcInsumo(int idPrcInsumo)
		{
			if (idPrcInsumo == 0)
				return new PRCInsumoVM();

			var regInsumo = _uowSciex.QueryStackSciex.PRCInsumo.SelecionarGrafo(w => new PRCInsumoVM()
			{
				IdInsumo = w.IdInsumo,
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
					ValorUnitario = q.ValorUnitario
				}).ToList()

			},
			q => q.IdInsumo == idPrcInsumo);

			var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == regInsumo.CodigoUnidade);
			regInsumo.DescCodigoUnidade = undMed != null ? undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao : "-";

			var qtdProduto = regInsumo?.Produto?.QuantidadeAprovado ?? 0;
			var coefTec = regInsumo?.ValorCoeficienteTecnico ?? 0;
			var percPerda = regInsumo?.ValorPercentualPerda ?? 0;

			regInsumo.QtdMaxInsumo = qtdProduto * coefTec;

			regInsumo.QtdMaxInsumo = regInsumo.QtdMaxInsumo + (regInsumo.QtdMaxInsumo * (percPerda / 100));

			return regInsumo;
		}

		public DadosProcessoDetalhesInsumosVM ListarDetalhePorIdProcessoInsumo(ListarProcessoInsumosNacionalImportadosVM vm)
		{
			if (vm == null || vm.IdProcessoInsumo == 0)
				return null;

			string filtroPosterior = null;

			if (vm.Sort != null)
			{
				if (vm.Sort.Equals("ValorTotal") || vm.Sort.Equals("CodigoPais") || vm.Sort.Equals("CodigoDescricaoMoeda"))
				{
					filtroPosterior = vm.Sort;
					vm.Sort = null;
				}
			}
			DadosProcessoDetalhesInsumosVM dados = new DadosProcessoDetalhesInsumosVM();
			PagedItems<PRCDetalheInsumoVM> listaProcessoDetalheInsumo;
			if (vm.isQuadroNacional)
			{

				listaProcessoDetalheInsumo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.ListarPaginadoGrafo(q => new PRCDetalheInsumoVM()
				{
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					NumeroSequencial = q.NumeroSequencial,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorUnitario = q.ValorUnitario,
					ValorFrete = q.ValorFrete,
					ValorDolar = q.ValorDolar,
					ValorUnitarioCFR = q.ValorUnitarioCFR,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					PrcInsumo = new PRCInsumoVM()
					{
						IdInsumo = q.PrcInsumo.IdInsumo,
						IdPrcProduto = q.PrcInsumo.IdPrcProduto,
						CodigoInsumo = q.PrcInsumo.CodigoInsumo,
						CodigoUnidade = q.PrcInsumo.CodigoUnidade,
						TipoInsumo = q.PrcInsumo.TipoInsumo,
						CodigoNCM = q.PrcInsumo.CodigoNCM,
						DescricaoInsumo = q.PrcInsumo.DescricaoInsumo,
						ValorPercentualPerda = q.PrcInsumo.ValorPercentualPerda,
						CodigoDetalhe = q.PrcInsumo.CodigoDetalhe,
						DescricaoPartNumber = q.PrcInsumo.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.PrcInsumo.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.PrcInsumo.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.PrcInsumo.ValorDolarAprovado,
						QuantidadeAprovado = q.PrcInsumo.QuantidadeAprovado,
						ValorNacionalAprovado = q.PrcInsumo.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.PrcInsumo.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.PrcInsumo.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.PrcInsumo.ValorFreteAprovado,
						ValorDolarComp = q.PrcInsumo.ValorDolarComp,
						QuantidadeComp = q.PrcInsumo.QuantidadeComp,
						ValorDolarSaldo = q.PrcInsumo.ValorDolarSaldo,
						QuantidadeSaldo = q.PrcInsumo.QuantidadeSaldo,
						DescricaoTipoInsumo = q.PrcInsumo.TipoInsumo == "N" ? "Nacional" : q.PrcInsumo.TipoInsumo == "R" ? "Regional" : "Padrão",
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcInsumo.PrcProduto.IdProduto,
							IdProcesso = q.PrcInsumo.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcInsumo.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcInsumo.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcInsumo.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcInsumo.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcInsumo.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcInsumo.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcInsumo.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcInsumo.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcInsumo.PrcProduto.ValorFluxoCaixa
						}
					}
				}
				,
				q =>
				q.IdPrcInsumo == vm.IdProcessoInsumo
				&&
				(q.PrcInsumo.TipoInsumo.Equals("N") || q.PrcInsumo.TipoInsumo.Equals("R"))
				,
				vm
				);

				foreach (var item in listaProcessoDetalheInsumo.Items)
				{
					var valorUnitario = item.ValorUnitario != null ? item.ValorUnitario : 0;
					item.ValorTotal = item.Quantidade * valorUnitario;
				}

			}
			else
			{

				listaProcessoDetalheInsumo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.ListarPaginadoGrafo(q => new PRCDetalheInsumoVM()
				{
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					NumeroSequencial = q.NumeroSequencial,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorUnitario = q.ValorUnitario,
					ValorFrete = q.ValorFrete,
					ValorDolar = q.ValorDolar,
					ValorUnitarioCFR = q.ValorUnitarioCFR,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					PrcInsumo = new PRCInsumoVM()
					{
						IdInsumo = q.PrcInsumo.IdInsumo,
						IdPrcProduto = q.PrcInsumo.IdPrcProduto,
						CodigoInsumo = q.PrcInsumo.CodigoInsumo,
						CodigoUnidade = q.PrcInsumo.CodigoUnidade,
						TipoInsumo = q.PrcInsumo.TipoInsumo,
						CodigoNCM = q.PrcInsumo.CodigoNCM,
						DescricaoInsumo = q.PrcInsumo.DescricaoInsumo,
						ValorPercentualPerda = q.PrcInsumo.ValorPercentualPerda,
						CodigoDetalhe = q.PrcInsumo.CodigoDetalhe,
						DescricaoPartNumber = q.PrcInsumo.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.PrcInsumo.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.PrcInsumo.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.PrcInsumo.ValorDolarAprovado,
						QuantidadeAprovado = q.PrcInsumo.QuantidadeAprovado,
						ValorNacionalAprovado = q.PrcInsumo.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.PrcInsumo.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.PrcInsumo.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.PrcInsumo.ValorFreteAprovado,
						ValorDolarComp = q.PrcInsumo.ValorDolarComp,
						QuantidadeComp = q.PrcInsumo.QuantidadeComp,
						ValorDolarSaldo = q.PrcInsumo.ValorDolarSaldo,
						QuantidadeSaldo = q.PrcInsumo.QuantidadeSaldo,
						DescricaoTipoInsumo = q.PrcInsumo.TipoInsumo == "N" ? "Nacional" : q.PrcInsumo.TipoInsumo == "R" ? "Regional" : "Padrão",
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcInsumo.PrcProduto.IdProduto,
							IdProcesso = q.PrcInsumo.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcInsumo.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcInsumo.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcInsumo.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcInsumo.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcInsumo.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcInsumo.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcInsumo.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcInsumo.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcInsumo.PrcProduto.ValorFluxoCaixa
						}
					}
				}
				,
				q =>
				q.IdPrcInsumo == vm.IdProcessoInsumo
				&&
				(q.PrcInsumo.TipoInsumo.Equals("P") || q.PrcInsumo.TipoInsumo.Equals("E"))
				,
				vm
				);


			}

			foreach (var item in listaProcessoDetalheInsumo.Items)
			{
				string codigoPais = Convert.ToInt32(item.CodigoPais).ToString("D3");
				if (codigoPais != "000")
				{
					var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

					item.DescricaoPais = pais.Descricao;
				}


				var regMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == item.IdMoeda);

				item.CodigoDescricaoMoeda = regMoeda.CodigoMoeda + " | " + regMoeda.Descricao;
			}

			if (!string.IsNullOrEmpty(filtroPosterior))
			{
				if (filtroPosterior.Equals("ValorTotal"))
				{
					if (!vm.Reverse)
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderBy(q => q.ValorTotal).ToList();
					}
					else
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderByDescending(q => q.ValorTotal).ToList();
					}
				}
				else if (filtroPosterior.Equals("CodigoPais"))
				{
					if (!vm.Reverse)
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderBy(q => q.CodigoPais).ToList();
					}
					else
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderByDescending(q => q.CodigoPais).ToList();
					}
				}
				else if (filtroPosterior.Equals("CodigoDescricaoMoeda"))
				{
					if (!vm.Reverse)
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderBy(q => q.CodigoDescricaoMoeda).ToList();
					}
					else
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderByDescending(q => q.CodigoDescricaoMoeda).ToList();
					}
				}
			}

			dados.listaProcessoDetalhesInsumos = listaProcessoDetalheInsumo;


			return dados;
		}

		enum EnumStatusInsumo
		{
			ATIVO = 1,
			EM_ALTERACAO = 2,
			ALTERACAO_APROVADA = 3,
			INATIVO = 4
		}

		enum EnumStatusSolicDetalhe
		{
			EM_ANALISE = 1,
			APROVADO = 2,
			REPROVADO = 3
		}

		public PRCInsumoVM AprovarAlteracaoInsumo(PRCInsumoVM vm)
		{
			if (vm == null) { return new PRCInsumoVM(); }

			var regInsumo = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(q => q.IdInsumo == vm.IdInsumo);

			var regSolicDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Listar(w => w.IdInsumo == regInsumo.IdInsumo && w.IdSolicitacaoAlteracao == regInsumo.IdPrcSolicitacaoAlteracao).FirstOrDefault();

			try
			{
				if (vm.IsAprovarAnalise) //BOTÃO ANALISE LE INSUMOS APROVADO
				{
					regInsumo.StatusInsumo = (int)EnumStatusInsumo.ALTERACAO_APROVADA;

					regSolicDetalhe.Status = (int)EnumStatusSolicDetalhe.APROVADO;

					//foreach (var insumo in regInsumo.LEInsumo)
					//{
					//	insumo.SituacaoInsumo = (int)EnumSituacaoInsumo.ATIVO;
					//	_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo);
					//}

				}
				else // BOTÃO ANALISE LE INSUMOS BLOQUEADO
				{
					regInsumo.StatusInsumo = (int)EnumStatusInsumo.EM_ALTERACAO;

					regSolicDetalhe.Status = (int)EnumStatusSolicDetalhe.REPROVADO;

					//foreach (var insumo in regInsumo.LEInsumo)
					//{
					//	insumo.SituacaoInsumo = (int)EnumSituacaoInsumo.INATIVO;
					//	_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo);
					//}

				}

				vm.Mensagem = "Operação realizada com sucesso";

				_uowSciex.CommandStackSciex.PRCInsumo.Salvar(regInsumo);

				_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(regSolicDetalhe);

				_uowSciex.CommandStackSciex.Save();

				vm.Sucesso = true;

				return vm;
			}
			catch (Exception)
			{
				vm.Sucesso = false;

				return vm;
				throw;
			}
			//ListaSolicDetalhe = q.PRCSolicDetalhe.Select(e => new PRCSolicDetalheVM()
			//{
			//	Id = e.Id,
			//	IdInsumo = e.IdInsumo,
			//	IdDetalheInsumo = e.IdDetalheInsumo,
			//	IdSolicitacaoAlteracao = e.IdSolicitacaoAlteracao,
			//	IdTipoSolicitacao = e.IdTipoSolicitacao,
			//	Status = e.Status,
			//	TipoSolicAlteracao = new TipoSolicAlteracaoVM()
			//	{
			//		Id = e.TipoSolicAlteracao.Id,
			//		Descricao = e.TipoSolicAlteracao.Descricao
			//	}
			//}).ToList()



		}
	}
}