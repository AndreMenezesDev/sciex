using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class RelatorioListagemHistoricoInsumosBll : IRelatorioListagemHistoricoInsumosBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public RelatorioListagemHistoricoInsumosBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public List<RelatorioListagemHistoricoInsumosVM> BuscarRelatorio (RelatorioListagemHistoricoInsumosVM filterVm)
		{
			var retornoMetodo = new List<RelatorioListagemHistoricoInsumosVM>();

			var listaLEProdutoEntity = GetListaProdutos(filterVm);

			if (listaLEProdutoEntity.Count == 0) { return null; }

			retornoMetodo = listaLEProdutoEntity.Select(x => new RelatorioListagemHistoricoInsumosVM {
				NomeEmpresa = x.RazaoSocial,
				InscricaoCadastral = x.InscricaoCadastral,
				CodigoProduto = x.CodigoProduto,
				DescricaoModelo = x.DescricaoModelo,
				DataImpressao = DateTime.Now.ToString("dd/MM/yy hh:mm:ss"),
				Insumos = (x.LEInsumo.Count > 0) ? 
								x.LEInsumo.Select(y => new InsumosRelatorioListagemHistoricoVM {
														CodigoInsumo = y.CodigoInsumo,
														DescricaoInsumo = y.DescricaoInsumo,
														DescricaoTipoAlteracao = getDescricaoTipoAlteracaoInsumo(y.TipoInsumoAlteracao),
														SituacaoInsumo = getDescricaoSituacaoInsumo(y.SituacaoInsumo),
														Justificativa = (y.SituacaoInsumo == (int)EnumSituacaoInsumo.INATIVO_OU_NAO && y.listaLEInsumoErro.Count > 0) ?
																			GetJustificativaDeErro(y.listaLEInsumoErro.ToList()) : 
																				"--"
								}).ToList() 
						 : null
			})
			.ToList();

			return retornoMetodo;
		}

		private string GetJustificativaDeErro(List<LEInsumoErroEntity> listaLEInsumoEntity)
		{
			var justificativa = listaLEInsumoEntity.Where(x => x.DescricaoErro != null)?.LastOrDefault().DescricaoErro;
			return justificativa == null ? "Nenhuma justificativa de erro encontrada." : justificativa;
		}
			
		private List<LEProdutoEntity> GetListaProdutos(RelatorioListagemHistoricoInsumosVM filterVm) =>
				_uowSciex.QueryStackSciex.LEProduto.Listar(x => 
																(filterVm.InscricaoCadastral == 0 || x.InscricaoCadastral.ToString().StartsWith(filterVm.InscricaoCadastral.ToString())) &&
																(string.IsNullOrEmpty(filterVm.NomeEmpresa) || x.RazaoSocial.ToUpper().Contains(filterVm.NomeEmpresa.ToUpper())) &&
																(filterVm.CodigoProduto == 0 || x.CodigoProduto == filterVm.CodigoProduto)
														  );

		private string getDescricaoTipoAlteracaoInsumo(int? tipoAlteracao) =>
			tipoAlteracao == (int?)EnumTipoAlteracaoInsumo.INCLUSAO_INSUMO ? "Inclusão insumo" :
			tipoAlteracao == (int?)EnumTipoAlteracaoInsumo.MOEDA ? "Moeda" :
			tipoAlteracao == (int?)EnumTipoAlteracaoInsumo.PAIS ? "País" :
			tipoAlteracao == (int?)EnumTipoAlteracaoInsumo.QUANTIDADE_COEF_TECNICO ? "Quantidade coef. técnico" :
			tipoAlteracao == (int?)EnumTipoAlteracaoInsumo.TRANSFERENCIA_SALDO_INSUMO ? "Transferência saldo insumo" :
			tipoAlteracao == (int?)EnumTipoAlteracaoInsumo.VALOR_FRETE ? "Valor frete" :
			tipoAlteracao == (int?)EnumTipoAlteracaoInsumo.VALOR_UNITARIO ? "Valor unitário" :
				"Tipo de alteração não encontrado";

		private string getDescricaoSituacaoInsumo(int? situacaoInsumo) =>
			situacaoInsumo == (int?)EnumSituacaoInsumo.ALTERADO ? "Alterado" :
			situacaoInsumo == (int?)EnumSituacaoInsumo.ATIVO_OU_SIM ? "Ativo" :
			situacaoInsumo == (int?)EnumSituacaoInsumo.CANCELADO ? "Cancelado" :
			situacaoInsumo == (int?)EnumSituacaoInsumo.INATIVO_OU_NAO ? "Inativo" :
				"--";

	}
}
