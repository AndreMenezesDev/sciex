using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.BusinessLogic.Pss;
using System.Text;

namespace Suframa.Sciex.BusinessLogic
{
	public class DesignarPliBll : IDesignarPliBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public DesignarPliBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_usuarioPssBll = usuarioPssBll;
		}

		public IEnumerable<PliVM> Listar(PliVM pliVM)
		{
			var pli = _uowSciex.QueryStackSciex.Pli.Listar<PliVM>();
			return AutoMapper.Mapper.Map<IEnumerable<PliVM>>(pli);
		}

		public IEnumerable<object> Listar()
		{
			return _uowSciex.QueryStackSciex.Pli
				.Listar()
				.OrderBy(o => o.NumeroPli)
				.Select(
					s => new
					{
						id = s.IdPLI,
						text = s.CodigoCNAE + " - " + s.NumeroPli
					});
		}

		public PagedItems<PliVM> ListarPaginado(PliVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

			if (pagedFilter == null) { return new PagedItems<PliVM>(); }

			String cpfAnalista = pagedFilter.IdAnalistaDesignado != null ? GetAnalistaCpf(pagedFilter.IdAnalistaDesignado.Value) : "";
			var pli = new PagedItems<PliVM>();

			pli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
				(
					(
						pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
					)
					&&
					(
						pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
					)
					&&
					(
						String.IsNullOrEmpty(pagedFilter.RazaoSocial) || o.RazaoSocial.ToUpper().Contains(pagedFilter.RazaoSocial.ToUpper())
					)
					&&
					(
						pagedFilter.IdAnalistaDesignado == null || o.PliAnaliseVisual.CpfAnalista.Equals(cpfAnalista)
					)
					&&
					(
						pagedFilter.TipoDocumento == 0 || o.TipoDocumento == pagedFilter.TipoDocumento
					)
					&&
					(
						pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
					)
					&&
					(
						o.StatusPli == (byte)EnumPliStatus.AGUARDANDO_ANÁLISE_VISUAL && o.StatusAnaliseVisual == 1 && (o.PliAnaliseVisual.StatusAnalise == 2 || o.PliAnaliseVisual.StatusAnalise == 9)
					)
					&&
					(
						(pagedFilter.DataInicio == null) || (o.DataEnvioPli >= dataInicio && o.DataEnvioPli <= dataFim)
					)
				),
				pagedFilter);

			return pli;

		}

		//ORDENACAO PADRAO ENTRE TABELAS
		public PagedItems<PRCSolicitacaoAlteracaoVM> ListarPaginadoSolicitacao(DesignarSolicitacaoVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

			int solicitacao = 0;
			int anoSolicitacao = 0;
			int numeroProcesso = 0;
			int anoProcesso = 0;
			if (!string.IsNullOrEmpty(pagedFilter.NumeroAnoSolicitacao))
			{
				solicitacao = Convert.ToInt32(pagedFilter.Solicitacao);
				anoSolicitacao = Convert.ToInt32(pagedFilter.AnoSolicitacao); 
			}

			if (!string.IsNullOrEmpty(pagedFilter.NumeroAnoProcesso))
			{
				numeroProcesso = Convert.ToInt32(pagedFilter.NumeroProcesso);
				anoProcesso = Convert.ToInt32(pagedFilter.AnoProcesso);
			}

			if (pagedFilter == null) { return new PagedItems<PRCSolicitacaoAlteracaoVM>(); }

			String cpfAnalista = pagedFilter.IdAnalistaDesignado != null ? GetAnalistaCpf(pagedFilter.IdAnalistaDesignado.Value) : "";

			Func<PRCSolicitacaoAlteracaoEntity, object> order = null;

			#region Create Flag To Sortable Validations  
			string sort = null;
			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("NumeroProcesso"))
			{
				order = q => q.Processo.NumeroProcesso;
			}
			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("NumeroSolicitacao"))
			{
				order = q => q.NumeroSolicitacao;
			}
			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("QuantidaDeItens"))
			{
				sort = "QuantidaDeItens";
				pagedFilter.Sort = null;
			}
			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("DescricaoStatus"))
			{
				sort = "DescricaoStatus";
				pagedFilter.Sort = null;
			}
			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("InscricaoSuframa"))
			{
				order = q => q.Processo.InscricaoSuframa;
			}
			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("DataAlteracao"))
			{
				order = q => q.DataAlteracao;
			}
			#endregion

			int EmAnalise= 3;

			PagedItems<PRCSolicitacaoAlteracaoVM> listaSolic = new PagedItems<PRCSolicitacaoAlteracaoVM>();

			var skip = (pagedFilter.Page.Value * pagedFilter.Size.Value) - pagedFilter.Size.Value;

			var listaSolic_ = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Listar(
			o =>
				(
					(
						o.Status == EmAnalise
					)
					&&
					(
						pagedFilter.InscricaoCadastral == null || o.Processo.InscricaoSuframa == pagedFilter.InscricaoCadastral
					)
					&&
					(
						String.IsNullOrEmpty(pagedFilter.RazaoSocial) || o.Processo.RazaoSocial.ToUpper().Contains(pagedFilter.RazaoSocial.ToUpper())
					)
					&&
					(
						string.IsNullOrEmpty(pagedFilter.NumeroAnoSolicitacao) || (o.NumeroSolicitacao == solicitacao && o.AnoSolicitacao == anoSolicitacao)
					)
					&&
					(
						(pagedFilter.DataInicio == null) || (dataInicio <= o.DataAlteracao && o.DataAlteracao <= dataFim)
					)
					&&
					(
						pagedFilter.IdAnalistaDesignado == null || o.CpfResponsavel.Equals(cpfAnalista)
					)
					&&
					(
						string.IsNullOrEmpty(pagedFilter.NumeroAnoProcesso) || (o.Processo.NumeroProcesso == numeroProcesso && o.Processo.AnoProcesso == anoProcesso)
					)

				));

			listaSolic.Total = listaSolic_.Count;

			if (order != null)
			{
				if (!pagedFilter.Reverse)
				{
					listaSolic_ = listaSolic_.OrderBy(order).Skip(skip < 0 ? 0 : skip).Take(pagedFilter.Size.Value).ToList();
				}
				else
				{
					listaSolic_ = listaSolic_.OrderBy(order).Reverse().Skip(skip < 0 ? 0 : skip).Take(pagedFilter.Size.Value).ToList();
				}

			}
			else
			{
				listaSolic_ = listaSolic_.Skip(skip).Take(pagedFilter.Size.Value).ToList();
			}

			

			listaSolic.Items = new List<PRCSolicitacaoAlteracaoVM>();

			foreach (var item in listaSolic_)
			{
				var reg = new PRCSolicitacaoAlteracaoVM()
				{
					Id = item.Id,
					RazaoSocial = item.RazaoSocial,
					NumeroSolicitacao = item.NumeroSolicitacao,
					AnoSolicitacao = item.AnoSolicitacao,
					DataAlteracao = item.DataAlteracao,
					CpfResponsavel = item.CpfResponsavel,
					NomeResponsavel = item.NomeResponsavel,
					Status = item.Status,
					ProcessoVM = new ProcessoExportacaoVM()
					{
						IdProcesso = item.Processo.IdProcesso,
						InscricaoSuframa = item.Processo.InscricaoSuframa,
						AnoProcesso = item.Processo.AnoProcesso,
						Cnpj = item.Processo.Cnpj,
						NumeroProcesso = item.Processo.NumeroProcesso,
						RazaoSocial = item.Processo.RazaoSocial
					}

				};

				listaSolic.Items.Add(reg);
			}

			if (listaSolic.Total > 0)
				{
					#region Concat year with number of requisitions.
					foreach (var item in listaSolic.Items)
					{
						var idSolicitacaoAlteracao = item.Id;
						item.DescricaoStatus = (item.Status > 0) ? getDescricaoStatusSolicitacao(item.Status) : "--";
						item.NumeroAnoProcessoFormatado = (item.ProcessoVM.NumeroProcesso > 0 && item.ProcessoVM.AnoProcesso > 0) ?
																getNumeroAnoProcessoFormatadoSolicitacao((int)item.ProcessoVM.NumeroProcesso, (int)item.ProcessoVM.AnoProcesso) :
																	"--";
						item.NumeroAnoSolicitacaoFormatado = (item.NumeroSolicitacao > 0 && item.AnoSolicitacao > 0) ?
																getNumeroAnoSolicitacaoFormatadoSolicitacao((int)item.NumeroSolicitacao, (int)item.AnoSolicitacao) :
																	"--";
						var prcDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Listar(o => o.IdSolicitacaoAlteracao == idSolicitacaoAlteracao);
						item.QuantidaDeItens = (prcDetalhe.Count > 0) ? prcDetalhe.Count : 0;

						item.DataAlteracaoFormatada = Convert.ToDateTime(item.DataAlteracao).ToString("dd/MM/yyyy");
						item.DataInclusaoFormatada = Convert.ToDateTime(item.DataInclusao).ToString("dd/MM/yyyy");
					}
					#endregion

					#region Ordering Pagination List.
					if (!string.IsNullOrWhiteSpace(sort))
					{
						switch (sort)
						{

							case "QuantidaDeItens":
								if (pagedFilter.Reverse)
								{
									listaSolic.Items = listaSolic.Items.OrderBy(q => q.QuantidaDeItens).ThenBy(q => q.QuantidaDeItens).ToList();
								}
								else
								{
									listaSolic.Items = listaSolic.Items.OrderByDescending(q => q.QuantidaDeItens).ThenByDescending(q => q.QuantidaDeItens).ToList();
								}
								break;

							case "DescricaoStatus":
								if (pagedFilter.Reverse)
								{
									listaSolic.Items = listaSolic.Items.OrderBy(q => q.DescricaoStatus).ThenBy(q => q.DescricaoStatus).ToList();
								}
								else
								{
									listaSolic.Items = listaSolic.Items.OrderByDescending(q => q.DescricaoStatus).ThenByDescending(q => q.DescricaoStatus).ToList();
								}
								break;
						}
					}
					#endregion
				}

			return listaSolic;

		}

		public string getDescricaoStatusSolicitacao(int? Status)
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
				return "Deferido";
			}
			else
			if (Status == 5)
			{
				return "Indeferido";
			}
			else
			if (Status == 6)
			{
				return "Em Correção";
			}
			else
			{
				return "--";
			}
		}

		public string getNumeroAnoProcessoFormatadoSolicitacao(int NumeroProcesso, int AnoProcesso) =>
				NumeroProcesso.ToString("D4") + "/" + AnoProcesso.ToString("D4");
		public string getNumeroAnoSolicitacaoFormatadoSolicitacao(int NumeroProcesso, int AnoProcesso) =>
				NumeroProcesso.ToString("D5") + "/" + AnoProcesso.ToString("D4");
	
		public string getDescricaoStatus(int? Status)
		{
			if (Status == 1)
			{
				return " Em Elaboração";
			}
			else
			if (Status == 2)
			{
				return "Aprovado";
			}
			else
			if (Status == 3)
			{
				return "Reprovado";
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

		public PagedItems<LEProdutoVM> ListarPaginadoLes(LEProdutoVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

			if (pagedFilter == null) { return new PagedItems<LEProdutoVM>(); }

			String cpfAnalista = pagedFilter.IdAnalistaDesignado != null ? GetAnalistaCpf(pagedFilter.IdAnalistaDesignado.Value) : "";

			var les = _uowSciex.QueryStackSciex.LEProduto.ListarPaginado<LEProdutoVM>( o =>
				(
					(
						pagedFilter.CodigoProduto == 0 || o.CodigoProduto.Equals(pagedFilter.CodigoProduto)
					)
					&&
					(
						pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
					)
					//&&
					//(
					//	String.IsNullOrEmpty(pagedFilter.RazaoSocial) || o.RazaoSocial.ToUpper().Contains(pagedFilter.RazaoSocial.ToUpper())
					//)
					&&
					(
						pagedFilter.IdAnalistaDesignado == null || o.CpfResponsavel.Equals(cpfAnalista)
					)
					&&
					(
						(pagedFilter.DataInicio == null) || (o.DataEnvio >= dataInicio && o.DataEnvio <= dataFim)
					)
				)
				, pagedFilter);

			return les;
		}

		public PagedItems<LEProdutoVM> ListarPaginadoLesSql(LEProdutoVM pagedFilter)
		{
			PagedItems<LEProdutoVM> ret = new PagedItems<LEProdutoVM>() { Items = new List<LEProdutoVM>(), Total = 0};
			string filtroRazao = null;


			if (pagedFilter.RazaoSocial != null && pagedFilter.RazaoSocial.Length > 0)
			{
				filtroRazao = pagedFilter.RazaoSocial;
				pagedFilter.Sort = null;
				var regProduto = _uowSciex.QueryStackSciex.ListarPaginadoSql<LEProdutoVM>(MontarConsultaLE(pagedFilter), pagedFilter);

				var listaInsCadastral = regProduto.Items.Select(q => q.InscricaoCadastral).ToList();

				var listaDadosView = _uowSciex.QueryStackSciex.ViewImportador.Listar(q => listaInsCadastral.Contains(q.InscricaoCadastral)
																					&&
																					q.RazaoSocial.Contains(filtroRazao));

				foreach (var registroProduto in regProduto.Items)
				{
					var regView = listaDadosView.FirstOrDefault(q => q.InscricaoCadastral == registroProduto.InscricaoCadastral);

					if (regView != null)
					{
						registroProduto.RazaoSocial = regView.RazaoSocial;
						ret.Items.Add(registroProduto);
						ret.Total++;
					} 
				}

				pagedFilter.Sort = filtroRazao;

			}
			else
			{
				ret = _uowSciex.QueryStackSciex.ListarPaginadoSql<LEProdutoVM>(MontarConsultaLE(pagedFilter), pagedFilter);

				var listaInsCadastral = ret.Items.Select(q => q.InscricaoCadastral).ToList();

				var listaDadosView = _uowSciex.QueryStackSciex.ViewImportador.Listar(q => listaInsCadastral.Contains(q.InscricaoCadastral));

				foreach (var registro in ret.Items)
				{
					registro.RazaoSocial = listaDadosView.FirstOrDefault(q => q.InscricaoCadastral == registro.InscricaoCadastral)?.RazaoSocial ?? "-";
				}
			}
			

			foreach (var item in ret.Items)
			{
				if (item.NomeResponsavel == null)
					item.NomeResponsavel = " - ";

				item.DataEnvioFormatada = item.DataEnvio.Value.ToShortDateString();
				item.DataCadastroFormatada = item.DataCadastro.Value.ToShortDateString();
				item.DescStatusLE = item.StatusLE == 1 ? "Em Elaboração" : item.StatusLE == 2 ? "Entregue" : item.StatusLE == 3 ? "Aguardando Aprovação" : "";
			}

			return ret;
		}

		public PagedItems<PlanoExportacaoVM> ListarPaginadoPlanosSql(PlanoExportacaoVM pagedFilter)
		{
			var ret = _uowSciex.QueryStackSciex.ListarPaginadoSql<PlanoExportacaoVM>(MontarConsultaPlano(pagedFilter), pagedFilter);

			foreach (var item in ret.Items)
			{
				if (item.NomeResponsavel == null)
					item.NomeResponsavel = " - ";

				item.DataEnvioFormatada = item.DataEnvio.Value.ToShortDateString();

				item.NumeroAnoPlanoFormatado = item.NumeroPlano.ToString("D5") + "/" + item.AnoPlano;
				item.SituacaoString = item.Situacao == 1 ? "EM ELABORAÇÃO"
												: item.Situacao == 2 ? "ENTREGUE"
												: item.Situacao == 3 ? "EM ANÁLISE"
												: "-";

				item.TipoExportacaoString = item.TipoExportacao == "AP" ? "APROVAÇÃO"
													: item.TipoExportacao == "CO" ? "COMPROVAÇÃO"
													: "-";

				item.TipoModalidadeString = item.TipoModalidade == "S" ? "SUSPENSÃO" : "-";
			}

			return ret;
		}

		private string MontarConsultaLE(LEProdutoVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

			String cpfAnalista = pagedFilter.IdAnalistaDesignado != null ? GetAnalistaCpf(pagedFilter.IdAnalistaDesignado.Value) : "";

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("select");
			sb.AppendLine("lep_id as IdLe");
			sb.AppendLine(",lep_nu_inscricao_cadastral as InscricaoCadastral");
			sb.AppendLine(",lep_nu_cnpj as Cnpj");
			sb.AppendLine(",lep_co_produto as CodigoProduto");
			sb.AppendLine(",lep_co_produto_suframa as CodigoProdutoSuframa");
			sb.AppendLine(",lep_co_tipo_produto_suframa as CodigoTipoProduto");
			sb.AppendLine(",lep_co_unidade as CodigoUnidadeMedida");
			sb.AppendLine(",lep_ds_modelo as DescricaoModelo");
			sb.AppendLine(",lep_st_le as StatusLE");
			sb.AppendLine(",lep_dt_envio as DataEnvio");
			sb.AppendLine(",lep_dt_aprovacao as DataAprovacao");
			sb.AppendLine(",lep_co_modelo_empresa CodigoModeloEmpresa");
			sb.AppendLine(",lep_ds_centro_custo as DescricaoCentroCusto");
			sb.AppendLine(",lep_nu_cpf_responsavel as CpfResponsavel");
			sb.AppendLine(",lep_no_responsavel as NomeResponsavel");
			sb.AppendLine(",lep_co_ncm as CodigoNCM");
			sb.AppendLine(",lep_dt_cadastro as DataCadastro");
			//sb.AppendLine(",imp_ds_razao_social as RazaoSocial");
			sb.AppendLine("from sciex_le_produto a");
			//sb.AppendLine("join VW_SCIEX_IMPORTADOR b");
			//sb.AppendLine("  on a.lep_nu_inscricao_cadastral = b.ins_co");
			sb.AppendLine("where 1 = 1 ");
			sb.AppendLine("AND (a.lep_st_le = 3 OR a.lep_st_le_alteracao = 3) ");


			if (pagedFilter.InscricaoCadastral > 0)
				sb.AppendLine("AND a.lep_nu_inscricao_cadastral = " + pagedFilter.InscricaoCadastral.ToString());

			//if (!String.IsNullOrEmpty(pagedFilter.RazaoSocial))
			//	sb.AppendLine("AND b.imp_ds_razao_social like '%" + pagedFilter.RazaoSocial + "%'");

			if (pagedFilter.CodigoProduto > 0)
				sb.AppendLine("AND a.lep_co_produto = " + pagedFilter.CodigoProduto.ToString());

			if (pagedFilter.DataInicio != null)
				sb.AppendLine("AND a.lep_dt_envio >= convert(datetime, '" + dataInicio.ToString() + "' , 103) AND a.lep_dt_envio <= convert(datetime, '" + dataFim.ToString() + "' , 103)");

			if(cpfAnalista != "")
				sb.AppendLine($@"AND a.lep_nu_cpf_responsavel = '{cpfAnalista}'");

			return sb.ToString();
		}

		private string MontarConsultaPlano(PlanoExportacaoVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

			String cpfAnalista = pagedFilter.IdAnalistaDesignado != null ? GetAnalistaCpf(pagedFilter.IdAnalistaDesignado.Value) : "";

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("SELECT [pex_id] as IdPlanoExportacao						   ");
			sb.AppendLine("	  ,[pex_nu_plano] as NumeroPlano						   ");
			sb.AppendLine("	  ,[pex_nu_ano_plano] as AnoPlano						   ");
			sb.AppendLine("	  ,[pex_nu_inscricao_cadastral] as NumeroInscricaoCadastral");
			sb.AppendLine("	  ,[pex_nu_cnpj] as Cnpj								   ");
			sb.AppendLine("	  ,[pex_ds_razao_social] as RazaoSocial					   ");
			sb.AppendLine("	  ,[pex_tp_modalidade] as TipoModalidade				   ");
			sb.AppendLine("	  ,[pex_tp_exportacao] as TipoExportacao				   ");
			sb.AppendLine("	  ,[pex_st] as Situacao									   ");
			sb.AppendLine("	  ,[pex_dt_envio] as DataEnvio							   ");
			sb.AppendLine("	  ,[pex_dt_cadastro] as DataCadastro					   ");
			sb.AppendLine("	  ,[pex_dt_status] as DataStatus						   ");
			sb.AppendLine("	  ,[pex_nu_cpf_responsavel] as CpfResponsavel			   ");
			sb.AppendLine("	  ,[pex_no_responsavel] as NomeResponsavel				   ");
			sb.AppendLine("FROM [dbo].[SCIEX_PLANO_EXPORTACAO]   ");
			sb.AppendLine("where 1 = 1 ");
			sb.AppendLine("AND [pex_st] = 3");


			if (pagedFilter.NumeroInscricaoCadastral > 0)
				sb.AppendLine("AND [pex_nu_inscricao_cadastral] = " + pagedFilter.NumeroInscricaoCadastral.ToString());

			if (!String.IsNullOrEmpty(pagedFilter.RazaoSocial))
				sb.AppendLine("AND [pex_ds_razao_social] like '%" + pagedFilter.RazaoSocial + "%'");

			if (!string.IsNullOrEmpty(pagedFilter.NumeroAnoPlanoFormatado))
			{
				sb.AppendLine("AND [pex_nu_plano] = " + pagedFilter.NumeroAnoPlanoFormatado.Substring(0, 5));
				sb.AppendLine("AND [pex_nu_ano_plano] = " + pagedFilter.NumeroAnoPlanoFormatado.Substring(6, 4));
			}

			if (pagedFilter.DataInicio != null)
				sb.AppendLine("AND [pex_dt_envio] >= convert(datetime, '" + dataInicio.ToString() + "' , 103) AND [pex_dt_envio] <= convert(datetime, '" + dataFim.ToString() + "' , 103)");

			if (cpfAnalista != "")
				sb.AppendLine("AND [pex_nu_cpf_responsavel] = " + "'" + cpfAnalista + "'");

			return sb.ToString();
		}

		private string GetAnalistaCpf(Int32 idAnalista)
		{
			return _uowSciex.QueryStackSciex.Analista.Selecionar(o => o.IdAnalista == idAnalista).CPF;
		}

		public PliVM Salvar(ListaPliVM pliVM)
		{
			if(pliVM == null || pliVM.Lista.Count() == 0)
				return null;

			var usuarioLogado = _usuarioPssBll.ObterUsuarioLogado();


			foreach (var item in pliVM.Lista)
			{
				var analista = _uowSciex.QueryStackSciex.Analista.Selecionar(o => o.IdAnalista == item.IdAnalistaDesignado);

				var plianalise = _uowSciex.QueryStackSciex.PliAnaliseVisual.Selecionar(o => o.IdPLI == item.IdPLI);

				var cpfOld = plianalise.CpfAnalista;

				plianalise.CpfAnalista = analista.CPF;
				plianalise.NomeAnalista = analista.Nome;

				_uowSciex.CommandStackSciex.PliAnaliseVisual.Salvar(plianalise);
				_uowSciex.CommandStackSciex.Save();

				AuditoriaEntity auditoria = new AuditoriaEntity();
				auditoria.IdAuditoriaAplicacao = 5;
				auditoria.CpfCnpjResponsavel = usuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();
				auditoria.NomeResponsavel = usuarioLogado.usuarioLogadoNome;
				auditoria.TipoAcao = 2;
				auditoria.DataHoraAcao = DateTime.Now;
				auditoria.IdReferencia = item.IdPLI.Value;
				auditoria.DescricaoAcao = "ALTERAÇÃO: Alterando registro: " + item.IdPLI.Value.ToString() + " – Campos afetados: CPF DE: " + cpfOld + " PARA: " + analista.CPF;

				_uowSciex.CommandStackSciex.DetachEntries();
				_uowSciex.CommandStackSciex.Auditoria.Salvar(auditoria);
				_uowSciex.CommandStackSciex.Save();
			}

			

			return new PliVM() { Mensagem = "Salvo com sucesso!"};
		}

		public LEProdutoVM DesignarAnalistaLe(ListaLeVM listaVM)
		{
			if (listaVM == null || listaVM.Lista.Count() == 0)
				return null;

			var usuarioLogado = _usuarioPssBll.ObterUsuarioLogado();


			foreach (var item in listaVM.Lista)
			{
				var analista = _uowSciex.QueryStackSciex.Analista.Selecionar(o => o.IdAnalista == item.IdAnalistaDesignado);

				var leanalise = _uowSciex.QueryStackSciex.LEProduto.Selecionar(o => o.IdLe == item.IdLe);

				var cpfOld = leanalise.CpfResponsavel;

				leanalise.CpfResponsavel = analista.CPF;
				leanalise.NomeResponsavel = analista.Nome;

				_uowSciex.CommandStackSciex.LEProduto.Salvar(leanalise);
				_uowSciex.CommandStackSciex.Save();

				AuditoriaEntity auditoria = new AuditoriaEntity();
				auditoria.IdAuditoriaAplicacao = 5;
				auditoria.CpfCnpjResponsavel = usuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();
				auditoria.NomeResponsavel = usuarioLogado.usuarioLogadoNome;
				auditoria.TipoAcao = 2;
				auditoria.DataHoraAcao = DateTime.Now;
				auditoria.IdReferencia = item.IdLe;
				auditoria.DescricaoAcao = "ALTERAÇÃO: Alterando registro: " + item.IdLe.ToString() + " – Campos afetados: CPF DE: " + cpfOld + " PARA: " + analista.CPF;

				_uowSciex.CommandStackSciex.DetachEntries();
				_uowSciex.CommandStackSciex.Auditoria.Salvar(auditoria);
				_uowSciex.CommandStackSciex.Save();
			}



			return new LEProdutoVM() { Mensagem = "Salvo com sucesso!" };
		}

		public PlanoExportacaoVM DesignarAnalistaPlano(ListaPlanoVM listaVM)
		{
			if (listaVM == null || listaVM.Lista.Count() == 0)
				return null;

			var usuarioLogado = _usuarioPssBll.ObterUsuarioLogado();


			foreach (var item in listaVM.Lista)
			{
				var analista = _uowSciex.QueryStackSciex.Analista.Selecionar(o => o.IdAnalista == item.IdAnalistaDesignado);

				var peAnalise = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(o => o.IdPlanoExportacao == item.IdPlanoExportacao);

				var cpfOld = peAnalise.CpfResponsavel;

				peAnalise.CpfResponsavel = analista.CPF;
				peAnalise.NomeResponsavel = analista.Nome;

				_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(peAnalise);
				_uowSciex.CommandStackSciex.Save();

				AuditoriaEntity auditoria = new AuditoriaEntity();
				auditoria.IdAuditoriaAplicacao = 5;
				auditoria.CpfCnpjResponsavel = usuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();
				auditoria.NomeResponsavel = usuarioLogado.usuarioLogadoNome;
				auditoria.TipoAcao = 2;
				auditoria.DataHoraAcao = DateTime.Now;
				auditoria.IdReferencia = item.IdPlanoExportacao;
				auditoria.DescricaoAcao = "ALTERAÇÃO: Alterando registro: " + item.IdPlanoExportacao.ToString() + " – Campos afetados: CPF DE: " + cpfOld + " PARA: " + analista.CPF;

				_uowSciex.CommandStackSciex.DetachEntries();
				_uowSciex.CommandStackSciex.Auditoria.Salvar(auditoria);
				_uowSciex.CommandStackSciex.Save();
			}

			return new PlanoExportacaoVM() { Mensagem = "Salvo com sucesso!" };
		}

		public PlanoExportacaoVM DesignarAnalistaSolicitacao(ListaSolicitacaoVM listaVM)
		{
			if (listaVM == null || listaVM.Lista.Count() == 0)
				return null;

			var usuarioLogado = _usuarioPssBll.ObterUsuarioLogado();


			foreach (var item in listaVM.Lista)
			{
				var analista = _uowSciex.QueryStackSciex.Analista.Selecionar(o => o.IdAnalista == item.IdAnalistaDesignado);

				var solicitacao = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.Id == item.Id);

				var cpfOld = solicitacao.CpfResponsavel;

				solicitacao.CpfResponsavel = analista.CPF;
				solicitacao.NomeResponsavel = analista.Nome;

				_uowSciex.CommandStackSciex.PRCSolicitacaoAlteracao.Salvar(solicitacao);
				_uowSciex.CommandStackSciex.Save();

				AuditoriaEntity auditoria = new AuditoriaEntity();
				auditoria.IdAuditoriaAplicacao = 5;
				auditoria.CpfCnpjResponsavel = usuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();
				auditoria.NomeResponsavel = usuarioLogado.usuarioLogadoNome;
				auditoria.TipoAcao = 2;
				auditoria.DataHoraAcao = DateTime.Now;
				auditoria.IdReferencia = item.Id;
				auditoria.DescricaoAcao = "ALTERAÇÃO: Alterando registro: " + item.Id.ToString() + " – Campos afetados: CPF DE: " + cpfOld + " PARA: " + analista.CPF;

				_uowSciex.CommandStackSciex.DetachEntries();
				_uowSciex.CommandStackSciex.Auditoria.Salvar(auditoria);
				_uowSciex.CommandStackSciex.Save();
			}

			return new PlanoExportacaoVM() { Mensagem = "Salvo com sucesso!" };
		}
		public PliVM Selecionar(long? idPli)
		{
			var pliVM = new PliVM();
			if (!idPli.HasValue)
			{
				return pliVM;
			}

			var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(x => x.IdPLI == idPli);
			var pliTaxaDebito= _uowSciex.QueryStackSciex.TaxaPliDebito.Listar(p => p.IdPli == pli.IdPLI).OrderBy(k => k.IdDebito).FirstOrDefault();
			var pliTaxa = _uowSciex.QueryStackSciex.TaxaPli.Selecionar(p => p.IdPli == pli.IdPLI);

			if (pli == null)
			{
				return null;
			}
			
			var importador = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(o => o.Cnpj == pli.Cnpj);
			var cnae = _uowCadsuf.QueryStack.ViewAtividadeEconomicaPrincipal.Selecionar(o => o.CNPJ == pli.Cnpj);

			var uticon = _uowSciex.QueryStackSciex.ControleImportacao.Listar(o => o.IdPliAplicacao == pli.IdPLIAplicacao).FirstOrDefault();

			pliVM = AutoMapper.Mapper.Map<PliVM>(pli);

			if(pli.PLIMercadoria != null && pli.PLIMercadoria.FirstOrDefault() != null && pli.PLIMercadoria.FirstOrDefault().Ali == null)
			{
				var erroProc = _uowSciex.QueryStackSciex.ErroProcessamento.Selecionar(o => o.IdPli == pli.IdPLI);
				if(erroProc != null)
					pliVM.DataProcessamento = erroProc.DataProcessamento.Value.ToShortDateString();
			}

			pliVM.Endereco = importador.Endereco;
			pliVM.Numero = importador.Numero;
			pliVM.Complemento = importador.Complemento;
			pliVM.Bairro = importador.Bairro;
			pliVM.CodigoMunicipio = importador.CodigoMunicipio.ToString();

			if (pliVM.DataDebitoGeracao == null)
			{
				pliVM.DescricaoDebito = " - ";
				pliVM.Situacao = " - ";
				pliVM.DescricaoValorGeralTcif = " - ";
			}
			else
			{
				pliVM.DescricaoDebito = pliTaxaDebito == null ? " - " : pliTaxaDebito.NumeroDebito.ToString() + "/" + pliTaxaDebito.AnoDebito.ToString();
				if (pliTaxaDebito == null)
				{
					pliVM.Situacao = " - ";
				}
				else
				{
					if (pliTaxaDebito.NumeroControleCobrancaTCIF == 0)
						pliVM.Situacao = "Cobrar Débito";
					if (pliTaxaDebito.NumeroControleCobrancaTCIF == 1)
						pliVM.Situacao = "Não Cobrar Débito";
					if (pliTaxaDebito.NumeroControleCobrancaTCIF == 2)
						pliVM.Situacao = "Suspender Débito";
				}
				pliVM.DescricaoValorGeralTcif = pliTaxa == null ? " - " : pliTaxa.ValorGeralTCIF.ToString();
			}


			pliVM.Municipio = importador.Municipio;
			pliVM.UF = importador.UF;
			pliVM.CEP = string.Format("{0:00000-000}", importador.CEP);
			pliVM.DescricaoCNAE = cnae.Descricao;
			pliVM.PaisCodigo = importador.CodigoPais;
			pliVM.PaisDescricao = importador.DescricaoPais;
			pliVM.Telefone = (importador.Telefone.Length == 10 ? string.Format("{0:(00) 0000-0000}", Convert.ToDecimal(importador.Telefone)) : string.Format("{0:(##) #####-####}", Convert.ToDecimal(importador.Telefone)));
			if(uticon != null)
			{
				pliVM.CodigoUtilizacao = uticon.CodigoUtilizacao.Codigo.ToString();
				pliVM.DescricaoUtilizacao = uticon.CodigoUtilizacao.Descricao.ToString();
				pliVM.CodigoConta = uticon.CodigoConta.Codigo.ToString();
				pliVM.DescricaoConta = uticon.CodigoConta.Descricao.ToString();
			}
			if (pli.NumeroLIReferencia != null)
			{
				if (!String.IsNullOrEmpty(pli.NumeroLIReferencia.Trim()))
				{
					var numLi = Int64.Parse(pli.NumeroLIReferencia);

					var idLi = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numLi).IdPliMercadoria;
					pliVM.NumeroALISubstitutiva = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == idLi).NumeroAli;

					pliVM.IdLiReferencia = idLi;

					var idPLi = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == idLi).IdPLI;
					pliVM.NumeroPLISubstitutivo = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == idPLi).NumeroPli;

					pliVM.IdPLISubstitutivo = idPLi;
					pliVM.IdPliMercadoriaSubstitutivo = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == idLi).IdPliMercadoria;

					var NumeroPLISubstitutivoInt = Convert.ToInt32(pliVM.NumeroPLISubstitutivo);

					pliVM.AnoPliSubstitutivo = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == idPLi).Ano;
					pliVM.NumeroPliSubstitutivoConcatenado = (pliVM.AnoPliSubstitutivo + "/" + NumeroPLISubstitutivoInt.ToString("d6"));
				} 
			}

			if(pliVM.StatusAnaliseVisual == 1)
			{
				var plianalise = _uowSciex.QueryStackSciex.PliAnaliseVisual.Selecionar(o => o.IdPLI == pliVM.IdPLI);

				if (plianalise == null || plianalise.IdPLI == null || plianalise.StatusAnalise == 02)
				{
					pliVM.StatusPliAnalise = 2;
					pliVM.StatusPliAnaliseFormatado = "EM ANÁLISE VISUAL";
				}
				else if (plianalise.StatusAnalise == 07)
				{
					pliVM.StatusPliAnalise = plianalise.StatusAnalise;
					pliVM.StatusPliAnaliseFormatado = "ANÁLISE VISUAL OK";
				}
				else if (plianalise.StatusAnalise == 08)
				{
					pliVM.StatusPliAnalise = plianalise.StatusAnalise;
					pliVM.StatusPliAnaliseFormatado = "ANÁLISE VISUAL NÃO OK";
				}
				else if (plianalise.StatusAnalise == 09)
				{
					pliVM.StatusPliAnalise = plianalise.StatusAnalise;
					pliVM.StatusPliAnaliseFormatado = "ANÁLISE VISUAL PENDENTE";
				}
			}

			return pliVM;
		}

		public void Deletar(long id)
		{
			var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(s => s.IdPLI == id);


			if (pli != null)
			{
				_uowSciex.CommandStackSciex.Pli.Apagar(pli.IdPLI);
			}
			_uowSciex.CommandStackSciex.Save();
		}
	}
}