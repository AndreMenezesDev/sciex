using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class SolicitacaoAlteracaoBll : ISolicitacaoAlteracaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public SolicitacaoAlteracaoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public List<PRCSolicDetalheVM> BuscarDados(SolicitacoesAlteracaoVM objeto)
		{
			var emAlteracao = 2;
			var insumoEmAlteracao = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo
																				     && o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto
																				     && o.StatusInsumo == emAlteracao);

			if(insumoEmAlteracao == null) //se nao existe insumo duplicado, nao existe solicitações ainda.
				return null;

			var PRCDetalheInsumoEntity = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar(o => o.IdPrcInsumo == insumoEmAlteracao.IdInsumo);

			int emElaboracao = 1;
			var _solicitacaoAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.IdProcesso == objeto.IdProcesso
																											 &&
																											 o.Status == emElaboracao);

			if (_solicitacaoAlteracaoEntity == null)
				return null;

			var _solicitacaoDetalheEntity = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Listar(o => o.IdSolicitacaoAlteracao == _solicitacaoAlteracaoEntity.Id
																							   && o.IdDetalheInsumo == PRCDetalheInsumoEntity.IdDetalheInsumo
																							   && o.IdInsumo == insumoEmAlteracao.IdInsumo);

			if (_solicitacaoDetalheEntity.Count > 0)
			{
				var retornoMetodo = new List<PRCSolicDetalheVM>();
								
				foreach (var item in _solicitacaoDetalheEntity)
				{
					var PRCSolicDetalheVM = new PRCSolicDetalheVM() {
						DescricaoTipoAlteracao = item.TipoSolicAlteracao.Descricao,
						DescricaoDe = item.DescricaoDe,
						DescricaoPara = item.DescricaoPara
					};
					retornoMetodo.Add(PRCSolicDetalheVM);
				}
				return retornoMetodo;
			}
			else
			{
				return null;
			}
		}

		public IEnumerable<object> ListarChave(PRCSolicitacaoAlteracaoVM view)
		{

			if (string.IsNullOrEmpty(view.Descricao) && view.Id == 0)
			{
				return new List<object>();
			}

			view.Descricao = view.Descricao != null ? view.Descricao.TrimStart('0') : null;

			var idProcesso = _uowSciex.QueryStackSciex.PRCProduto.Selecionar(o => o.IdProduto == view.Id).IdProcesso;

			var ncm = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao
				.Listar()
				.Where(o =>
						//(string.IsNullOrEmpty(view.Descricao)
						//	||
						//	//o.DescricaoInsumo.ToLower().Contains(view.Descricao.ToLower())
						//	||o.Id.ToString().Contains(view.Descricao.ToString())
						//)
						//&&
						(idProcesso == 0
							||
							o.IdProcesso == idProcesso
						)
						&&
						o.Status == 3 // (STATUS -> 3 = EM ANALISE)
					   )
				.Select(
					s => new
					{
						id = s.Id,
						text = s.NumeroSolicitacao + "/" + s.AnoSolicitacao
					})
				.Distinct().OrderBy(q => q.id);

			return ncm;
		}

	}
}
