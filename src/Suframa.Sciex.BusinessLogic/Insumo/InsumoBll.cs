using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class InsumoBll: IInsumoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public InsumoBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioPssBll = usuarioPssBll;
		}

		public IEnumerable<object> PesquisarInsumo(InsumoVM InsumoVM)
		{
			if (InsumoVM.Descricao == null && InsumoVM.Id == null)
			{
				return new List<object>();
			}

			var dadosUsuarioLogado = _usuarioPssBll.ObterUsuarioLogado().Perfis;

			if (dadosUsuarioLogado.Contains(EnumPerfil.Preposto))
			{
				InsumoVM.cnpjLogado = _usuarioPssBll.ObterUsuarioLogado().usuCnpjRepresentanteLogado.CnpjUnformat();
			}
			else
			{
				InsumoVM.cnpjLogado = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.CnpjUnformat();
			}


			var insumo = _uowSciex.QueryStackSciex.ListarSql<InsumoVM>(this.MontarConsultaInsumo(InsumoVM))
	  											  .OrderBy(o => o.DescricaoInsumo)
												  .Select(
															s => new
															{
																id = s.CodigoInsumo == null ? "" : s.CodigoInsumo.ToString(),
																text = s.DescricaoInsumo == null ? "" : s.CodigoInsumo.ToString() + " | " + s.DescricaoInsumo
															});

			return insumo;

		}

		private string MontarConsultaInsumo(InsumoVM parametros)
		{
			StringBuilder strQuery = new StringBuilder();

			if (!string.IsNullOrEmpty(parametros.Descricao))
			{
				//valida se o parametro de pesquisa é pelo codigo ou a descrição do insumo
				if (char.IsNumber(parametros.Descricao, 0))
				{
					parametros.Id = int.Parse(parametros.Descricao);
					parametros.Descricao = string.Empty;
				}
			}

			strQuery.AppendLine("select sub.CodigoInsumo, sub.DescricaoInsumo");
			strQuery.AppendLine("from");
			strQuery.AppendLine("(SELECT A.lei_co_insumo CodigoInsumo, A.lei_ds_insumo DescricaoInsumo");
			strQuery.AppendLine("FROM SCIEX_LE_INSUMO A, SCIEX_LE_PRODUTO B");
			strQuery.AppendLine("WHERE A.LEP_ID = B.LEP_ID");
			if (!string.IsNullOrEmpty(parametros.Descricao))
			{
				strQuery.AppendLine("    AND A.LEI_CO_PRODUTO = " + parametros.CodigoProdutoExportacao);
				strQuery.AppendLine("    AND A.lei_ds_insumo LIKE '%"+ parametros.Descricao.ToUpper() + "%'");
			}
			if (!string.IsNullOrEmpty(parametros.Id.ToString()) && parametros.CodigoProdutoExportacao > 0)
			{
				strQuery.AppendLine(" AND A.lei_co_insumo ='"+parametros.Id+"'");
				strQuery.AppendLine("    AND A.LEI_CO_PRODUTO = " + parametros.CodigoProdutoExportacao);
			}

			if (!string.IsNullOrEmpty(parametros.Id.ToString()) && parametros.CodigoProdutoExportacao == 0)
			{
				strQuery.AppendLine(" AND A.lei_co_insumo ='" + parametros.Id + "'");
			}
			strQuery.AppendLine("    AND A.lei_st_insumo = 1 --(ATIVO)");
			strQuery.AppendLine("	 AND A.LEI_TP IN ('P', 'E')");
			strQuery.AppendLine("	 AND B.LEP_NU_CNPJ = '" + parametros.cnpjLogado + "') sub");
			strQuery.AppendLine("where not exists( select * from SCIEX_PRC_INSUMO where ins_co_insumo = CodigoInsumo)");

			return strQuery.ToString();
		}
	}
}
