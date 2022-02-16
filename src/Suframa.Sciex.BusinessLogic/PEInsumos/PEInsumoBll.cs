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
	public class PEInsumoBll : IPEInsumoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;

		public PEInsumoBll(
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
		public PagedItems<LEInsumoVM> ListarInsumosPorCodigoPENacionalOuImportado(ListarInsumosNacionalImportadosVM vm)
		{

			if (vm == null || vm.CodigoProdutoExportacao == 0)
				return null;

			List<int> listaCodigoInsumosIncluidos = new List<int>();

			if (vm.isQuadroNacional)
			{
				listaCodigoInsumosIncluidos = _uowSciex.QueryStackSciex.PEInsumo.Listar(q => 
																						q.IdPEProduto == vm.IdPEProduto 
																						&&
																						(q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R"))
																						).Select(q => q.CodigoInsumo).ToList();
			}
			else
			{
				listaCodigoInsumosIncluidos = _uowSciex.QueryStackSciex.PEInsumo.Listar(q =>
																						q.IdPEProduto == vm.IdPEProduto
																						&&
																						(q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E"))
																						).Select(q => q.CodigoInsumo).ToList();
			}

			PagedItems<LEInsumoVM> listaInsumo = null;

			Expression<Func<LEInsumoEntity, bool>> predicado;

			if (listaCodigoInsumosIncluidos.Count > 0)
			{
				if (vm.isQuadroNacional)
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
										(q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E"))
										&&
										!listaCodigoInsumosIncluidos.Contains(q.CodigoInsumo)
										;
				}
}
			else
			{
				if (vm.isQuadroNacional)
				{
					predicado = q => q.CodigoProduto == vm.CodigoProdutoExportacao
												&&
												q.SituacaoInsumo == 1
												&&
												(q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R")); 
				}
				else
				{
					predicado = q => q.CodigoProduto == vm.CodigoProdutoExportacao
												&&
												q.SituacaoInsumo == 1
												&&
												(q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E"));
				}
			}

			listaInsumo = _uowSciex.QueryStackSciex.LEInsumo.ListarPaginado<LEInsumoVM>(
																		predicado
																		, vm);


			return listaInsumo;
		}

		public PagedItems<PEInsumoVM> ListarInsumosNacionalImportadosPorIdProduto(ListarInsumosNacionalImportadosVM vm)
		{
			if (vm == null || vm.IdPEProduto == 0)
				return null;

			List<int?> statusAnalise;
			bool IsPlanoEmElaboracao = false;

			var idPlano = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(q => q.IdPEProduto == vm.IdPEProduto)?.IdPlanoExportacao ?? 0;

			var regPlano = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(q => q.IdPlanoExportacao == idPlano);

			if (regPlano.Situacao == (int)EnumSituacaoPlanoExportacao.EM_ELABORAÇÃO)
			{
				statusAnalise = new List<int?>();
				IsPlanoEmElaboracao = true;
			}
			else
			{
				statusAnalise = new List<int?>()
				{
					null,
					(int)EnumSituacaoAnalisePlanoExportacao.APROVADO,
					(int)EnumSituacaoAnalisePlanoExportacao.REPROVADO,
					(int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO
				};
			}

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
					SituacaoAnalise = w.SituacaoAnalise,
					DescricaoJustificativaErro = w.DescricaoJustificativaErro,
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
					}).ToList(),
					PEProduto = new PEProdutoVM()
					{
						IdPEProduto = w.PlanoExportacaoProduto.IdPEProduto,
						CodigoProdutoExportacao = w.PlanoExportacaoProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = w.PlanoExportacaoProduto.CodigoProdutoSuframa,
						CodigoNCM = w.PlanoExportacaoProduto.CodigoNCM,
						CodigoTipoProduto = w.PlanoExportacaoProduto.CodigoTipoProduto,
						DescricaoModelo = w.PlanoExportacaoProduto.DescricaoModelo,
						Qtd = w.PlanoExportacaoProduto.Qtd,
						ValorDolar = w.PlanoExportacaoProduto.ValorDolar,
						ValorFluxoCaixa = w.PlanoExportacaoProduto.ValorFluxoCaixa,
						IdPlanoExportacao = w.PlanoExportacaoProduto.IdPlanoExportacao,
						CodigoUnidade = w.PlanoExportacaoProduto.CodigoUnidade
					}

				}, 
				q=> q.IdPEProduto == vm.IdPEProduto 
					&& 
					(q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R"))
					&&
					(statusAnalise.Count == 0 || statusAnalise.Contains(q.SituacaoAnalise))
				, vm);

				

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
					DescricaoEspecificacaoTecnica = (w.DescricaoEspecificacaoTecnica == null || w.DescricaoEspecificacaoTecnica == "") ? null : w.DescricaoEspecificacaoTecnica,
					DescricaoInsumo = w.DescricaoInsumo,
					DescricaoPartNumber = w.DescricaoPartNumber,
					TipoInsumo = w.TipoInsumo,
					ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
					ValorDolar = w.ValorDolar,
					ValorPercentualPerda = w.ValorPercentualPerda,
					SituacaoAnalise = w.SituacaoAnalise,
					DescricaoJustificativaErro = w.DescricaoJustificativaErro,
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
					}).ToList(),
					PEProduto = new PEProdutoVM()
					{
						IdPEProduto = w.PlanoExportacaoProduto.IdPEProduto,
						CodigoProdutoExportacao = w.PlanoExportacaoProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = w.PlanoExportacaoProduto.CodigoProdutoSuframa,
						CodigoNCM = w.PlanoExportacaoProduto.CodigoNCM,
						CodigoTipoProduto = w.PlanoExportacaoProduto.CodigoTipoProduto,
						DescricaoModelo = w.PlanoExportacaoProduto.DescricaoModelo,
						Qtd = w.PlanoExportacaoProduto.Qtd,
						ValorDolar = w.PlanoExportacaoProduto.ValorDolar,
						ValorFluxoCaixa = w.PlanoExportacaoProduto.ValorFluxoCaixa,
						IdPlanoExportacao = w.PlanoExportacaoProduto.IdPlanoExportacao,
						CodigoUnidade = w.PlanoExportacaoProduto.CodigoUnidade
					}
				},
				q => q.IdPEProduto == vm.IdPEProduto 
					&& 
					(q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E"))
					&&
					(statusAnalise.Count == 0 || statusAnalise.Contains(q.SituacaoAnalise))
				, vm);

				
			}

			foreach (var regInsumo in listaInsumos.Items)
			{
				regInsumo.QtdSomatorioDetalheInsumo = regInsumo.ListaPEDetalheInsumo?.Sum(q => q.Quantidade) ?? 0;
				regInsumo.QtdSomatorioDetalheInsumoFormatada = regInsumo.QtdSomatorioDetalheInsumo != null ? Convert.ToDecimal(regInsumo.QtdSomatorioDetalheInsumo).ToString("N5") : "0" ;

				regInsumo.ValorInsumo = regInsumo.ListaPEDetalheInsumo?.Sum(q => q.Quantidade * q.ValorUnitario) ?? 0;
				regInsumo.ValorInsumoFormatada = regInsumo.ValorInsumo != null ? Convert.ToDecimal(regInsumo.ValorInsumo).ToString("N5") : "0";

				var qtdProduto = regInsumo?.PEProduto?.Qtd ?? 0;
				var coefTec = regInsumo?.ValorCoeficienteTecnico ?? 0;
				var percPerda = regInsumo?.ValorPercentualPerda ?? 0;

				regInsumo.QtdMaxInsumo = qtdProduto * coefTec;

				regInsumo.QtdMaxInsumo = regInsumo.QtdMaxInsumo + (regInsumo.QtdMaxInsumo * (percPerda / 100));

				var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == regInsumo.CodigoUnidade);
				regInsumo.DescCodigoUnidade = undMed != null ? undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao : "-";

				regInsumo.SituacaoAnaliseString = regInsumo.SituacaoAnalise != null && regInsumo.SituacaoAnalise != 0
																? Enum.GetName(typeof(EnumSituacaoAnalisePlanoExportacao), regInsumo.SituacaoAnalise).Replace("_", " ")
																: "-";

				regInsumo.IsPlanoEmElaboracao = IsPlanoEmElaboracao;
			}

			return listaInsumos;

		}

		public PagedItems<PEInsumoVM> ListarInsumosNacionalImportadosPorIdProdutoParaCorrecao(ListarInsumosNacionalImportadosVM vm)
		{
			if (vm == null || vm.IdPEProduto == 0)
				return null;

			PagedItems<PEInsumoVM> listaInsumos;
			List<int?> statusAnalise = new List<int?>()
			{
				(int)EnumSituacaoAnalisePlanoExportacao.APROVADO,
				(int)EnumSituacaoAnalisePlanoExportacao.REPROVADO,
				(int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO
			};

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
					SituacaoAnalise = w.SituacaoAnalise,
					DescricaoJustificativaErro = w.DescricaoJustificativaErro,
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
					}).ToList(),
					PEProduto = new PEProdutoVM()
					{
						IdPEProduto = w.PlanoExportacaoProduto.IdPEProduto,
						CodigoProdutoExportacao = w.PlanoExportacaoProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = w.PlanoExportacaoProduto.CodigoProdutoSuframa,
						CodigoNCM = w.PlanoExportacaoProduto.CodigoNCM,
						CodigoTipoProduto = w.PlanoExportacaoProduto.CodigoTipoProduto,
						DescricaoModelo = w.PlanoExportacaoProduto.DescricaoModelo,
						Qtd = w.PlanoExportacaoProduto.Qtd,
						ValorDolar = w.PlanoExportacaoProduto.ValorDolar,
						ValorFluxoCaixa = w.PlanoExportacaoProduto.ValorFluxoCaixa,
						IdPlanoExportacao = w.PlanoExportacaoProduto.IdPlanoExportacao,
						CodigoUnidade = w.PlanoExportacaoProduto.CodigoUnidade
					}

				},
				q => q.IdPEProduto == vm.IdPEProduto
					&&
					(q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R"))
					&&
					statusAnalise.Contains(q.SituacaoAnalise)
				, vm);



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
					SituacaoAnalise = w.SituacaoAnalise,
					DescricaoJustificativaErro = w.DescricaoJustificativaErro,
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
					}).ToList(),
					PEProduto = new PEProdutoVM()
					{
						IdPEProduto = w.PlanoExportacaoProduto.IdPEProduto,
						CodigoProdutoExportacao = w.PlanoExportacaoProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = w.PlanoExportacaoProduto.CodigoProdutoSuframa,
						CodigoNCM = w.PlanoExportacaoProduto.CodigoNCM,
						CodigoTipoProduto = w.PlanoExportacaoProduto.CodigoTipoProduto,
						DescricaoModelo = w.PlanoExportacaoProduto.DescricaoModelo,
						Qtd = w.PlanoExportacaoProduto.Qtd,
						ValorDolar = w.PlanoExportacaoProduto.ValorDolar,
						ValorFluxoCaixa = w.PlanoExportacaoProduto.ValorFluxoCaixa,
						IdPlanoExportacao = w.PlanoExportacaoProduto.IdPlanoExportacao,
						CodigoUnidade = w.PlanoExportacaoProduto.CodigoUnidade
					}
				},
				q => q.IdPEProduto == vm.IdPEProduto
					&&
					(q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E"))
					&&
					statusAnalise.Contains(q.SituacaoAnalise)
				, vm);


			}

			foreach (var regInsumo in listaInsumos.Items)
			{
				regInsumo.QtdSomatorioDetalheInsumo = regInsumo.ListaPEDetalheInsumo?.Sum(q => q.Quantidade) ?? 0;
				regInsumo.QtdSomatorioDetalheInsumoFormatada = regInsumo.QtdSomatorioDetalheInsumo != null ? Convert.ToDecimal(regInsumo.QtdSomatorioDetalheInsumo).ToString("N5") : "0";

				regInsumo.ValorInsumo = regInsumo.ListaPEDetalheInsumo?.Sum(q => q.Quantidade * q.ValorUnitario) ?? 0;
				regInsumo.ValorInsumoFormatada = regInsumo.ValorInsumo != null ? Convert.ToDecimal(regInsumo.ValorInsumo).ToString("N5") : "0";

				var qtdProduto = regInsumo?.PEProduto?.Qtd ?? 0;
				var coefTec = regInsumo?.ValorCoeficienteTecnico ?? 0;
				var percPerda = regInsumo?.ValorPercentualPerda ?? 0;

				regInsumo.QtdMaxInsumo = qtdProduto * coefTec;

				regInsumo.QtdMaxInsumo = regInsumo.QtdMaxInsumo + (regInsumo.QtdMaxInsumo * (percPerda / 100));

				var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == regInsumo.CodigoUnidade);
				regInsumo.DescCodigoUnidade = undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao;

				regInsumo.SituacaoAnaliseString = Enum.GetName(typeof(EnumSituacaoAnalisePlanoExportacao), regInsumo.SituacaoAnalise).Replace("_", " ");

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

		public ResultadoMensagemProcessamentoVM AprovarTodosInsumosEDetalhes(ListarInsumosNacionalImportadosVM vm)
		{
			if (vm == null || vm.IdPEProduto == 0)
			{
				return new ResultadoMensagemProcessamentoVM()
				{					
					Resultado = false,
					Mensagem = "Id do produto inválido"
				};
			}

			var result = new ResultadoMensagemProcessamentoVM()
			{
				Resultado = true
			};


			IList<PEInsumoVM> listaInsumos = BuscarListaRegistros(vm);

			AprovarRegistros(listaInsumos, result);


			return result;
		}

		private IList<PEInsumoVM> BuscarListaRegistros(ListarInsumosNacionalImportadosVM vm)
		{
			if (vm == null || vm.IdPEProduto == 0)
				return null;

			IList<PEInsumoVM> listaInsumos;
			List<int?> statusAnalise = new List<int?>()
			{
				null
				//,(int)EnumSituacaoAnalisePlanoExportacao.APROVADO
				//,(int)EnumSituacaoAnalisePlanoExportacao.REPROVADO
				//,(int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO
				//,(int)EnumSituacaoAnalisePlanoExportacao.ALTERADO
			};

			if (vm.isQuadroNacional)
			{
				listaInsumos = _uowSciex.QueryStackSciex.PEInsumo.ListarGrafo(w =>

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
					SituacaoAnalise = w.SituacaoAnalise,
					DescricaoJustificativaErro = w.DescricaoJustificativaErro,
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
						ValorUnitario = q.ValorUnitario,
						SituacaoAnalise = q.SituacaoAnalise
					}).ToList(),
					PEProduto = new PEProdutoVM()
					{
						IdPEProduto = w.PlanoExportacaoProduto.IdPEProduto,
						CodigoProdutoExportacao = w.PlanoExportacaoProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = w.PlanoExportacaoProduto.CodigoProdutoSuframa,
						CodigoNCM = w.PlanoExportacaoProduto.CodigoNCM,
						CodigoTipoProduto = w.PlanoExportacaoProduto.CodigoTipoProduto,
						DescricaoModelo = w.PlanoExportacaoProduto.DescricaoModelo,
						Qtd = w.PlanoExportacaoProduto.Qtd,
						ValorDolar = w.PlanoExportacaoProduto.ValorDolar,
						ValorFluxoCaixa = w.PlanoExportacaoProduto.ValorFluxoCaixa,
						IdPlanoExportacao = w.PlanoExportacaoProduto.IdPlanoExportacao,
						CodigoUnidade = w.PlanoExportacaoProduto.CodigoUnidade
					}

				},
				q => q.IdPEProduto == vm.IdPEProduto
					&&
					(q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R"))
					&&
					(
						statusAnalise.Contains(q.SituacaoAnalise)
						||
						q.ListaPEDetalheInsumo.Any(w=> statusAnalise.Contains(w.SituacaoAnalise))
					)
				);



			}
			else
			{
				listaInsumos = _uowSciex.QueryStackSciex.PEInsumo.ListarGrafo(w =>

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
					SituacaoAnalise = w.SituacaoAnalise,
					DescricaoJustificativaErro = w.DescricaoJustificativaErro,
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
						ValorUnitario = q.ValorUnitario,
						SituacaoAnalise = q.SituacaoAnalise
					}).ToList(),
					PEProduto = new PEProdutoVM()
					{
						IdPEProduto = w.PlanoExportacaoProduto.IdPEProduto,
						CodigoProdutoExportacao = w.PlanoExportacaoProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = w.PlanoExportacaoProduto.CodigoProdutoSuframa,
						CodigoNCM = w.PlanoExportacaoProduto.CodigoNCM,
						CodigoTipoProduto = w.PlanoExportacaoProduto.CodigoTipoProduto,
						DescricaoModelo = w.PlanoExportacaoProduto.DescricaoModelo,
						Qtd = w.PlanoExportacaoProduto.Qtd,
						ValorDolar = w.PlanoExportacaoProduto.ValorDolar,
						ValorFluxoCaixa = w.PlanoExportacaoProduto.ValorFluxoCaixa,
						IdPlanoExportacao = w.PlanoExportacaoProduto.IdPlanoExportacao,
						CodigoUnidade = w.PlanoExportacaoProduto.CodigoUnidade
					}
				},
				q => q.IdPEProduto == vm.IdPEProduto
					&&
					(q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E"))
					&&
					(
						statusAnalise.Contains(q.SituacaoAnalise)
						||
						q.ListaPEDetalheInsumo.Any(w => statusAnalise.Contains(w.SituacaoAnalise))
					)
				);


			}

			return listaInsumos;
		}

		private void AprovarRegistros(IList<PEInsumoVM> listaInsumos, ResultadoMensagemProcessamentoVM result)
		{
			
			try
			{

				var listaInsumosSemStatusAlterado = listaInsumos.Where(q => q.SituacaoAnalise != (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO).ToList();
				var listaInsumosStatusAlterado = listaInsumos.Where(q => q.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO).ToList();

				foreach (var insumo in listaInsumosSemStatusAlterado)
				{
					var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == insumo.IdPEInsumo);

					foreach (var detalheInsumo in regInsumo.ListaPEDetalheInsumo)
					{
						detalheInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.APROVADO;
						detalheInsumo.DescricaoJustificativaErro = null;
						_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalheInsumo);
					}

					regInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.APROVADO;
					regInsumo.DescricaoJustificativaErro = null;
					_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumo);
				}

				foreach (var insumo in listaInsumosStatusAlterado)
				{
					var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == insumo.IdPEInsumo);

					foreach (var detalheInsumo in regInsumo.ListaPEDetalheInsumo)
					{
						detalheInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
						detalheInsumo.DescricaoJustificativaErro = null;
						_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalheInsumo);
					}

					regInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
					regInsumo.DescricaoJustificativaErro = null;
					_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumo);
				}

				_uowSciex.CommandStackSciex.Save();
			}
			catch (Exception e)
			{
				result.Resultado = false;
				result.Mensagem = $"Falha ao aprovar registros: (Mensagem: {e.Message} / StackTrace: {e.StackTrace})";
			}
		}

		public ResultadoMensagemProcessamentoVM InativarInsumo(PEInsumoVM vm)
		{
			if (vm == null || vm.IdPEInsumo == 0)
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

			var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == vm.IdPEInsumo);

			if (regInsumo == null)
			{
				result.Resultado = false;
				result.Mensagem = $"Não foi encontrado insumo com o id {vm.IdPEInsumo}";
				return result;
			}
			else
			{

				foreach (var detalhe in regInsumo.ListaPEDetalheInsumo)
				{

					if (detalhe.SituacaoAnalise == null 
						||
						detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO)
					{
						detalhe.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO_EMPRESA;
						_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalhe);	
					}
				}

				//regInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
				regInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO_EMPRESA;
				_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumo);
				_uowSciex.CommandStackSciex.Save();
			}

			return result;
		}

		public ResultadoMensagemProcessamentoVM CorrigirDadosInsumo(PEInsumoVM vm)
		{
			if (vm == null || vm.IdPEInsumo == 0)
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

						foreach (var detalhe in regInsumoDE.ListaPEDetalheInsumo)
						{
							if (detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.APROVADO)
							{
								regInsumoPARA.ListaPEDetalheInsumo.Add(detalhe);
							}
							else if (detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO)
							{
								PEDetalheInsumoEntity copiaDetalhe = new PEDetalheInsumoEntity()
								{
									IdPEInsumo = detalhe.IdPEInsumo,
									IdMoeda = detalhe.IdMoeda,
									CodigoPais = detalhe.CodigoPais,
									DescricaoJustificativaErro = detalhe.DescricaoJustificativaErro,
									NumeroSequencial = detalhe.NumeroSequencial,
									Quantidade = detalhe.Quantidade,
									SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO,
									ValorDolar = detalhe.ValorDolar,
									ValorDolarCRF = detalhe.ValorDolarCRF,
									ValorDolarFOB = detalhe.ValorDolarFOB,
									ValorFrete = detalhe.ValorFrete,
									ValorUnitario = detalhe.ValorUnitario
								};

								regInsumoPARA.ListaPEDetalheInsumo.Add(copiaDetalhe);

								detalhe.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO;
							}

						}


					}

					_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoDE);
					_uowSciex.CommandStackSciex.Save();

					_uowSciex.CommandStackSciex.DetachEntries();

					regInsumoPARA.DescricaoPartNumber = vm.DescricaoPartNumber;
					regInsumoPARA.ValorPercentualPerda = vm.ValorPercentualPerda;
					_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoPARA);
				}
				else if (regInsumoDE.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO)
				{
					regInsumoDE.DescricaoPartNumber = vm.DescricaoPartNumber;
					regInsumoDE.ValorPercentualPerda = vm.ValorPercentualPerda;
					_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoDE);
				}
				
				
				_uowSciex.CommandStackSciex.Save();
			}

			return result;
		}

		public PagedItems<PEInsumoVM> ListarInsumosNacionalImportadosPorIdProdutoAnalise(ListarInsumosNacionalImportadosVM vm)
		{
			if (vm == null || vm.IdPEProduto == 0)
				return null;

			PagedItems<PEInsumoVM> listaInsumos;
			List<int?> statusAnalise = new List<int?>()
			{
				null,
				(int)EnumSituacaoAnalisePlanoExportacao.APROVADO,
				(int)EnumSituacaoAnalisePlanoExportacao.REPROVADO,
				(int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO
			};

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
					DescricaoEspecificacaoTecnica = (w.DescricaoEspecificacaoTecnica == null || w.DescricaoEspecificacaoTecnica == "") ? null : w.DescricaoEspecificacaoTecnica,
					DescricaoInsumo = w.DescricaoInsumo,
					DescricaoPartNumber = w.DescricaoPartNumber,
					TipoInsumo = w.TipoInsumo,
					ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
					ValorDolar = w.ValorDolar,
					ValorPercentualPerda = w.ValorPercentualPerda,
					SituacaoAnalise = w.SituacaoAnalise,
					DescricaoJustificativaErro = w.DescricaoJustificativaErro,
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
					}).ToList(),
					PEProduto = new PEProdutoVM()
					{
						IdPEProduto = w.PlanoExportacaoProduto.IdPEProduto,
						CodigoProdutoExportacao = w.PlanoExportacaoProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = w.PlanoExportacaoProduto.CodigoProdutoSuframa,
						CodigoNCM = w.PlanoExportacaoProduto.CodigoNCM,
						CodigoTipoProduto = w.PlanoExportacaoProduto.CodigoTipoProduto,
						DescricaoModelo = w.PlanoExportacaoProduto.DescricaoModelo,
						Qtd = w.PlanoExportacaoProduto.Qtd,
						ValorDolar = w.PlanoExportacaoProduto.ValorDolar,
						ValorFluxoCaixa = w.PlanoExportacaoProduto.ValorFluxoCaixa,
						IdPlanoExportacao = w.PlanoExportacaoProduto.IdPlanoExportacao,
						CodigoUnidade = w.PlanoExportacaoProduto.CodigoUnidade
					}

				},
				q => q.IdPEProduto == vm.IdPEProduto
					&&
					(q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R"))
					&&
					statusAnalise.Contains(q.SituacaoAnalise)
				, vm);



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
					DescricaoEspecificacaoTecnica = (w.DescricaoEspecificacaoTecnica == null || w.DescricaoEspecificacaoTecnica == "") ? null : w.DescricaoEspecificacaoTecnica,
					DescricaoInsumo = w.DescricaoInsumo,
					DescricaoPartNumber = w.DescricaoPartNumber,
					TipoInsumo = w.TipoInsumo,
					ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
					ValorDolar = w.ValorDolar,
					ValorPercentualPerda = w.ValorPercentualPerda,
					SituacaoAnalise = w.SituacaoAnalise,
					DescricaoJustificativaErro = w.DescricaoJustificativaErro,
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
					}).ToList(),
					PEProduto = new PEProdutoVM()
					{
						IdPEProduto = w.PlanoExportacaoProduto.IdPEProduto,
						CodigoProdutoExportacao = w.PlanoExportacaoProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = w.PlanoExportacaoProduto.CodigoProdutoSuframa,
						CodigoNCM = w.PlanoExportacaoProduto.CodigoNCM,
						CodigoTipoProduto = w.PlanoExportacaoProduto.CodigoTipoProduto,
						DescricaoModelo = w.PlanoExportacaoProduto.DescricaoModelo,
						Qtd = w.PlanoExportacaoProduto.Qtd,
						ValorDolar = w.PlanoExportacaoProduto.ValorDolar,
						ValorFluxoCaixa = w.PlanoExportacaoProduto.ValorFluxoCaixa,
						IdPlanoExportacao = w.PlanoExportacaoProduto.IdPlanoExportacao,
						CodigoUnidade = w.PlanoExportacaoProduto.CodigoUnidade
					}
				},
				q => q.IdPEProduto == vm.IdPEProduto
					&&
					(q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E"))
					&&
					statusAnalise.Contains(q.SituacaoAnalise)
				, vm);


			}

			if (listaInsumos.Items.Count > 0)
			{

				decimal? ValorTotalInsumo = 0;
				decimal? ValorTotalFOB = 0;
				decimal? ValorTotalFrete = 0;
				decimal? ValorTotalCFR = 0;

			
				foreach (var regInsumo in listaInsumos.Items)
				{
					regInsumo.QtdSomatorioDetalheInsumo = regInsumo.ListaPEDetalheInsumo?.Where(q => statusAnalise.Contains(q.SituacaoAnalise)).Sum(q => q.Quantidade) ?? 0;
					regInsumo.QtdSomatorioDetalheInsumoFormatada = regInsumo.QtdSomatorioDetalheInsumo != null ? Convert.ToDecimal(regInsumo.QtdSomatorioDetalheInsumo).ToString("N5") : "0";

					regInsumo.ValorInsumo = regInsumo.ListaPEDetalheInsumo?.Where(q => statusAnalise.Contains(q.SituacaoAnalise)).Sum(q => q.ValorUnitario) ?? 0;
					regInsumo.ValorInsumoFormatada = regInsumo.ValorInsumo != null ? Convert.ToDecimal(regInsumo.ValorInsumo).ToString("N5") : "0";

					var qtdProduto = regInsumo?.PEProduto?.Qtd ?? 0;
					var coefTec = regInsumo?.ValorCoeficienteTecnico ?? 0;
					var percPerda = regInsumo?.ValorPercentualPerda ?? 0;

					regInsumo.QtdMaxInsumo = qtdProduto * coefTec;

					regInsumo.QtdMaxInsumo += (regInsumo.QtdMaxInsumo * (percPerda / 100));

					var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == regInsumo.CodigoUnidade);
					regInsumo.DescCodigoUnidade = undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao;

					regInsumo.SituacaoAnaliseString = regInsumo.SituacaoAnalise != null && regInsumo.SituacaoAnalise != 0
																		? Enum.GetName(typeof(EnumSituacaoAnalisePlanoExportacao), regInsumo.SituacaoAnalise).Replace("_", " ")
																		: "-";

					if (vm.isQuadroNacional)
					{
						foreach (var detalhe in regInsumo.ListaPEDetalheInsumo)
						{
							if (detalhe.SituacaoAnalise == null
								||
								detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.APROVADO
								||
								detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO
								)
								ValorTotalInsumo += detalhe.Quantidade * detalhe.ValorUnitario;
						}
					}
					else
					{
						foreach (var detalhe in regInsumo.ListaPEDetalheInsumo)
						{
							if (detalhe.SituacaoAnalise == null
								||
								detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.APROVADO
								||
								detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO
								)
							{
								ValorTotalFOB += detalhe.ValorDolarFOB;
								ValorTotalFrete += detalhe.ValorFrete;
								ValorTotalCFR += detalhe.ValorDolarCRF;
							}

						}
					}

				}

				listaInsumos.Items[0].ValorTotalInsumo = ValorTotalInsumo;
				listaInsumos.Items[0].ValorTotalInsumoFormatado = listaInsumos.Items[0].ValorTotalInsumo != null ? Convert.ToDecimal(listaInsumos.Items[0].ValorTotalInsumo).ToString("N5") : "0";
				listaInsumos.Items[0].ValorTotalFOB = ValorTotalFOB;
				listaInsumos.Items[0].ValorTotalFOBFormatado = listaInsumos.Items[0].ValorTotalFOB != null ? Convert.ToDecimal(listaInsumos.Items[0].ValorTotalFOB).ToString("N5") : "0";
				listaInsumos.Items[0].ValorTotalFrete = ValorTotalFrete;
				listaInsumos.Items[0].ValorTotalFreteFormatado = listaInsumos.Items[0].ValorTotalFrete != null ? Convert.ToDecimal(listaInsumos.Items[0].ValorTotalFrete).ToString("N5") : "0";
				listaInsumos.Items[0].ValorTotalCFR = ValorTotalCFR;
				listaInsumos.Items[0].ValorTotalCFRFormatado = listaInsumos.Items[0].ValorTotalCFR != null ? Convert.ToDecimal(listaInsumos.Items[0].ValorTotalCFR).ToString("N5") : "0";

			}
			return listaInsumos;

		}

		public PEInsumoVM SelecionarInsumoAnteriorPorIdInsumoAtual(int idPEInsumoAtual)
		{
			if (idPEInsumoAtual == 0)
				return new PEInsumoVM();

			var regInsumoAtual = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == idPEInsumoAtual);

			if (regInsumoAtual == null)
			{
				return new PEInsumoVM();
			}

			var codigoInsumo = regInsumoAtual.CodigoInsumo;
			var codigoIdProduto = regInsumoAtual.IdPEProduto;

			int situacaoAlterado = 3;
			var regInsumoAnterior = _uowSciex.QueryStackSciex.PEInsumo.ListarGrafo(w => new PEInsumoVM()
			{
				IdPEInsumo = w.IdPEInsumo,
				IdPEProduto = w.IdPEProduto,
				CodigoDetalhe = w.CodigoDetalhe,
				CodigoInsumo = w.CodigoInsumo,
				CodigoNcm = w.CodigoNcm,
				CodigoUnidade = w.CodigoUnidade,
				DescricaoEspecificacaoTecnica = (w.DescricaoEspecificacaoTecnica == null || w.DescricaoEspecificacaoTecnica == "") ? null : w.DescricaoEspecificacaoTecnica,
				DescricaoInsumo = w.DescricaoInsumo,
				DescricaoPartNumber = w.DescricaoPartNumber,
				TipoInsumo = w.TipoInsumo,
				ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
				ValorDolar = w.ValorDolar,
				ValorPercentualPerda = w.ValorPercentualPerda,
				SituacaoAnalise = w.SituacaoAnalise,
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
			q => q.IdPEProduto == codigoIdProduto
				&&
				q.CodigoInsumo == codigoInsumo
				&&
				q.SituacaoAnalise == situacaoAlterado).LastOrDefault();

			return regInsumoAnterior;
		}

		public PEInsumoVM SelecionarPEInsumo(int idPEInsumo)
		{
			if (idPEInsumo == 0)
				return new PEInsumoVM();

			var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.SelecionarGrafo(w => new PEInsumoVM()
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
				DescricaoTipoInsumo = w.TipoInsumo == "N" ? "Nacional" : w.TipoInsumo == "R" ? "Regional" : "Padrão",
				ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
				ValorDolar = w.ValorDolar,
				ValorPercentualPerda = w.ValorPercentualPerda,
				PEProduto = new PEProdutoVM()
				{
					IdPEProduto = w.PlanoExportacaoProduto.IdPEProduto,
					CodigoProdutoExportacao = w.PlanoExportacaoProduto.CodigoProdutoExportacao,
					CodigoProdutoSuframa = w.PlanoExportacaoProduto.CodigoProdutoSuframa,
					CodigoNCM = w.PlanoExportacaoProduto.CodigoNCM,
					CodigoTipoProduto = w.PlanoExportacaoProduto.CodigoTipoProduto,
					DescricaoModelo = w.PlanoExportacaoProduto.DescricaoModelo,
					Qtd = w.PlanoExportacaoProduto.Qtd,
					ValorDolar = w.PlanoExportacaoProduto.ValorDolar,
					ValorFluxoCaixa = w.PlanoExportacaoProduto.ValorFluxoCaixa,
					IdPlanoExportacao = w.PlanoExportacaoProduto.IdPlanoExportacao,
					CodigoUnidade = w.PlanoExportacaoProduto.CodigoUnidade
				},
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
			q => q.IdPEInsumo == idPEInsumo);

			var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == regInsumo.CodigoUnidade);
			regInsumo.DescCodigoUnidade = undMed != null ? undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao : "-";

			var qtdProduto = regInsumo?.PEProduto?.Qtd ?? 0;
			var coefTec = regInsumo?.ValorCoeficienteTecnico ?? 0;
			var percPerda = regInsumo?.ValorPercentualPerda ?? 0;

			regInsumo.QtdMaxInsumo = qtdProduto * coefTec;

			regInsumo.QtdMaxInsumo = regInsumo.QtdMaxInsumo + (regInsumo.QtdMaxInsumo * (percPerda / 100));

			return regInsumo;
		}
	}
}