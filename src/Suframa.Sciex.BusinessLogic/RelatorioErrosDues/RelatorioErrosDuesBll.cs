using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class RelatorioErrosDuesBll : IRelatorioErrosDuesBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public RelatorioErrosDuesBll(IUnitOfWorkSciex uowSciex,
									 IUnitOfWork uowCadsuf,
									 IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_usuarioPssBll = usuarioPssBll;
		}

		public RelatorioErrosDuesVM GerarRelatorio(RelatorioErrosDuesVM filterVm)
		{
			var retornoMetodo = new RelatorioErrosDuesVM();

			try
			{
				var _listaEntityPlanoDeExportacao = GetListaPlanoExportacao(filterVm);

				if (_listaEntityPlanoDeExportacao.Count == 0)
				{
					retornoMetodo.StatusCode = (int)HttpStatusCode.NotFound;
					retornoMetodo.TextResponse = "Plano de exportação não encontrado!";
					return retornoMetodo;
				}

				var planoExportacaoEntity = _listaEntityPlanoDeExportacao.FirstOrDefault();

				#region Informações relatório

				retornoMetodo.NomeEmpresa = planoExportacaoEntity.RazaoSocial;
				retornoMetodo.NumeroPlano = planoExportacaoEntity.NumeroPlano;
				retornoMetodo.NumeroPlanoFormated = planoExportacaoEntity.NumeroPlano.ToString("D5");
				retornoMetodo.AnoPlano = planoExportacaoEntity.AnoPlano;
				retornoMetodo.AnoNumPlano = planoExportacaoEntity.AnoPlano + "/" + planoExportacaoEntity.NumeroPlano.ToString("D5");
				retornoMetodo.Modalidade = planoExportacaoEntity.TipoModalidade;
				retornoMetodo.Tipo = planoExportacaoEntity.TipoExportacao;
				retornoMetodo.DataStatus = planoExportacaoEntity.DataStatus?.ToString("dd/MM/yyyy");
				retornoMetodo.DataRecebimento = "--";
				retornoMetodo.DataImpressao = DateTime.Now.ToString("dd/MM/yy");
				retornoMetodo.AnoNumProcesso = planoExportacaoEntity.NumeroAnoProcesso + "/" + planoExportacaoEntity.NumeroProcesso?.ToString("D5");

				#endregion

				var _listaEntityPlanoDeExportacaoProduto = GetListaPlanoExportacaoProduto(_listaEntityPlanoDeExportacao.Select(x=>x.IdPlanoExportacao).ToList());

				if (_listaEntityPlanoDeExportacaoProduto.Count == 0)
				{
					retornoMetodo.StatusCode = (int)HttpStatusCode.NotFound;
					retornoMetodo.TextResponse = "Plano de exportação produto não encontrado!";
					return retornoMetodo;
				}

				var _listaEntityPlanoDeExportacaoProdutoPais = GetListaPlanoExportacaoProdutoPais(_listaEntityPlanoDeExportacaoProduto.Select(x => (int?)x.IdPEProduto).ToList());

				if (_listaEntityPlanoDeExportacaoProdutoPais.Count == 0)
				{
					retornoMetodo.StatusCode = (int)HttpStatusCode.NotFound;
					retornoMetodo.TextResponse = "Plano de exportação produto país não encontrado!";
					return retornoMetodo;
				}

				var _listaEntityPlanoDeExportacaoDUE = GetListaPlanoExportacaoDUE(_listaEntityPlanoDeExportacaoProdutoPais.Select(x => (int?)x.IdPEProdutoPais).ToList());

				if (_listaEntityPlanoDeExportacaoDUE.Count == 0)
				{
					retornoMetodo.StatusCode = (int)HttpStatusCode.NotFound;
					retornoMetodo.TextResponse = "Plano de exportação - DUE não encontrado!";
					return retornoMetodo;
				}

				retornoMetodo.RelatorioHistoricoAnalise = GetRelatorioHistoricoAnalise(_listaEntityPlanoDeExportacaoDUE, 
																					   planoExportacaoEntity.NomeResponsavel);

				retornoMetodo.RelatorioDePara = GetRelatorioDeParaDUE(_listaEntityPlanoDeExportacaoDUE,
																	  _listaEntityPlanoDeExportacaoProdutoPais,
																	  planoExportacaoEntity.NomeResponsavel);
				retornoMetodo.StatusCode = (int)HttpStatusCode.OK;
			}
			catch(Exception ex)
			{
				retornoMetodo.StatusCode = (int)HttpStatusCode.InternalServerError;
				retornoMetodo.TextResponse = " Falha ao Gerar Relatório! [Ex]: " + ex.InnerException +
											 " [Message]: " + ex.Message;
			}


			return retornoMetodo;
		}

		private List<PlanoExportacaoEntity> GetListaPlanoExportacao(RelatorioErrosDuesVM filterVm) =>
				_uowSciex.QueryStackSciex.PlanoExportacao.Listar(x => (filterVm.InscricaoCadastral == null || x.NumeroInscricaoCadastral == filterVm.InscricaoCadastral) &&
																	  (string.IsNullOrEmpty(filterVm.NomeEmpresa) || x.RazaoSocial.ToUpper().Contains(filterVm.NomeEmpresa.ToUpper())) &&
																	  (filterVm.NumeroPlano == 0 || x.NumeroPlano == filterVm.NumeroPlano) && 
																	  (filterVm.AnoPlano == 0 || x.AnoPlano == filterVm.AnoPlano));

		private List<PEProdutoEntity> GetListaPlanoExportacaoProduto(List<int> idsPlanoExportacao) =>
				_uowSciex.QueryStackSciex.PlanoExportacaoProduto.Listar(x=> idsPlanoExportacao.Contains(x.IdPlanoExportacao));

		private List<PEProdutoPaisEntity> GetListaPlanoExportacaoProdutoPais(List<int?> idsPlanoExportacaoProduto) =>
				_uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(x => idsPlanoExportacaoProduto.Contains(x.IdPEProduto));

		private List<PlanoExportacaoDUEEntity> GetListaPlanoExportacaoDUE(List<int?> idsPlanoExportacaoProdutoPais) =>
				_uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(x => idsPlanoExportacaoProdutoPais.Contains(x.IdPEProdutoPais));

		private List<RelatorioDuesVM> GetRelatorioHistoricoAnalise(List<PlanoExportacaoDUEEntity> listaDuesEntity, string NomeAnalista) =>
			listaDuesEntity.Select(_due => new RelatorioDuesVM
			{
				Codigo = _due.CodigoPais,
				NumeroDue = Convert.ToInt64(_due.Numero).ToString("D9"),
				Situacao = GetSituacaoDue(_due.SituacaoAnalise),
				Responsavel = NomeAnalista,
				Justificativa = (_due.DescricaoJustificativa == null) ? "--" : _due.DescricaoJustificativa
			})
			.ToList();

		public List<RelatorioDuesVM> GetRelatorioDeParaDUE(List<PlanoExportacaoDUEEntity> listaDuesEntity, List<PEProdutoPaisEntity> listaPaises, string NomeAnalista)
		{
			var retornoMetodo = new List<RelatorioDuesVM>();

			var duesAlteradas = listaDuesEntity.Where(x=> x.SituacaoAnalise == (int)EnumSituacaoAnaliseDUE.ALTERADO)
														.OrderBy(x => x.Numero)
													    .Select(x => x.Numero)
														.Distinct()
														.ToList();

			if (duesAlteradas.Count == 0) { return null; }

			foreach(var _numeroDue in duesAlteradas)
			{
				var duesFiltradas = listaDuesEntity.Where(x => x.Numero == _numeroDue)
												   .OrderBy(x => x.DataAverbacao)
												   .ToList();
				foreach(var _due in duesFiltradas)
				{
					var item = new RelatorioDuesVM
					{
						Codigo = _due.CodigoPais,
						NumeroDue = Convert.ToInt64(_due.Numero).ToString("D9"),
						Situacao = GetSituacaoDue(_due.SituacaoAnalise),
						Responsavel = NomeAnalista,
						Justificativa = (_due.DescricaoJustificativa == null) ? "--" : _due.DescricaoJustificativa,
						DataAverbacao = _due.DataAverbacao.ToString("dd/MM/yyyy"),
						Quantidade = _due.Quantidade,
						Valor = _due.ValorDolar,
						PaisDestino = _uowSciex.QueryStackSciex.ViewPais.Selecionar(x=>x.CodigoPais == _due.CodigoPais.ToString()).Descricao
					};
					retornoMetodo.Add(item);
				}
			}
			return retornoMetodo;
		}

		private string GetSituacaoDue (int? situacaoAnalise) =>
			situacaoAnalise == (int)EnumSituacaoAnaliseDUE.APROVADO ? "Aprovado" :
			situacaoAnalise == (int)EnumSituacaoAnaliseDUE.REPROVADO ? "Reprovado" :
			situacaoAnalise == (int)EnumSituacaoAnaliseDUE.ALTERADO ? "Alterado" :
			situacaoAnalise == (int)EnumSituacaoAnaliseDUE.CORRIGIDO ? "Corrigido" :
			situacaoAnalise == (int)EnumSituacaoAnaliseDUE.INATIVO ? "Inativo" :
			situacaoAnalise == (int)EnumSituacaoAnaliseDUE.NOVO ? "Novo" :
				"--";

	}
}
