using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Ftp;
using Suframa.Sciex.CrossCutting.Texto;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class AliHistoricoBll : IAliHistoricoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;
		private readonly IComplementarPLIBll _complementarPLIBll;
		private readonly IControleExecucaoServicoBll _controleExecucaoServicoBll;
		private string CNPJ { get; set; }

		public AliHistoricoBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf,
			IUsuarioLogado usuarioLogado, IViewImportadorBll viewImportadorBll, IComplementarPLIBll complementarPLIBll,
			IUsuarioInformacoesBll usuarioInformacoesBll, IControleExecucaoServicoBll controleExecucaoServicoBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_IUsuarioLogado = usuarioLogado;
			_IViewImportadorBll = viewImportadorBll;
			_complementarPLIBll = complementarPLIBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
			_controleExecucaoServicoBll = controleExecucaoServicoBll;
			this.CNPJ = _usuarioInformacoesBll.ObterCNPJ();
		}

		public IEnumerable<AliHistoricoVM> Listar(AliHistoricoVM AliHistoricoVM)
		{
			var AliHistorico = _uowSciex.QueryStackSciex.AliHistorico.Listar<AliHistoricoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<AliHistoricoVM>>(AliHistorico);
		}

		public IEnumerable<object> ListarChave(AliHistoricoVM AliHistoricoVM)
		{
			var lista = _uowSciex.QueryStackSciex.AliHistorico
				.Listar().Where(o =>
						(AliHistoricoVM.IdPliMercadoria == -1 || o.IdPliMercadoria == AliHistoricoVM.IdPliMercadoria)

					)
				.OrderBy(o => o.DataOperacao)
				.Select(
					s => new
					{
						id = s.IdPliMercadoria,
						text = s.Observacao
					});
			return lista;
		}

		public PagedItems<AliHistoricoVM> ListarPaginado(AliHistoricoVM pagedFilter)
		{
			string sorted = string.Empty;
			if (pagedFilter == null) { return new PagedItems<AliHistoricoVM>(); }

			if (
				pagedFilter.Sort == "acao" || 
				pagedFilter.Sort == "dataformatada" || 
				pagedFilter.Sort == "cpfcnpjResponsavel" ||
				pagedFilter.Sort == "nomeResponsavel" ||
				pagedFilter.Sort == "observacao"
				)
			{
				sorted = pagedFilter.Sort;
				pagedFilter.Sort = "";
			}
			var ali = _uowSciex.QueryStackSciex.AliHistorico.ListarPaginado<AliHistoricoVM>(o =>
				(
				   (pagedFilter.IdPliMercadoria == o.IdPliMercadoria && o.StatusAliAnterior == 3 && o.StatusLiAnterior == 1) ||
				   (pagedFilter.IdPliMercadoria == o.IdPliMercadoria && o.StatusAliAnterior == 6 && o.StatusLiAnterior == 1) ||
					(pagedFilter.IdPliMercadoria == o.IdPliMercadoria && o.StatusAliAnterior == 6 && o.StatusLiAnterior == 3) 

				),
				pagedFilter);

			if (pagedFilter.Reverse)
			{
				switch (sorted)
				{
					case "acao":
						{
							ali.Items = ali.Items.OrderBy(o => o.DescricaoStatus).ToList();
							break;
						}
					case "dataformatada":
						{
							ali.Items = ali.Items.OrderBy(o => o.DataFormadata).ToList();
							break;
						}
					case "cpfcnpjResponsavel":
						{
							ali.Items = ali.Items.OrderBy(o => o.CPFCNPJResponsavel).ToList();
							break;
						}
					case "nomeResponsavel":
						{
							ali.Items = ali.Items.OrderBy(o => o.NomeResponsavel).ToList();
							break;
						}
					case "observacao":
						{
							ali.Items = ali.Items.OrderBy(o => o.Observacao).ToList();
							break;
						}
					default:
						break;
				}
			}
			else
			{
				switch (sorted)
				{
					case "acao":
						{
							ali.Items = ali.Items.OrderByDescending(o => o.DescricaoStatus).ToList();
							break;
						}
					case "dataformatada":
						{
							ali.Items = ali.Items.OrderByDescending(o => o.DataFormadata).ToList();
							break;
						}
					case "cpfcnpjResponsavel":
						{
							ali.Items = ali.Items.OrderByDescending(o => o.CPFCNPJResponsavel).ToList();
							break;
						}
					case "nomeResponsavel":
						{
							ali.Items = ali.Items.OrderByDescending(o => o.NomeResponsavel).ToList();
							break;
						}
					case "observacao":
						{
							ali.Items = ali.Items.OrderByDescending(o => o.Observacao).ToList();
							break;
						}
					default:
						break;
				}
			}

			return ali;
		}

		public AliHistoricoVM RegrasSalvar(AliHistoricoVM AliHistoricoVM)
		{
			var entityAliHistorico = AutoMapper.Mapper.Map<AliHistoricoEntity>(AliHistoricoVM);
			entityAliHistorico.CPFCNPJResponsavel = entityAliHistorico.CPFCNPJResponsavel.CnpjCpfUnformat();
			_uowSciex.CommandStackSciex.AliHistorico.Salvar(entityAliHistorico);
			_uowSciex.CommandStackSciex.Save();

			var _AliHistoricoVM = AutoMapper.Mapper.Map<AliHistoricoVM>(entityAliHistorico);
			return _AliHistoricoVM;
		}

		public AliHistoricoVM Selecionar(int? numeroAliHistorico)
		{
			var AliHistoricoVM = new AliHistoricoVM();
			if (!numeroAliHistorico.HasValue) { return AliHistoricoVM; }

			var AliHistorico = _uowSciex.QueryStackSciex.AliHistorico.Selecionar(x => x.IdPliMercadoria == numeroAliHistorico);
			if (AliHistorico == null) { return AliHistoricoVM; }

			AliHistoricoVM = AutoMapper.Mapper.Map<AliHistoricoVM>(AliHistorico);
			return AliHistoricoVM;
		}

		public AliHistoricoVM VisuAliHistoricozar(AliHistoricoVM AliHistoricoVM)
		{
			var entity = _uowSciex.QueryStackSciex.AliHistorico.Selecionar(x => x.IdPliMercadoria == AliHistoricoVM.IdPliMercadoria);
			var retorno = AutoMapper.Mapper.Map<AliHistoricoVM>(entity);
			return retorno;
		}

		public void Salvar(AliHistoricoVM AliHistoricoVM)
		{
			RegrasSalvar(AliHistoricoVM);
		}

		public void Deletar(int id)
		{
			var AliHistorico = _uowSciex.QueryStackSciex.AliHistorico.Selecionar(s => s.IdPliMercadoria == id);
			if (AliHistorico != null)
			{
				_uowSciex.CommandStackSciex.AliHistorico.Apagar(AliHistorico.IdPliMercadoria);
			}
		}




	}
}
