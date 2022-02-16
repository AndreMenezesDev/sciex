using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.BusinessLogic.Pss;

namespace Suframa.Sciex.BusinessLogic
{
	public class HistoricoProcessoSuframaBll : IHistoricoProcessoSuframaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public HistoricoProcessoSuframaBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioPssBll = usuarioPssBll;
		}

		
		public PagedItems<PRCSolicHistoricoVM> ListarPaginado(PRCStatusVM objeto)
		{

			#region Create Flag To Sortable Validations  
			string sort = null;
			if (!string.IsNullOrEmpty(objeto.Sort) && objeto.Sort.Equals(" "))
			{
				sort = "DescricaoPlanoExportacao";
				objeto.Sort = null;
			}
			if (!string.IsNullOrEmpty(objeto.Sort) && objeto.Sort.Equals("DescricaoStatus"))
			{
				sort = "DescricaoStatus";
				objeto.Sort = null;
			}
			#endregion

			
			var pagedItems = _uowSciex.QueryStackSciex.PRCStatus.ListarPaginadoGrafo(o => new PRCSolicHistoricoVM() {
				AnoPlano = o.AnoPlano,
				Data = o.Data,
				DataValidade = o.DataValidade,
				Tipo = o.Tipo,
				IdProcesso = o.IdProcesso,
				IdStatus = o.IdStatus,
				IdSolicitacaoAlteracao= o.IdSolicitacaoAlteracao,
				NomeResponsavel = o.NomeResponsavel,
				NumeroPlano = o.NumeroPlano,
				DescricaoObservacao = o.DescricaoObservacao,
				SolicitacaoAlteracao = new PRCSolicitacaoAlteracaoVM()
				{
					AnoSolicitacao = o.SolicitacaoAlteracao.AnoSolicitacao,
					NumeroSolicitacao = o.SolicitacaoAlteracao.NumeroSolicitacao
				}

			}, o => o.IdProcesso == objeto.IdProcesso, objeto) ;
			if(pagedItems.Total > 0)
			{
				foreach(var item in pagedItems.Items)
				{
					item.DescricaoPlanoExportacao = item.AnoPlano != null && item.NumeroPlano!=null ? item.AnoPlano  + "/" + item.NumeroPlano?.ToString("D5") : " ";
					item.DescricaoStatus = getDescriptionStatus(item.Tipo);
					item.AnoNumeroSolicitacaoFormatado = item.SolicitacaoAlteracao.AnoSolicitacao != null&& item.SolicitacaoAlteracao.NumeroSolicitacao!=null ? Convert.ToInt32(item.SolicitacaoAlteracao.AnoSolicitacao).ToString() + "/" + Convert.ToInt32(item.SolicitacaoAlteracao.NumeroSolicitacao).ToString("D5") : " ";
				}
				
				#region Ordering Pagination List.
				if (!string.IsNullOrWhiteSpace(sort))
				{
					switch (sort)
					{
						case "DescricaoPlanoExportacao":
							if (objeto.Reverse)
							{
								pagedItems.Items = pagedItems.Items.OrderBy(q => q.DescricaoPlanoExportacao).ThenBy(q => q.DescricaoPlanoExportacao).ToList();
							}
							else
							{
								pagedItems.Items = pagedItems.Items.OrderByDescending(q => q.DescricaoPlanoExportacao).ThenByDescending(q => q.DescricaoPlanoExportacao).ToList();
							}
							break;

						case "DescricaoStatus":
							if (objeto.Reverse)
							{
								pagedItems.Items = pagedItems.Items.OrderBy(q => q.DescricaoStatus).ThenBy(q => q.DescricaoStatus).ToList();
							}
							else
							{
								pagedItems.Items = pagedItems.Items.OrderByDescending(q => q.DescricaoStatus).ThenByDescending(q => q.DescricaoStatus).ToList();
							}
							break;
					}
				}
				#endregion
			}

			return pagedItems;
		}



		private string getDescriptionStatus(string status)
		{
			return status == "AP" ? "Aprovado" :
				   status == "AL" ? "Alterado" :
				   status == "PR" ? "Prorrogado" :
				   status == "PE" ? "Prorrogado em caráter especial" :
				   status == "CO" ? "Comprovado" :
				   status == "AU" ? "Autorizado" :
				   status == "CA" ? "Cancelado" :

				   "";
		
		}

	}

}
