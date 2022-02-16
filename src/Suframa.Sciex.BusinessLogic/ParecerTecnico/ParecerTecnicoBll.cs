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
using System.Runtime.InteropServices.WindowsRuntime;
using NLog;

namespace Suframa.Sciex.BusinessLogic
{
	public class ParecerTecnicoBll : IParecerTecnicoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public ParecerTecnicoBll(
			IUnitOfWorkSciex uowSciex, 
			IUnitOfWork uowCadsuf,
			IUsuarioPssBll usuarioPssBll, 
			IUsuarioInformacoesBll usuarioInformacoesBll
			)
		{
			_uowSciex = uowSciex;
			_usuarioPssBll = usuarioPssBll;
		}

		enum EnumTipoParecer
		{
			APROVADO = 1,
			ALTERADO = 2,
			CANCELADO = 3 
		}

		public RelatorioParecerTecnicoVM SelecionarRelatorio(int id)
		{
			var parecer = _uowSciex.QueryStackSciex.ParecerTecnico.Selecionar<RelatorioParecerTecnicoVM>(o => o.IdParecerTecnico == id);
			int tipoParecer= 0;
			switch (parecer.TipoStatus)
			{
				case "AP":
					parecer.QuantidadeProdutosFormatado = (parecer.parecerTecnicoProdutos.Count()).ToString("D3");
					foreach (var item in parecer.parecerTecnicoProdutos)
					{
						item.NumeroSequenciaFormatado = item.NumeroSequencia.Value.ToString("D3");
						item.ValorUnitarioProdutoAprovadoFormatado = item.ValorUnitarioProdutoAprovado == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.ValorUnitarioProdutoAprovado.Value);
						item.ValorInsumoImportacaoProdutoFobFormatado = item.ValorInsumoImportacaoProdutoFob == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.ValorInsumoImportacaoProdutoFob.Value);
						item.ValorInsumoNacionalProdutoFormatado = item.ValorInsumoNacionalProduto == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.ValorInsumoNacionalProduto.Value);
						item.ValorInsumoImportacaoProdutoFormatado = item.ValorInsumoImportacaoProduto == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.ValorInsumoImportacaoProduto.Value);
						item.QuantidadePaisFormatado = item.QuantidadePais == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.QuantidadePais.Value);
						item.ValorPaisFormatado = item.ValorPais == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.ValorPais.Value);
						item.CodigoProdutoSuframaFormatado = item.CodigoProdutoSuframa.Value.ToString("D4");
						item.DescricaoUnidadeFormatado = item.DescricaoUnidade == "U" ? "UNIDADE"
																			: item.DescricaoUnidade;
					}

					parecer.QuantidadeTotalProdutoFormatado = parecer.QuantidadeTotalProduto == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.QuantidadeTotalProduto.Value);
					parecer.ValorExportacaoAprovadoFormatado = parecer.ValorExportacaoAprovado == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorExportacaoAprovado.Value);
					parecer.ValorInsumoNacionalFormatado = parecer.ValorInsumoNacional == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorInsumoNacional.Value);
					parecer.ValorInsumoImportadoRealFormatado = parecer.ValorInsumoImportadoReal == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorInsumoImportadoReal.Value);
					parecer.ValorTotalInsumosRealFormatado = parecer.ValorTotalInsumosReal == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorTotalInsumosReal.Value);
					parecer.ValorInsumoImportadoFobFormatado = parecer.ValorInsumoImportadoFob == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorInsumoImportadoFob.Value);
					parecer.ValorInsumoImportacoCfrFormatado = parecer.ValorInsumoImportacoCfr == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorInsumoImportacoCfr.Value);
					parecer.QuantidadeExportacaoAprovadoFormatado = parecer.QuantidadeExportacaoAprovado == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.QuantidadeExportacaoAprovado.Value);
					parecer.ValorIndiceNacionalizacaoFormatado = parecer.ValorIndiceNacionalizacao == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorIndiceNacionalizacao.Value);
					parecer.ValorIndiceImportacaoFormatado = parecer.ValorIndiceImportacao == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorIndiceImportacao.Value);
					parecer.ValorTaxaCambialFormatado = parecer.ValorTaxaCambial == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorTaxaCambial.Value);


					parecer.DataStatusFormatada = parecer.DataStatus.Value.ToShortDateString();
					parecer.DataValidadeFormatada = parecer.DataValidade.Value.ToShortDateString();
					parecer.NumeroControleString = parecer.NumeroControle.ToString("D4") + "/" + parecer.AnoControle.ToString("D4");
					parecer.NumeroPlanoString = parecer.NumeroPlano.Value.ToString("D4") + "/" + parecer.AnoPlano.Value.ToString("D4");
					parecer.NumeroProcessoString = parecer.NumeroProcesso.Value.ToString("D4") + "/" + parecer.AnoProcesso.Value.ToString("D4");
					parecer.TipoModalidadeDescricao = parecer.TipoModalidade == "S" ? "Suspensão" : parecer.TipoModalidade == "I" ? "Isenção" : "-";
					parecer.TipoStatusDescricao = parecer.TipoStatus == "AP" ? "APROVADO" : "-";

					tipoParecer = Convert.ToInt32(EnumTipoParecer.APROVADO);
					parecer.ParecerTecnicoComplementar = _uowSciex.QueryStackSciex.ParecerComplementar.Selecionar<ParecerComplementarVM>(x => x.IdParecerComplementar == tipoParecer);
					break;

				case "AL":
					parecer.QuantidadeProdutosFormatado = (parecer.parecerTecnicoProdutos.Count()).ToString("D3");
					foreach (var item in parecer.parecerTecnicoProdutos)
					{
						item.NumeroSequenciaFormatado = item.NumeroSequencia.Value.ToString("D3");
						item.ValorUnitarioProdutoAprovadoFormatado = item.ValorUnitarioProdutoAprovado == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.ValorUnitarioProdutoAprovado.Value);
						item.ValorInsumoImportacaoProdutoFobFormatado = item.ValorInsumoImportacaoProdutoFob == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.ValorInsumoImportacaoProdutoFob.Value);
						item.ValorInsumoNacionalProdutoFormatado = item.ValorInsumoNacionalProduto == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.ValorInsumoNacionalProduto.Value);
						item.ValorInsumoImportacaoProdutoFormatado = item.ValorInsumoImportacaoProduto == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.ValorInsumoImportacaoProduto.Value);
						item.QuantidadePaisFormatado = item.QuantidadePais == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.QuantidadePais.Value);
						item.ValorPaisFormatado = item.ValorPais == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", item.ValorPais.Value);
						item.CodigoProdutoSuframaFormatado = item.CodigoProdutoSuframa.Value.ToString("D4");
						item.DescricaoUnidadeFormatado = item.DescricaoUnidade == "U" ? "UNIDADE"
																			: item.DescricaoUnidade;
					}

					parecer.QuantidadeTotalProdutoFormatado = parecer.QuantidadeTotalProduto == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.QuantidadeTotalProduto.Value);
					parecer.ValorExportacaoAprovadoFormatado = parecer.ValorExportacaoAprovado == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorExportacaoAprovado.Value);
					parecer.ValorInsumoNacionalFormatado = parecer.ValorInsumoNacional == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorInsumoNacional.Value);
					parecer.ValorInsumoImportadoRealFormatado = parecer.ValorInsumoImportadoReal == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorInsumoImportadoReal.Value);
					parecer.ValorTotalInsumosRealFormatado = parecer.ValorTotalInsumosReal == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorTotalInsumosReal.Value);
					parecer.ValorInsumoImportadoFobFormatado = parecer.ValorInsumoImportadoFob == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorInsumoImportadoFob.Value);
					parecer.ValorInsumoImportacoCfrFormatado = parecer.ValorInsumoImportacoCfr == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorInsumoImportacoCfr.Value);
					parecer.QuantidadeExportacaoAprovadoFormatado = parecer.QuantidadeExportacaoAprovado == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.QuantidadeExportacaoAprovado.Value);
					parecer.ValorIndiceNacionalizacaoFormatado = parecer.ValorIndiceNacionalizacao == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorIndiceNacionalizacao.Value);
					parecer.ValorIndiceImportacaoFormatado = parecer.ValorIndiceImportacao == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorIndiceImportacao.Value);
					parecer.ValorTaxaCambialFormatado = parecer.ValorTaxaCambial == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ValorTaxaCambial.Value);


					parecer.DataStatusFormatada = parecer.DataStatus.Value.ToShortDateString();
					parecer.DataValidadeFormatada = parecer.DataValidade.Value.ToShortDateString();
					parecer.NumeroControleString = parecer.NumeroControle.ToString("D4") + "/" + parecer.AnoControle.ToString("D4");
					parecer.NumeroPlanoString = parecer.NumeroPlano.Value.ToString("D4") + "/" + parecer.AnoPlano.Value.ToString("D4");
					parecer.NumeroProcessoString = parecer.NumeroProcesso.Value.ToString("D4") + "/" + parecer.AnoProcesso.Value.ToString("D4");
					parecer.TipoModalidadeDescricao = parecer.TipoModalidade == "S" ? "Suspensão" : parecer.TipoModalidade == "I" ? "Isenção" : "-";
					parecer.TipoStatusDescricao = parecer.TipoStatus == "AP" ? "APROVADO" : "-";

					tipoParecer = Convert.ToInt32(EnumTipoParecer.ALTERADO);
					parecer.ParecerTecnicoComplementar = _uowSciex.QueryStackSciex.ParecerComplementar.Selecionar<ParecerComplementarVM>(x => x.IdParecerComplementar == tipoParecer);

					//parecer.NumeroProcesso


					break;

				case "CA":
					
					parecer.TipoStatusDescricao = parecer.TipoStatus == "CA" ? "CANCELADO" : "-";
					parecer.NumeroPlanoString = parecer.NumeroPlano.Value.ToString("D4") + "/" + parecer.AnoPlano.Value.ToString("D4");
					parecer.NumeroControleString = parecer.AnoControle.ToString("D4") + "/" + parecer.NumeroControle.ToString("D4");
					parecer.InscricaoCadastral = parecer.InscricaoSuframa.ToString();
					parecer.NumeroProcessoString = parecer.NumeroProcesso.Value.ToString("D4") + "/" + parecer.AnoProcesso.Value.ToString("D4");
					parecer.ExportacaoComprovadaFormatado = parecer.ExportacaoComprovada == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.ExportacaoComprovada);
					parecer.InsumosNacionaisAdquiridosFormatado = parecer.InsumosNacionaisAdquiridos == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.InsumosNacionaisAdquiridos);
					var regProcesso = _uowSciex.QueryStackSciex.Processo.Selecionar(o => o.AnoProcesso == parecer.AnoProcesso && o.NumeroProcesso == parecer.NumeroProcesso);
					var regProduto = _uowSciex.QueryStackSciex.PRCProduto.Selecionar(o => o.IdProcesso == regProcesso.IdProcesso);
					parecer.InsumosImportadosAutorizados = regProduto.ListaInsumos.Where(o => o.StatusInsumo == 1).Sum(p => p.ValorDolarComp);
					parecer.InsumosImportadosAutorizadosFormatado = parecer.InsumosImportadosAutorizados == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.InsumosImportadosAutorizados);
					parecer.TotalInsumosImportadosInternadosFormatado = parecer.TotalInsumosImportadosInternados == null ? string.Format("{0:0,000.0000000}", 0) : string.Format("{0:0,000.0000000}", parecer.TotalInsumosImportadosInternados);
					parecer.TipoModalidadeDescricao = parecer.TipoModalidade == "S" ? "Suspensão" : parecer.TipoModalidade == "I" ? "Isenção" : "-";
					tipoParecer = Convert.ToInt32(EnumTipoParecer.CANCELADO);
					parecer.ParecerTecnicoComplementar = _uowSciex.QueryStackSciex.ParecerComplementar.Selecionar<ParecerComplementarVM>(x => x.IdParecerComplementar == tipoParecer);
					parecer.DataCancelamento = Convert.ToDateTime(regProcesso.ListaStatus.Where(o => o.Tipo == parecer.TipoStatus).FirstOrDefault().Data).ToShortDateString();
					parecer.AssinaturaResponsavel = parecer.NomeResponsavel + " " + parecer.CpfResponsavel;


					break;
			}


			return parecer;
		}

		public PagedItems<ParecerTecnicoVM> ListarPaginado(ParecerTecnicoVM pagedFilter)
		{
			if (!string.IsNullOrEmpty(pagedFilter.NumeroControleString))
			{
				var numeroControle = pagedFilter.NumeroControleString.Split('/');
				pagedFilter.NumeroControle = long.Parse(numeroControle[0]);
				pagedFilter.AnoControle = int.Parse(numeroControle[1]);
			}

			var lista = _uowSciex.QueryStackSciex.ParecerTecnico
					.ListarPaginado<ParecerTecnicoVM>(
								o => o.AnoProcesso == pagedFilter.AnoProcesso && o.NumeroProcesso == pagedFilter.NumeroProcesso
								&&
								(String.IsNullOrEmpty(pagedFilter.TipoStatus) || o.TipoStatus == pagedFilter.TipoStatus)
								&&
								(pagedFilter.NumeroControle == 0 || o.NumeroControle == pagedFilter.NumeroControle)
								&&
								(pagedFilter.AnoControle == 0 || o.AnoControle == pagedFilter.AnoControle)
								, pagedFilter
							);

			foreach (var item in lista.Items)
			{
				item.NumeroControleString = item.NumeroControle.ToString("D4") + "/" + item.AnoControle.ToString("D4");
				item.TipoModalidadeDescricao = item.TipoModalidade == "S" ? "Suspensão" : item.TipoModalidade == "I" ? "Isenção" : "-";
				item.TipoStatusDescricao = item.TipoStatus == "AP" ? "Aprovação"
															: item.TipoStatus == "AL" ? "Alteração"
															: item.TipoStatus == "CA" ? "Cancelamento"
															: "-";
			}

			return lista;
		}
		
		public ParecerTecnicoVM Selecionar(int id)
		{
			var pe = _uowSciex.QueryStackSciex.Processo.SelecionarGrafo(o => new ProcessoExportacaoVM()
			{
				IdProcesso = o.IdProcesso,
				NumeroProcesso = o.NumeroProcesso,
				AnoProcesso = o.AnoProcesso,
				InscricaoSuframa = o.InscricaoSuframa,
				RazaoSocial = o.RazaoSocial,
				TipoModalidade = o.TipoModalidade,
				TipoStatus = o.TipoStatus,
				DataValidade = o.DataValidade,
				ValorPremio = o.ValorPremio,
				ValorPercentualIndImportado = o.ValorPercentualIndImportado,
				ValorPercentualIndNacional = o.ValorPercentualIndNacional,
				Cnpj = o.Cnpj,
				ListaStatus = o.ListaStatus.Select(q => new PRCStatusVM()
				{
					IdStatus = q.IdStatus,
					IdProcesso = q.IdProcesso,
					Tipo = q.Tipo,
					Data = q.Data,
					DataValidade = q.DataValidade,
					CpfResponsavel = q.CpfResponsavel,
					NomeResponsavel = q.NomeResponsavel,
					NumeroPlano = q.NumeroPlano,
					AnoPlano = q.AnoPlano
				}
				).ToList(),

				ListaProduto = o.ListaProduto.Select(q => new PRCProdutoVM()
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
				}).ToList()
			}
			, o => o.IdProcesso == id);

			pe.NumeroAnoProcessoFormatado = Convert.ToInt32(pe.NumeroProcesso).ToString("D4") + "/" + pe.AnoProcesso;

			var ultimoStatus = pe.ListaStatus.LastOrDefault();

			pe.NumeroAnoPlanoFormatado = Convert.ToInt32(ultimoStatus.NumeroPlano).ToString("D5") + "/" + ultimoStatus.AnoPlano;

			pe.DataValidadeFormatada = pe.DataValidade == DateTime.MinValue ? DateTime.MinValue.ToShortDateString() : ((DateTime)pe.DataValidade).ToShortDateString();

			pe.TipoModalidadeString = pe.TipoModalidade == "S" ? "SUSPENSÃO"
																		: pe.TipoModalidade == "I" ? "ISENÇÃO"
																		: "-"
																		;

			pe.TipoStatusString = pe.TipoStatus.Equals("AP") ? "APROVADO"
																			: "-";

			foreach (var produto in pe.ListaProduto)
			{

				var dadosPrj = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
					.Listar(x => x.CodigoNCM == produto.CodigoNCM && x.CodigoTipoProduto == produto.TipoProduto && x.CodigoProduto == produto.CodigoProdutoSuframa
					&& x.InscricaoCadastral == pe.InscricaoSuframa).FirstOrDefault();
				if(dadosPrj != null)
				{
					produto.DescricaoNCM = dadosPrj.DescricaoNCM;
					produto.DescricaoProduto = dadosPrj.DescricaoProduto;
					produto.DescricaoTipoProduto = dadosPrj.DescricaoTipoProduto;
					
				}

				var codigoUnidadeMedida = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == produto.CodigoUnidade);
				if (codigoUnidadeMedida != null)
				{
					produto.DescricaoUnidadeMedida = codigoUnidadeMedida.Descricao;
				}
				


				pe.InsumosAprovados.Nacionais += produto.ListaInsumos.Sum(q => q.ValorNacionalAprovado);
				pe.InsumosAprovados.ImportadosFOB += produto.ListaInsumos.Sum(q => q.ValorDolarAprovado);
				pe.InsumosAprovados.Frete += produto.ListaInsumos.Sum(q => q.ValorFreteAprovado);
				pe.InsumosAprovados.TotalFOB += produto.ListaInsumos.Sum(q => q.ValorNacionalAprovado + q.ValorDolarAprovado);

				pe.Saldos.Nacionais += produto.ListaInsumos.Sum(q => q.ValorNacionalAprovado);
				pe.Saldos.Importados += produto.ListaInsumos.Sum(q => q.ValorDolarSaldo);
			}



			return new ParecerTecnicoVM();
		}

	}
}