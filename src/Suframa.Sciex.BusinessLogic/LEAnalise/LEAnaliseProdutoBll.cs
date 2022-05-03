using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using Suframa.Sciex.BusinessLogic.Pss;
using System.Text;
using Suframa.Sciex.CrossCutting.Extension;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls.Expressions;

namespace Suframa.Sciex.BusinessLogic
{
	public class LEAnaliseProdutoBll : ILEAnaliseProdutoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;

		public LEAnaliseProdutoBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf,
			 IViewImportadorBll viewImportadorBll, IComplementarPLIBll complementarPLIBll,
			 IUsuarioPssBll usuarioPssBll, IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_IViewImportadorBll = viewImportadorBll;
			_usuarioPssBll = usuarioPssBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
		}

		public IEnumerable<LEProdutoVM> Listar(LEProdutoVM vm)
		{
			var leProd = _uowSciex.QueryStackSciex.LEProduto.Listar<LEProdutoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<LEProdutoVM>>(leProd);
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

		public PagedItems<LEProdutoVM> ListarPaginado(LEProdutoVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);
			
			var usuCpfCnpjEmpresaOuLogado = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado .CnpjCpfUnformat();

			if (pagedFilter == null) { return new PagedItems<LEProdutoVM>(); }

			string OrdenacaoPosterior = null;
			if (pagedFilter.Sort == "Empresa" || pagedFilter.Sort == "QtdeInsumo" || pagedFilter.Sort == "DescricaoProduto")
			{
				OrdenacaoPosterior = pagedFilter.Sort;
				pagedFilter.Sort = null;
			}
			
			List<int?> listaDeStatus = new List<int?>() { (int)EnumSituacaoListagemExportacao.AGUARDANDO_APROVAÇÃO };

			var le = _uowSciex.QueryStackSciex.LEProduto.ListarPaginado<LEProdutoVM>(o =>
				(
					
					(
						pagedFilter.InscricaoCadastral == 0 || o.InscricaoCadastral.Equals(pagedFilter.InscricaoCadastral)
					)
					&&
					(
						pagedFilter.RazaoSocial == null || o.RazaoSocial.Equals(pagedFilter.RazaoSocial)
					)
					&&
					(
						pagedFilter.CodigoProduto == 0 || o.CodigoProduto.Equals(pagedFilter.CodigoProduto)
					)
					&&
					(
						(pagedFilter.DataInicio == null) || (o.DataEnvio >= dataInicio && o.DataEnvio <= dataFim)
					)
					&&
					(
						(pagedFilter.StatusLE == 0 && (listaDeStatus.Contains(o.StatusLE) || listaDeStatus.Contains(o.StatusLEAlteracao)) ) 
						|| 
						o.StatusLE.Equals(pagedFilter.StatusLE) || listaDeStatus.Contains(o.StatusLEAlteracao)
					)
					&&
					(
						o.CpfResponsavel == usuCpfCnpjEmpresaOuLogado
					)
				),
				pagedFilter);

			foreach (var item in le.Items)
			{
				item.DescCodigoProdutoSuframa = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
							.Listar(
									o => (
											item.Cnpj == null || (o.Cnpj.Equals(item.Cnpj))
										 )
								   )
							.Where(o =>
										(
											item.CodigoProdutoSuframa == 0 || (o.CodigoProduto.ToString().Contains(item.CodigoProdutoSuframa.ToString()))
										)
									)
							.GroupBy(o => new { o.CodigoProduto, o.DescricaoProduto })
							.Select(
										s => new
										{
											id = s.Key.CodigoProduto,
											text = (s.Key.CodigoProduto.ToString("D4") + " | " + s.Key.DescricaoProduto)
										}
								   ).FirstOrDefault().text;

				item.DescCodigoTipoProduto = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
							.Listar(
									o => (
											item.Cnpj == null || (o.Cnpj.Equals(item.Cnpj))
										 )
								   )
							.Where(o =>
										(
											item.CodigoProdutoSuframa == 0 || (o.CodigoProduto.ToString().Contains(item.CodigoProdutoSuframa.ToString()))
										)
										&&
										(
											item.CodigoTipoProduto == 0 || (o.CodigoTipoProduto.ToString().Contains(item.CodigoTipoProduto.ToString()))
										)
									)
							.GroupBy(o => new { o.CodigoTipoProduto, o.DescricaoTipoProduto })
							.Select(
										s => new
										{
											id = s.Key.CodigoTipoProduto,
											text = (s.Key.CodigoTipoProduto.ToString("D4") + " | " + s.Key.DescricaoTipoProduto)
										}
								   ).FirstOrDefault().text;

				item.DataCadastroFormatada = item.DataCadastro.Value.ToShortDateString();

				if (item.StatusLEAlteracao != 0)
				{
					item.DescStatusLEAlteracao = item.StatusLEAlteracao == 1 ? "Em Elaboração"
														: item.StatusLEAlteracao == 2 ? "Entregue"
														: item.StatusLEAlteracao == 3 ? "Aguardando Aprovação"
														: item.StatusLEAlteracao == 4 ? "Aprovada"
														: item.StatusLEAlteracao == 5 ? "Bloqueada"
														: "-"; 
				}

				item.DescStatusLE = item.StatusLE == 1 ? "Em Elaboração"
									: item.StatusLE == 2 ? "Entregue"
									: item.StatusLE == 3 ? "Aguardando Aprovação"
									: item.StatusLE == 4 ? "Aprovada"
									: item.StatusLE == 5 ? "Bloqueada"
									: "-";

				item.QtdInsumo = _uowSciex.QueryStackSciex.ContarQuantidadeInsumoPorProdutoEInscricaoCad(item.InscricaoCadastral, item.IdLe, item.StatusLEAlteracao);

				item.RazaoSocial = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(w => w.Cnpj == item.Cnpj).RazaoSocial;

				if (usuCpfCnpjEmpresaOuLogado.Length == 11)
				{
					item.isUsuarioInterno = true;
				}
				else
				{
					item.isUsuarioInterno = false;
				}
			}


			if (OrdenacaoPosterior != null)
			{
				if (OrdenacaoPosterior.Equals("Empresa"))
				{
					if (pagedFilter.Reverse)
					{
						le.Items = le.Items.OrderBy(q => q.RazaoSocial).ThenBy(q => q.RazaoSocial).ToList();
					}
					else
					{
						le.Items = le.Items.OrderBy(q => q.RazaoSocial).ThenByDescending(q => q.RazaoSocial).ToList();
					}

				}
				else if (OrdenacaoPosterior.Equals("QtdeInsumo"))
				{
					if (pagedFilter.Reverse)
					{
						le.Items = le.Items.OrderBy(q => q.QtdInsumo).ThenBy(q => q.QtdInsumo).ToList();
					}
					else
					{
						le.Items = le.Items.OrderBy(q => q.QtdInsumo).ThenByDescending(q => q.QtdInsumo).ToList();
					}

				}
				else if (OrdenacaoPosterior.Equals("DescricaoProduto"))
				{
					if (pagedFilter.Reverse)
					{
						le.Items = le.Items.OrderBy(q => q.DescCodigoProdutoSuframa).ThenBy(q => q.DescCodigoProdutoSuframa).ToList();
					}
					else
					{
						le.Items = le.Items.OrderBy(q => q.DescCodigoProdutoSuframa).ThenByDescending(q => q.DescCodigoProdutoSuframa).ToList();
					}

				}
			}


			return le;

		}

		enum EnumSituacaoInsumo
		{
			ATIVO = 1,
			INATIVO = 2
		}

		enum EnumSituacaoProduto
		{
			APROVADO = 4,
			BLOQUEADO = 5
		}
		enum EnumSituacaoAlteracao
		{
			CONCLUIDO = 4
		}

		public LEInsumoVM SalvarAnalise(LEInsumoVM vm)
		{

			if (vm == null) { return new LEInsumoVM(); }


			var regProduto = _uowSciex.QueryStackSciex.LEProduto.Selecionar(q => q.IdLe == vm.IdLe);

			bool existeInsumoInativo = false;
			foreach (var insumo in regProduto.LEInsumo)
			{
				if (insumo.SituacaoInsumo == (int)EnumSituacaoInsumo.INATIVO)
				{
					existeInsumoInativo = true;
				}
			}

			if (!existeInsumoInativo)
			{
				regProduto.StatusLE = (int)EnumSituacaoListagemExportacao.APROVADA;

				int entregue = 2;
				int emAnalise = 3;
				int aprovada = 4;
				int bloqueada = 5;
				if (regProduto.StatusLEAlteracao == emAnalise || regProduto.StatusLEAlteracao == bloqueada || regProduto.StatusLEAlteracao == entregue)
				{
					regProduto.StatusLEAlteracao = aprovada;
				}
			}
			else
			{
				regProduto.StatusLE = (int)EnumSituacaoListagemExportacao.BLOQUEADA;

				int entregue = 2;
				int emAnalise = 3;
				int bloqueada = 5;
				if (regProduto.StatusLEAlteracao == emAnalise || regProduto.StatusLEAlteracao == entregue)
				{
					regProduto.StatusLEAlteracao = bloqueada;
				}
			}

			_uowSciex.CommandStackSciex.LEProduto.Salvar(regProduto);

			_uowSciex.CommandStackSciex.Save();

			vm.Mensagem = "Salvo com sucesso";

			return vm;

		}

		public LEProdutoVM AprovarAnalise(LEProdutoVM vm)
		{
			if (vm == null) { return new LEProdutoVM(); }


			var regProduto = _uowSciex.QueryStackSciex.LEProduto.Selecionar(q => q.IdLe == vm.IdLe);
			regProduto.StatusLEAlteracao = (int)EnumSituacaoAlteracao.CONCLUIDO;

			if (vm.isAprovarAnalise) //BOTÃO ANALISE LE INSUMOS APROVADO
			{
				regProduto.StatusLE = (int)EnumSituacaoProduto.APROVADO;
				regProduto.DataAprovacao = DateTime.Now;

				//foreach (var insumo in regProduto.LEInsumo)
				//{
				//	insumo.SituacaoInsumo = (int)EnumSituacaoInsumo.ATIVO;
				//	_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo);
				//}

				vm.Mensagem = "Aprovado com sucesso";
			}
			else // BOTÃO ANALISE LE INSUMOS BLOQUEADO
			{
				regProduto.StatusLE = (int)EnumSituacaoProduto.BLOQUEADO;

				//foreach (var insumo in regProduto.LEInsumo)
				//{
				//	insumo.SituacaoInsumo = (int)EnumSituacaoInsumo.INATIVO;
				//	_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo);
				//}

				vm.Mensagem = "Bloqueado com sucesso";
			}

			_uowSciex.CommandStackSciex.LEProduto.Salvar(regProduto);

			_uowSciex.CommandStackSciex.Save();

			return vm;
		}

		public LEProdutoVM Entregar(LEProdutoVM vm)
		{
			if (vm == null)
				return new LEProdutoVM();

			var leProd = _uowSciex.QueryStackSciex.LEProduto.Selecionar(o => o.IdLe == vm.IdLe);

			leProd.StatusLE = 2;
			leProd.DataEnvio = DateTime.Now;

			_uowSciex.CommandStackSciex.LEProduto.Salvar(leProd);
			_uowSciex.CommandStackSciex.Save();


			
			return new LEProdutoVM() { Mensagem = "Entrega realizada com sucesso!"};
		}

		public bool VerificaExisteProduto(LEProdutoVM vm)
		{
			var obj = _uowSciex.QueryStackSciex.LEProduto.Listar(o =>
			//														o.Cnpj.Equals(vm.Cnpj)
			//														&&
																	o.CodigoProdutoSuframa.Equals(vm.CodigoProdutoSuframa)
																	&&
																	o.CodigoTipoProduto.Equals(vm.CodigoTipoProduto)
																	&&
																	o.CodigoNCM.Equals(vm.CodigoNCM)
																	&&
																	o.CodigoUnidadeMedida.Equals(vm.CodigoUnidadeMedida)
																	&&
																	o.DescricaoModelo.ToLower().Equals(vm.DescricaoModelo.ToLower())
																);
			if (obj != null && obj.Count > 0)
				return true;
			else
				return false;
		}

		public LEProdutoVM Selecionar(int id)
		{
			var leProd = _uowSciex.QueryStackSciex.LEProduto.Selecionar<LEProdutoVM>(x => x.IdLe == id);

			leProd.DescCodigoProdutoSuframa = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
							.Listar(
									o => (
											leProd.Cnpj == null || (o.Cnpj.Equals(leProd.Cnpj))
										 )
								   )
							.Where(o =>
										(
											leProd.CodigoProdutoSuframa == 0 || (o.CodigoProduto.ToString().Contains(leProd.CodigoProdutoSuframa.ToString()))
										)
									)
							.GroupBy(o => new { o.CodigoProduto, o.DescricaoProduto })
							.Select(
										s => new
										{
											id = s.Key.CodigoProduto,
											text = (s.Key.CodigoProduto.ToString("D4") + " | " + s.Key.DescricaoProduto)
										}
								   ).FirstOrDefault().text;

			leProd.DescCodigoTipoProduto = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
						.Listar(
								o => (
										leProd.Cnpj == null || (o.Cnpj.Equals(leProd.Cnpj))
									 )
							   )
						.Where(o =>
									(
										leProd.CodigoProdutoSuframa == 0 || (o.CodigoProduto.ToString().Contains(leProd.CodigoProdutoSuframa.ToString()))
									)
									&&
									(
										leProd.CodigoTipoProduto == 0 || (o.CodigoTipoProduto.ToString().Contains(leProd.CodigoTipoProduto.ToString()))
									)
								)
						.GroupBy(o => new { o.CodigoTipoProduto, o.DescricaoTipoProduto })
						.Select(
									s => new
									{
										id = s.Key.CodigoTipoProduto,
										text = (s.Key.CodigoTipoProduto.ToString("D4") + " | " + s.Key.DescricaoTipoProduto)
									}
							   ).FirstOrDefault().text;

			var ncm = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
						.Listar(
								o => (
										leProd.Cnpj == null || (o.Cnpj.Equals(leProd.Cnpj))
									 )
							   )
						.Where(o =>
									(
										leProd.CodigoProdutoSuframa == 0 || (o.CodigoProduto.ToString().Contains(leProd.CodigoProdutoSuframa.ToString()))
									)
									&&
									(
										leProd.CodigoTipoProduto == 0 || (o.CodigoTipoProduto.ToString().Contains(leProd.CodigoTipoProduto.ToString()))
									)
									&&
									(
										string.IsNullOrEmpty(leProd.CodigoNCM) || (o.CodigoNCM.Equals(leProd.CodigoNCM))
									)
								)
						.GroupBy(o => o.CodigoNCM)
						.Select(
									s => new
									{
										id = s.Select(x => x.IdProdutoEmpresaExportacao).FirstOrDefault(),
										text = string.Format(s.Select(x => x.CodigoNCM).FirstOrDefault(), "D4") + " | " + s.Select(x => x.DescricaoNCM).FirstOrDefault()
									}
								).FirstOrDefault();

			leProd.DescCodigoNCM = ncm.text;

			var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida
				.Listar(o =>
									(
										leProd.IdUnidadeMedida == 0 || o.CodigoUnidadeMedida.Equals(leProd.CodigoUnidadeMedida)
									)
							   ).FirstOrDefault();

			leProd.DescCodigoUnidadeMedida = undMed.Sigla + " | " + undMed.Descricao;

			leProd.DataCadastroFormatada = leProd.DataCadastro.Value.ToShortDateString();
			leProd.DescStatusLE = leProd.StatusLE == 1 ? "Em Elaboração" : leProd.StatusLE == 2 ? "Entregue" : leProd.StatusLE == 3 ? "Aguardando Aprovação" : "";

			return leProd;
		}

		public void Deletar(long id)
		{
			var leProduto = _uowSciex.QueryStackSciex.LEProduto.Selecionar(s => s.IdLe == id);


			if (leProduto != null)
			{
				var listaAtualizar = _uowSciex.QueryStackSciex.LEProduto.Listar(o => o.Cnpj == leProduto.Cnpj && o.CodigoProduto > leProduto.CodigoProduto);
				foreach (var item in listaAtualizar)
				{
					item.CodigoProduto--;
					_uowSciex.CommandStackSciex.LEProduto.Salvar(item);
				}
				_uowSciex.CommandStackSciex.Save();

				var leInsumos = _uowSciex.QueryStackSciex.LEInsumo.Listar(o => o.IdLe == leProduto.IdLe);

				if(leInsumos != null && leInsumos.Count > 0)
				{
					foreach (var item in leInsumos)
					{
						_uowSciex.CommandStackSciex.LEInsumo.Apagar(item.IdLeInsumo);
					}
					_uowSciex.CommandStackSciex.Save();
				}

				_uowSciex.CommandStackSciex.LEProduto.Apagar(leProduto.IdLe);
			}
			_uowSciex.CommandStackSciex.Save();
		}

	}
}