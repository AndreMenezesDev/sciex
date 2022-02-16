using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Suframa.Sciex.BusinessLogic
{
	public class ErroProcessamentoBll : IErroProcessamentoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public ErroProcessamentoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;

		}


		public IEnumerable<ErroProcessamentoVM> Listar(ErroProcessamentoVM ErroProcessamentoVM)
		{
			var ErroProcessamento = _uowSciex.QueryStackSciex.ErroProcessamento.Listar<ErroProcessamentoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<ErroProcessamentoVM>>(ErroProcessamento);
		}

		public PagedItems<ErroProcessamentoVM> ListarPaginado(ErroProcessamentoVM pagedFilter)
		{
			try
			{
				if (pagedFilter == null) { return new PagedItems<ErroProcessamentoVM>(); }

				//Verifica cada item da lista da tabela de erro processamento se há erro no PLI
				var ErroProcessamento = _uowSciex.QueryStackSciex.ErroProcessamento.ListarPaginado<ErroProcessamentoVM>(o =>
					(
						(
							pagedFilter.IdDiEntrada == null || o.IdDiEntrada == pagedFilter.IdDiEntrada
						) &&
						(
							pagedFilter.IdPli == null || o.IdPli == pagedFilter.IdPli
						) &&
						(
							pagedFilter.CodigoNivelErro == null || o.CodigoNivelErro == pagedFilter.CodigoNivelErro
						) &&
						(
							pagedFilter.IdErroMensagem == null || o.IdErroMensagem == pagedFilter.IdErroMensagem
						)
						&&
						(
							pagedFilter.IdSolicitacaoPli == null || o.IdSolicitacaoPli == pagedFilter.IdSolicitacaoPli
						)
						&&
						(
							string.IsNullOrEmpty(pagedFilter.NumeroPLIImportador) || o.NumeroPLIImportador.Contains(pagedFilter.NumeroPLIImportador)
						)
					),
					pagedFilter);

				return ErroProcessamento;
			}
			catch
			{
				return new PagedItems<ErroProcessamentoVM>();
			}
		}

		public DadosErroProcessamentoVM ListarErrosLePaginado(EstruturaPropriaLEEntityVM pagedFilter)
		{
			try
			{
				if (pagedFilter == null) { return new DadosErroProcessamentoVM(); }

				var regSolicitacaoLE = _uowSciex.QueryStackSciex.SolicitacaoLeInsumo.SelecionarGrafo(q => new SolicitacaoLeInsumoVM()
				{
					IdEstruturaPropriaLe = q.IdEstruturaPropriaLe,
					IdSolicitacaoLeInsumo = q.IdSolicitacaoLeInsumo,
					CodigoDestaque = q.CodigoDestaque,
					CodigoNCM = q.CodigoNCM,
					TipoInsumo = q.TipoInsumo,
					CodigoUnidade = q.CodigoUnidade,
					DescricaoInsumo = q.DescricaoInsumo,
					ValorCoeficienteTecnico = q.ValorCoeficienteTecnico,
					SituacaoInsumo = q.SituacaoInsumo,
					CodigoPartNumber = q.CodigoPartNumber,
					DescricaoEspecificacaoTecnica = q.DescricaoEspecificacaoTecnica,
					NumeroLinha = q.NumeroLinha,
				}, w=>  w.IdSolicitacaoLeInsumo == pagedFilter.IdSolicitacaoLeInsumo);

				regSolicitacaoLE.QtdErros = _uowSciex.QueryStackSciex.ErroProcessamento.Listar(o => o.IdPliMercadoriaOuPliDetalheMercadoria == regSolicitacaoLE.IdSolicitacaoLeInsumo).Count();

				var estrutura = _uowSciex.QueryStackSciex.EstruturaPropriaPLI.SelecionarGrafo(o => new EstruturaPropriaPLIGrafoVM()
				{
					IdEstruturaPropria = o.IdEstruturaPropria,
					NumeroProtocolo = o.NumeroProtocolo,
					Empresa = o.CNPJImportador + " | " + o.RazaoSocialImportador,
					DataValidacao = o.DataInicioProcessamento != DateTime.MinValue ? o.DataInicioProcessamento.ToString() : "",
					QuantidadePLIProcessadoFalha = o.QuantidadePLIProcessadoFalha
				},

				q =>
				(
					(
						pagedFilter.IdEstruturaPropria == -1 || q.IdEstruturaPropria == regSolicitacaoLE.IdEstruturaPropriaLe
					)
				));


				var ErroProcessamento = _uowSciex.QueryStackSciex.ErroProcessamento.ListarPaginadoGrafo(o => new ErroProcessamentoVM()
				{
					IdErroProcessamento = o.IdErroProcessamento,
					IdDiEntrada = o.IdDiEntrada,
					IdErroMensagem = o.IdErroMensagem,
					IdPli = o.IdPli,
					IdSolicitacaoPli = o.IdSolicitacaoPli,
					IdPliMercadoriaOuPliDetalheMercadoria = o.IdPliMercadoriaOuPliDetalheMercadoria,
					IdPliMercadoria = o.Pli.PLIMercadoria.FirstOrDefault() == null ? 0 : o.Pli.PLIMercadoria.FirstOrDefault().IdPliMercadoria,
					LocalErro = o.CodigoNivelErro == (byte)EnumErroProcessamentoLENivelErro.LE ? EnumErroProcessamentoLENivelErro.LE.ToString().Replace("_", " ") 
															: o.CodigoNivelErro == (byte)EnumErroProcessamentoLENivelErro.PRODUTO ? EnumErroProcessamentoLENivelErro.PRODUTO.ToString().Replace("_", " ")
															: o.CodigoNivelErro == (byte)EnumErroProcessamentoNivelErro.ITEM ? EnumErroProcessamentoNivelErro.ITEM.ToString().Replace("_", " ") 
															: "ERROR"
															,
					MensagemErro = o.Descricao == "-" ? "-" : o.Descricao,
					OrigemErro = o.ErroMensagem == null ? "-" : o.ErroMensagem.CodigoSistemaOrigem == 5 ? "ANÁLISE VISUAL" 
																: o.ErroMensagem.CodigoSistemaOrigem == 6 ? "EXPORTAÇÃO"
																: "-"
				},
				q=>
				(
					pagedFilter.IdSolicitacaoLeInsumo == 0 || q.IdPliMercadoriaOuPliDetalheMercadoria == pagedFilter.IdSolicitacaoLeInsumo
				),
				pagedFilter);


				var dados = new DadosErroProcessamentoVM();

				estrutura.DataValidacao = string.IsNullOrEmpty(estrutura.DataValidacao) ? "-" : (Convert.ToDateTime(estrutura.DataValidacao)).ToShortDateString();

				dados.EstruturaPropriaLE = estrutura;
				dados.listaErroProcessamentoPaginada = ErroProcessamento;
				dados.SolicitacaoLeInsumo = regSolicitacaoLE;

				return dados;
			}
			catch (Exception ex)
			{
				return new DadosErroProcessamentoVM();
			}
		}

		public DadosErroProcessamentoPEVM ListarErrosPEPaginado(EstruturaPropriaPEVM pagedFilter)
		{
			try
			{
				if (pagedFilter == null) { return new DadosErroProcessamentoPEVM(); }

				var estrutura = _uowSciex.QueryStackSciex.EstruturaPropriaPLI.SelecionarGrafo(o => new EstruturaPropriaPEVM()
				{
					IdEstruturaPropria = o.IdEstruturaPropria,
					NumeroProtocolo = o.NumeroProtocolo,
					Empresa = o.CNPJImportador + " | " + o.RazaoSocialImportador,
					DataValidacao = o.DataInicioProcessamento != DateTime.MinValue ? o.DataInicioProcessamento.ToString() : "",
					QuantidadePLIProcessadoFalha = o.QuantidadePLIProcessadoFalha
				},

				q =>
				(
					(
						pagedFilter.IdEstruturaPropria == -1 || q.IdEstruturaPropria == pagedFilter.IdEstruturaPropria
					)
				));

				var solicPELote = _uowSciex.QueryStackSciex.SolicitacaoPELote.Listar<SolicitacaoPELoteVM>(o => o.EspId == estrutura.IdEstruturaPropria);
				var obj = solicPELote.FirstOrDefault();
				estrutura.SolicitacaoPELote = obj;

				var solicPEProtudo = _uowSciex.QueryStackSciex.SolicitacaoPEProduto.Selecionar(o => o.LoteId == pagedFilter.IdLote);

				estrutura.PlanoExportacaoConcatenado = obj.NumeroLote + "/" + obj.Ano;
				estrutura.ModalidadeDescricao = obj.TipoModalidade == "S" ? "Suspensão" : obj.TipoExportacao == "I" ? "Isenção" : obj.TipoExportacao == "R" ? "Restituição" : "-";
				estrutura.TipoExportacaoDescricao = obj.TipoExportacao == "AP" ? "Aprovação" : obj.TipoExportacao == "CO" ? "Comprovação" : "-";
				estrutura.ProdutoExportacaoDescricao = solicPEProtudo.CodigoProdutoPexPam.ToString();
				estrutura.QtdErros = _uowSciex.QueryStackSciex.ErroProcessamento.Listar(o => o.IdLote == pagedFilter.IdLote).Count();

				var ErroProcessamento = _uowSciex.QueryStackSciex.ErroProcessamento.ListarPaginadoGrafo(o => new ErroProcessamentoVM()
				{
					IdErroProcessamento = o.IdErroProcessamento,
					IdDiEntrada = o.IdDiEntrada,
					IdErroMensagem = o.IdErroMensagem,
					IdPli = o.IdPli,
					IdSolicitacaoPli = o.IdSolicitacaoPli,
					IdLote = o.IdLote,
					IdPliMercadoriaOuPliDetalheMercadoria = o.IdPliMercadoriaOuPliDetalheMercadoria,
					IdPliMercadoria = o.Pli.PLIMercadoria.FirstOrDefault() == null ? 0 : o.Pli.PLIMercadoria.FirstOrDefault().IdPliMercadoria,
					LocalErro = o.CodigoNivelErro == (byte)EnumErroProcessamentoLENivelErro.LE ? EnumErroProcessamentoLENivelErro.LE.ToString().Replace("_", " ")
															: o.CodigoNivelErro == (byte)EnumErroProcessamentoLENivelErro.PRODUTO ? EnumErroProcessamentoLENivelErro.PRODUTO.ToString().Replace("_", " ")
															: o.CodigoNivelErro == (byte)EnumErroProcessamentoNivelErro.ITEM ? EnumErroProcessamentoNivelErro.ITEM.ToString().Replace("_", " ")
															: "ERROR"
															,
					MensagemErro = o.Descricao == "-" ? "-" : o.Descricao,
					OrigemErro = o.ErroMensagem == null ? "-" : o.ErroMensagem.CodigoSistemaOrigem == 5 ? "ANÁLISE VISUAL"
																: o.ErroMensagem.CodigoSistemaOrigem == 6 ? "EXPORTAÇÃO"
																: "-"
				},
				q =>
				(
					pagedFilter.IdLote == 0 || q.IdLote == pagedFilter.IdLote
				),
				pagedFilter);


				var dados = new DadosErroProcessamentoPEVM();

				estrutura.DataValidacao = string.IsNullOrEmpty(estrutura.DataValidacao) ? "-" : (Convert.ToDateTime(estrutura.DataValidacao)).ToShortDateString();

				dados.EstruturaPropriaPE = estrutura;
				dados.listaErroProcessamentoPaginada = ErroProcessamento;

				return dados;
			}
			catch (Exception ex)
			{
				return new DadosErroProcessamentoPEVM();
			}
		}
	}

}