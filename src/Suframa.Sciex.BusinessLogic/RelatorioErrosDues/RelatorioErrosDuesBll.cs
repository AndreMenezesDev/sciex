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
		int[] situacaoPlanoExportacaoFiltragem = new int[3] { (int)EnumSituacaoPlanoExportacao.AGUARDANDO_ANÁLISE, (int)EnumSituacaoPlanoExportacao.DEFERIDO, (int)EnumSituacaoPlanoExportacao.INDEFERIDO };

		public RelatorioErrosDuesBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public List<RelatorioErrosDuesVM> GerarRelatorio(RelatorioErrosDuesVM filterVm)
		{
			var retornoMetodo = new List<RelatorioErrosDuesVM>();
			
			var _listaEntityPlanoDeExportacao = GetListaPlanoExportacao(filterVm);

			if (_listaEntityPlanoDeExportacao.Count == 0){ return null; }

			foreach(var planoExportacaoEntity in _listaEntityPlanoDeExportacao)
			{
				var objetoRelatorio = new RelatorioErrosDuesVM
				{
					NomeEmpresa = planoExportacaoEntity.RazaoSocial,
					NumeroPlano = planoExportacaoEntity.NumeroPlano,
					NumeroPlanoFormated = planoExportacaoEntity.NumeroPlano.ToString("D5"),
					AnoPlano = planoExportacaoEntity.AnoPlano,
					AnoNumPlano = planoExportacaoEntity.NumeroPlano.ToString("D5") + "/" + planoExportacaoEntity.AnoPlano ,
					Modalidade = planoExportacaoEntity.TipoModalidade,
					Tipo = planoExportacaoEntity.TipoExportacao,
					DataStatus = planoExportacaoEntity.DataStatus?.ToString("dd/MM/yyyy"),
					DataRecebimento = planoExportacaoEntity.DataEnvio?.ToString("dd/MM/yyyy"),
					DataImpressao = DateTime.Now.ToString("dd/MM/yy"),
					AnoNumProcesso = planoExportacaoEntity.NumeroProcesso?.ToString("D4") + "/" + planoExportacaoEntity.NumeroAnoProcesso
				};
				
				var _relatorioHistoricoAnalise = new List<DadosDuesVM>();

				var _relatorioDePara = new List<DadosDuesVM>();

				bool permiteAdicionarNoRelatorio = false;

				var _listaEntityPlanoDeExportacaoProduto = GetListaPlanoExportacaoProduto(planoExportacaoEntity.IdPlanoExportacao);

				if (_listaEntityPlanoDeExportacaoProduto.Count > 0)
				{
					foreach (var planoExportacaoProduto in _listaEntityPlanoDeExportacaoProduto)
					{
						var _listaEntityPlanoDeExportacaoProdutoPais = GetListaPlanoExportacaoProdutoPais(planoExportacaoProduto.IdPEProduto);

						if (_listaEntityPlanoDeExportacaoProdutoPais.Count > 0)
						{
							foreach (var planoExportacaoProdutoPais in _listaEntityPlanoDeExportacaoProdutoPais)
							{
								var _listaEntityPlanoDeExportacaoDUE = GetListaPlanoExportacaoDUE(planoExportacaoProdutoPais.IdPEProdutoPais);

								if(_listaEntityPlanoDeExportacaoDUE.Count > 0)
								{
									var relatorioHistoricoAnalise = GetRelatorioHistoricoAnalise(_listaEntityPlanoDeExportacaoDUE, planoExportacaoEntity.NomeResponsavel);

									relatorioHistoricoAnalise.ForEach(x => _relatorioHistoricoAnalise.Add(x));

									var relatorioDePara = GetRelatorioDeParaDUE(_listaEntityPlanoDeExportacaoDUE, _listaEntityPlanoDeExportacaoProdutoPais,	planoExportacaoEntity.NomeResponsavel);

									relatorioDePara?.ForEach(x => _relatorioDePara.Add(x));

									permiteAdicionarNoRelatorio = true;
								}
							}
						}
					}
				}
				if (permiteAdicionarNoRelatorio)
				{
					objetoRelatorio.Relatorios = new RelatoriosDuesVM
					{
						RelatorioDePara = _relatorioDePara,
						RelatorioHistoricoAnalise = _relatorioHistoricoAnalise
					};
					retornoMetodo.Add(objetoRelatorio);
				}
			}
			return retornoMetodo;
		}

		private List<PlanoExportacaoEntity> GetListaPlanoExportacao(RelatorioErrosDuesVM filterVm) =>
				_uowSciex.QueryStackSciex.PlanoExportacao.Listar(x => (filterVm.InscricaoCadastral == null || x.NumeroInscricaoCadastral == filterVm.InscricaoCadastral) &&
																	  (string.IsNullOrEmpty(filterVm.NomeEmpresa) || x.RazaoSocial.ToUpper().Contains(filterVm.NomeEmpresa.ToUpper())) &&
																	  (filterVm.NumeroPlano == 0 || x.NumeroPlano == filterVm.NumeroPlano) && 
																	  (filterVm.AnoPlano == 0 || x.AnoPlano == filterVm.AnoPlano) && 
																      x.TipoExportacao == "CO" && //COMPROVAÇÃO
																	  situacaoPlanoExportacaoFiltragem.Contains(x.Situacao)
																); 

		private List<PEProdutoEntity> GetListaPlanoExportacaoProduto(int idsPlanoExportacao) =>
				_uowSciex.QueryStackSciex.PlanoExportacaoProduto.Listar(x => x.IdPlanoExportacao == idsPlanoExportacao);

		private List<PEProdutoPaisEntity> GetListaPlanoExportacaoProdutoPais(int? idsPlanoExportacaoProduto) =>
				_uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(x => x.IdPEProduto == idsPlanoExportacaoProduto);

		private List<PlanoExportacaoDUEEntity> GetListaPlanoExportacaoDUE(int? idsPlanoExportacaoProdutoPais) =>
				_uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(x => x.IdPEProdutoPais == idsPlanoExportacaoProdutoPais);

		private List<DadosDuesVM> GetRelatorioHistoricoAnalise(List<PlanoExportacaoDUEEntity> listaDuesEntity, string NomeAnalista) =>
			listaDuesEntity.Select(_due => new DadosDuesVM
			{
				Codigo = _due.CodigoPais,
				NumeroDue = _due.Numero,
				Situacao = GetSituacaoDue(_due.SituacaoAnalise),
				Responsavel = NomeAnalista,
				Justificativa = (_due.DescricaoJustificativa == null) ? "--" : _due.DescricaoJustificativa
			})
			.ToList();

		public List<DadosDuesVM> GetRelatorioDeParaDUE(List<PlanoExportacaoDUEEntity> listaDuesEntity, List<PEProdutoPaisEntity> listaPaises, string NomeAnalista)
		{
			var retornoMetodo = new List<DadosDuesVM>();

			var duesAlteradas = listaDuesEntity.Where(x=> x.SituacaoAnalise == (int)EnumSituacaoAnaliseDUE.ALTERADO)
											   .OrderBy(x => x.Numero)
											   .Select(x => x.Numero)
											   .Distinct()
											   .ToList();

			if (duesAlteradas.Count == 0) { return null; }

			foreach(var _numeroDue in duesAlteradas)
			{
				var duesFiltradas = listaDuesEntity.Where(x => x.Numero == _numeroDue)
												   .OrderBy(x => x.IdDue)
												   .ToList();
				foreach(var _due in duesFiltradas)
				{
					var item = new DadosDuesVM
					{
						Codigo = _due.CodigoPais,
						NumeroDue = _due.Numero,
						Situacao = GetSituacaoDue(_due.SituacaoAnalise),
						Responsavel = NomeAnalista,
						Justificativa = (_due.DescricaoJustificativa == null) ? "--" : _due.DescricaoJustificativa,
						DataAverbacao = _due.DataAverbacao.ToString("dd/MM/yyyy"),
						Quantidade = _due.Quantidade,
						Valor = _due.ValorDolar,
						PaisDestino = _uowSciex.QueryStackSciex.ViewPais.Selecionar(x => x.CodigoPais == _due.CodigoPais.ToString()).Descricao
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
