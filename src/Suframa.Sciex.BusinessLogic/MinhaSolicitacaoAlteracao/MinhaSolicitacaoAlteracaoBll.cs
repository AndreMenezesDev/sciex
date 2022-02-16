using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.BusinessLogic.Pss;
using System.Globalization;

namespace Suframa.Sciex.BusinessLogic
{
	public class MinhaSolicitacaoAlteracaoBll : IMinhaSolicitacaoAlteracaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public MinhaSolicitacaoAlteracaoBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioPssBll = usuarioPssBll;
		}

		public PagedItems<PRCSolicitacaoAlteracaoVM> ListarPaginado(PRCSolicitacaoAlteracaoVM objeto)
		{
			var dataInicio = objeto.DataInicio == null ? new DateTime(1, 1, 1) : 
								new DateTime(objeto.DataInicio.Value.Year, objeto.DataInicio.Value.Month, objeto.DataInicio.Value.Day);

			var dataFim = objeto.DataFim == null ? new DateTime(1, 1, 1) : 
								new DateTime(objeto.DataFim.Value.Year, objeto.DataFim.Value.Month, objeto.DataFim.Value.Day, 23, 59, 59);

			objeto.Cnpj = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();

			#region Get IdProcesso
			if (objeto.AnoProcesso > 0 && objeto.NumeroProcesso > 0)
			{
				var processoEntity = _uowSciex.QueryStackSciex.Processo.Selecionar(o => o.NumeroProcesso == objeto.NumeroProcesso &&
																						o.AnoProcesso == objeto.AnoProcesso);
				objeto.IdProcesso = processoEntity.IdProcesso; 									
			}
			#endregion

			#region Create Flag To Sortable Validations  
			string sort = null;
			if (!string.IsNullOrEmpty(objeto.Sort) && objeto.Sort.Equals("NumeroProcesso"))
			{
				sort = "NumeroProcesso";
				objeto.Sort = null;
			}
			if (!string.IsNullOrEmpty(objeto.Sort) && objeto.Sort.Equals("NumeroSolicitacao"))
			{
				sort = "NumeroSolicitacao";
				objeto.Sort = null;
			}
			if (!string.IsNullOrEmpty(objeto.Sort) && objeto.Sort.Equals("QuantidaDeItens"))
			{
				sort = "QuantidaDeItens";
				objeto.Sort = null;
			}
			if (!string.IsNullOrEmpty(objeto.Sort) && objeto.Sort.Equals("DescricaoStatus"))
			{
				sort = "DescricaoStatus";
				objeto.Sort = null;
			}
			#endregion

			#region STATUS
			var arrayStatus = new List<int?>();
			var TODOS = 99;
			if (objeto.Status == TODOS)
			{
				arrayStatus.Add((int)EnumStatusSolicitacaoAlteracao.EmElaboracao);
				arrayStatus.Add((int)EnumStatusSolicitacaoAlteracao.Entregue);
				arrayStatus.Add((int)EnumStatusSolicitacaoAlteracao.EmAnalise);
				arrayStatus.Add((int)EnumStatusSolicitacaoAlteracao.Finalizado);
			}
			else
			{
				arrayStatus.Add(objeto.Status);
			}
			#endregion


			var dadosUsuarioLogado = _usuarioPssBll.ObterUsuarioLogado();
			var isUsuarioInterno = false;
			if (dadosUsuarioLogado.Perfis.Contains(EnumPerfil.Analista) || dadosUsuarioLogado.Perfis.Contains(EnumPerfil.Coordenador)) 
			{
				isUsuarioInterno = true;
			}


			var PagedItems = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.ListarPaginadoGrafo(o => new PRCSolicitacaoAlteracaoVM()
			{
				Id = o.Id,
				AnoSolicitacao = o.AnoSolicitacao,
				NumeroSolicitacao = o.NumeroSolicitacao,
				Cnpj = o.Cnpj,
				CpfResponsavel = o.CpfResponsavel,
				DataAlteracao = o.DataAlteracao,
				DataInclusao = o.DataInclusao,				
				IdProcesso = o.IdProcesso,
				NomeResponsavel = o.NomeResponsavel,
				RazaoSocial = o.RazaoSocial,
				ProcessoVM = new ProcessoExportacaoVM()
				{
					NumeroProcesso = o.Processo.NumeroProcesso,
					AnoProcesso = o.Processo.AnoProcesso
				},
				Status = o.Status
			},
			o =>
			(
			    (objeto.NumeroProcesso == 0 || (o.ProcessoVM.NumeroProcesso == objeto.NumeroProcesso && o.ProcessoVM.AnoProcesso == objeto.AnoProcesso))
				&&
				(objeto.NumeroSolicitacao == -1 || (o.NumeroSolicitacao == objeto.NumeroSolicitacao && o.AnoSolicitacao == objeto.AnoSolicitacao)) 
				&&
				(dataInicio == DateTime.MinValue || (dataInicio <= o.DataInclusao && o.DataInclusao <= dataFim)) 
				&&
				(arrayStatus.Contains(o.Status) )
				&&
				(isUsuarioInterno || o.Cnpj.Equals(objeto.Cnpj))
				&&
				((objeto.IdProcesso == 0 || objeto.IdProcesso == null) || o.IdProcesso == objeto.IdProcesso)
			),			
			objeto);

			
			if (PagedItems.Total > 0)
			{
				#region Concat year with number of requisitions.
				foreach (var item in PagedItems.Items)
				{
					var idSolicitacaoAlteracao = item.Id;
					item.DescricaoStatus = (item.Status > 0) ? getDescricaoStatusGrid(item.Status) : "--";
					item.NumeroAnoProcessoFormatado = (item.ProcessoVM.NumeroProcesso > 0 && item.ProcessoVM.AnoProcesso > 0) ?
															getNumeroAnoFormatado((int)item.ProcessoVM.NumeroProcesso, (int)item.ProcessoVM.AnoProcesso) :
																"--";
					item.NumeroAnoSolicitacaoFormatado = (item.NumeroSolicitacao > 0 && item.AnoSolicitacao > 0) ?
															getNumeroAnoFormatado((int)item.NumeroSolicitacao, (int)item.AnoSolicitacao) :
																"--";
					var prcDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Listar(o => o.IdSolicitacaoAlteracao == idSolicitacaoAlteracao);					
					item.QuantidaDeItens = (prcDetalhe.Count > 0) ? prcDetalhe.Count : 0;

					item.DataAlteracaoFormatada = Convert.ToDateTime(item.DataAlteracao).ToString("dd/MM/yyyy HH:mm:ss");
					item.DataInclusaoFormatada = Convert.ToDateTime(item.DataInclusao).ToString("dd/MM/yyyy HH:mm:ss");
				}
				#endregion

				#region Ordering Pagination List.
				if (!string.IsNullOrWhiteSpace(sort))
				{
					switch (sort)
					{
						case "NumeroProcesso":
							if (objeto.Reverse)
							{
								PagedItems.Items = PagedItems.Items.OrderBy(q => q.NumeroAnoProcessoFormatado).ThenBy(q => q.NumeroAnoProcessoFormatado).ToList();
							}
							else
							{
								PagedItems.Items = PagedItems.Items.OrderByDescending(q => q.NumeroAnoProcessoFormatado).ThenByDescending(q => q.NumeroAnoProcessoFormatado).ToList();
							}
							break;

						case "NumeroSolicitacao":
							if (objeto.Reverse)
							{
								PagedItems.Items = PagedItems.Items.OrderBy(q => q.NumeroAnoSolicitacaoFormatado).ThenBy(q => q.NumeroAnoSolicitacaoFormatado).ToList();
							}
							else
							{
								PagedItems.Items = PagedItems.Items.OrderByDescending(q => q.NumeroAnoSolicitacaoFormatado).ThenByDescending(q => q.NumeroAnoSolicitacaoFormatado).ToList();
							}
							break;

						case "QuantidaDeItens":
							if (objeto.Reverse)
							{
								PagedItems.Items = PagedItems.Items.OrderBy(q => q.QuantidaDeItens).ThenBy(q => q.QuantidaDeItens).ToList();
							}
							else
							{
								PagedItems.Items = PagedItems.Items.OrderByDescending(q => q.QuantidaDeItens).ThenByDescending(q => q.QuantidaDeItens).ToList();
							}
							break;

						case "DescricaoStatus":
							if (objeto.Reverse)
							{
								PagedItems.Items = PagedItems.Items.OrderBy(q => q.DescricaoStatus).ThenBy(q => q.DescricaoStatus).ToList();
							}
							else
							{
								PagedItems.Items = PagedItems.Items.OrderByDescending(q => q.DescricaoStatus).ThenByDescending(q => q.DescricaoStatus).ToList();
							}
							break;
					}
				}
				#endregion
			}
			
			return PagedItems;
		}

		public string BuscarNumeroAnoProcessoPorIdProcesso (int idProcesso)
		{
			var processo = _uowSciex.QueryStackSciex.Processo.Selecionar(o => o.IdProcesso == idProcesso);
			return getNumeroAnoFormatado((int) processo.NumeroProcesso, (int)processo.AnoProcesso);
		}

		public string getNumeroAnoFormatado(int NumeroProcesso, int AnoProcesso) =>
				NumeroProcesso.ToString("D4") + "/" + AnoProcesso.ToString("D4");

		public string getDescricaoStatusGrid(int? Status)
		{
			if (Status == 1)
			{
				return " Em Elaboração";
			}
			else
			if (Status == 2)
			{
				return "Entregue";
			}
			else
			if (Status == 3)
			{
				return "Em Análise";
			}
			else
			if (Status == 4)
			{
				return "Finalizado";
			}
			else
			{
				return "--";
			}
		}

		public string getDescricaoStatus(int? Status)
		{
			if(Status == 1)
			{
				return " Em Elaboração";
			} else
			if (Status == 2)
			{
				return "Aprovado";
			} else
			if (Status == 3)
			{
				return "Reprovado";
			} else
			if (Status == 4)
			{
				return "Finalizado";
			}
			else
			{
				return "--";
			}			
		}



	}
}
