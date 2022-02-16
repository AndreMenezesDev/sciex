using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{ 
	public class SolicitarInclusaoInsumoBll : ISolicitarInclusaoInsumoBll
	{

		private readonly IUnitOfWorkSciex _uowSciex;

		public SolicitarInclusaoInsumoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}
		
		public PagedItems<LEInsumoVM> ListarPaginado(LEInsumoVM objeto)
		{		
			var insumosIncluidosNoProcesso = _uowSciex.QueryStackSciex.PRCInsumo.Listar(o => o.IdPrcProduto == objeto.IdProduto);

			var codigosInsumosFiltrados = new List<int>();
			codigosInsumosFiltrados = insumosIncluidosNoProcesso.Count > 0 ?
											insumosIncluidosNoProcesso.ToList().Select(o => ((o.CodigoInsumo == null) ? 0 : (int)o.CodigoInsumo)).ToList() :
												new List<int>();

			var ativo = 1;

			string filtroPosterior = null;
			if (objeto.Sort != null)
			{
				if (objeto.Sort.Equals("DescricaoUnidadeMedida") )
				{
					filtroPosterior = objeto.Sort;
					objeto.Sort = null;
				}
			}

			var result = _uowSciex.QueryStackSciex.LEInsumo.ListarPaginado<LEInsumoVM>(o => o.CodigoProduto == objeto.CodigoProduto
																					    &&  o.SituacaoInsumo == ativo
																						&& (o.TipoInsumo == "P" || o.TipoInsumo == "E") 
																						&& !codigosInsumosFiltrados.Contains(o.CodigoInsumo), objeto);


			foreach (var insumo in result.Items)
			{
				var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == insumo.CodigoUnidadeMedida);
				insumo.DescricaoUnidadeMedida = undMed != null ? undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao : "-";
			}

			if (!string.IsNullOrEmpty(filtroPosterior))
			{
				if (filtroPosterior.Equals("DescricaoUnidadeMedida"))
				{
					if (!objeto.Reverse)
					{
						result.Items = result.Items.OrderBy(q => q.DescricaoUnidadeMedida).ToList();
					}
					else
					{
						result.Items = result.Items.OrderByDescending(q => q.DescricaoUnidadeMedida).ToList();
					}
				}
				
			}


			return result;
		}

		public string Salvar(List<LEInsumoVM> objeto)
		{
			try
			{
				var emElaboracao = 1;
				var idProcesso = objeto.FirstOrDefault().IdProcesso;
				var prcSolicAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.IdProcesso == idProcesso && o.Status == emElaboracao);

				foreach (var item in objeto)
				{
					if(item.Checkbox == true)
					{
						#region RN 18 - ADD ITEM IN SCIEX_PRC_INSUMO
						var prcInsumoEntity = new PRCInsumoEntity()
						{
							IdPrcSolicitacaoAlteracao = prcSolicAlteracaoEntity.Id,
							CodigoInsumo = item.CodigoInsumo,
							CodigoUnidade = item.CodigoUnidadeMedida,
							TipoInsumo = item.TipoInsumo,
							CodigoNCM = item.CodigoNCM,
							CodigoDetalhe = item.CodigoDetalhe,
							DescricaoInsumo = item.DescricaoInsumo,
							DescricaoEspecificacaoTecnica = item.DescricaoEspecTecnica,
							StatusInsumo = 2, //EM ALTERAÇÃO
							IdPrcProduto = item.IdProduto,
							ValorCoeficienteTecnico = item.ValorCoeficienteTecnico,
							StatusInsumoNovo = 1 //NOVO INSUMO
						};
						_uowSciex.CommandStackSciex.PRCInsumo.Salvar(prcInsumoEntity);
						_uowSciex.CommandStackSciex.Save();
						#endregion

						//#region RN 18 - SAVE INFORMATION IN SCIEX_PRC_SOLIC_DETALHE								
						var prcSolicDetalheEntity = new PRCSolicDetalheEntity
						{
							DescricaoPara = item.CodigoInsumo.ToString(),
							IdInsumo = prcInsumoEntity.IdInsumo,
							IdSolicitacaoAlteracao = (prcSolicAlteracaoEntity == null) ? 0 : prcSolicAlteracaoEntity.Id,
							IdTipoSolicitacao = 1 //INCLUSAO DE INSUMO 						
						};
						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(prcSolicDetalheEntity);
						_uowSciex.CommandStackSciex.Save();
						//#endregion
					}

				}

				return "OK";
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}



	}
}
