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
	public class LEConsultarInsumoBll : ILEConsultarInsumoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;

		public LEConsultarInsumoBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf,
			 IViewImportadorBll viewImportadorBll, IComplementarPLIBll complementarPLIBll,
			 IUsuarioPssBll usuarioPssBll, IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_IViewImportadorBll = viewImportadorBll;
			_usuarioPssBll = usuarioPssBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
		}

		public IEnumerable<LEInsumoVM> Listar(LEInsumoVM vm)
		{
			var leInsumo = _uowSciex.QueryStackSciex.LEInsumo.Listar<LEInsumoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<LEInsumoVM>>(leInsumo);
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

		public PagedItems<LEInsumoVM> ListarPaginado(LEInsumoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<LEInsumoVM>(); }

			var insumos = _uowSciex.QueryStackSciex.LEInsumo.ListarPaginado<LEInsumoVM>(o =>
				(
					(
						pagedFilter.IdLe == 0 || o.IdLe.Equals(pagedFilter.IdLe)
					)
					&&
					(
						string.IsNullOrEmpty(pagedFilter.TipoInsumo) || o.TipoInsumo.Equals(pagedFilter.TipoInsumo)
					)
					&&
					(
						pagedFilter.CodigoNCM == null || o.CodigoNCM.Equals(pagedFilter.CodigoNCM)
					)
				),
				pagedFilter);

			foreach (var item in insumos.Items)
			{
				item.DescricaoTipoInsumo = item.TipoInsumo == "1" ? "P" : item.TipoInsumo == "2" ? "E" : item.TipoInsumo == "3" ? "N" : item.TipoInsumo == "4" ? "R" : " ";
				if(Convert.ToInt32(item.TipoInsumo) == 1)
				{
					if (Convert.ToInt32(item.CodigoNCM) > 0)
					{
						item.CodigoNCMFormatado = Convert.ToInt32(item.CodigoNCM).ToString("D8");
						item.IdCodigoNCM = _uowSciex.QueryStackSciex.ViewMercadoria.Selecionar(o => o.CodigoProdutoMercadoria == item.CodigoProduto && o.CodigoNCMMercadoria == item.CodigoNCM).IdMercadoria;
					}
				}
				else if (Convert.ToInt32(item.TipoInsumo) > 1)
				{
					if (Convert.ToInt32(item.CodigoNCM) > 0)
					{
						item.CodigoNCMFormatado = Convert.ToInt32(item.CodigoNCM).ToString("D8");
						item.IdCodigoNCM = _uowSciex.QueryStackSciex.ViewNcm.Selecionar(o => o.CodigoNCM == item.CodigoNCMFormatado ).IdNcm;
					}
				}
				
				if (item.CodigoDetalhe > 0)
				{
					var detalhe = _uowSciex.QueryStackSciex.ViewDetalheMercadoria.Selecionar(o => o.CodigoProduto == item.CodigoProduto && o.CodigoNCMMercadoria == item.CodigoNCM && o.CodigoDetalheMercadoria == item.CodigoDetalhe);
					item.IdCodigoDetalhe = detalhe.IdDetalheMercadoria;
				}
					
				if (item.CodigoUnidadeMedida > 0)
				{
					var obj = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == item.CodigoUnidadeMedida);
					item.DescricaoUnidadeMedida = obj.Sigla + " | " + obj.Descricao;
				}
			}

			return insumos;
		}

		public LEInsumoVM Salvar(LEInsumoVM vm)
		{
			var regInsumo = _uowSciex.QueryStackSciex.LEInsumo.Selecionar(o => o.IdLeInsumo == vm.IdLeInsumo);

			var usuLogado = _usuarioPssBll.ObterUsuarioLogado();


			regInsumo.SituacaoInsumo = vm.SituacaoInsumo;

			if (vm.SituacaoInsumo == 2)
			{
				var regInsumoErro = _uowSciex.QueryStackSciex.LEInsumoErro.Listar(q => q.IdLeInsumo == regInsumo.IdLeInsumo).LastOrDefault();

				if (regInsumoErro == null)
				{
					var insumoErro = new LEInsumoErroEntity()
					{
						IdLeInsumo = regInsumo.IdLeInsumo,
						DataErroRegistro = DateTime.Now,
						DescricaoErro = vm.DescricaoErro,
						CpfResponsavel = usuLogado.usuarioLogadoCpfCnpj.CnpjCpfUnformat(),
						NomeResponsavel = usuLogado.usuarioLogadoNome
					};

					_uowSciex.CommandStackSciex.LEInsumoErro.Salvar(insumoErro);
				}
				else
				{

					regInsumoErro.IdLeInsumo = regInsumo.IdLeInsumo;
					regInsumoErro.DataErroRegistro = DateTime.Now;
					regInsumoErro.DescricaoErro = vm.DescricaoErro;
					regInsumoErro.CpfResponsavel = usuLogado.usuarioLogadoCpfCnpj.CnpjCpfUnformat();
					regInsumoErro.NomeResponsavel = usuLogado.usuarioLogadoNome;

					_uowSciex.CommandStackSciex.LEInsumoErro.Salvar(regInsumoErro);
				}

				_uowSciex.CommandStackSciex.Save();
			}
			else
			{
				var listaRegInsumoErro = _uowSciex.QueryStackSciex.LEInsumoErro.Listar(q => q.IdLeInsumo == regInsumo.IdLeInsumo);

				if (listaRegInsumoErro.Count > 0)
				{
					foreach (var regInsumoErro in listaRegInsumoErro)
					{
						_uowSciex.CommandStackSciex.LEInsumoErro.Apagar(regInsumoErro.IdLeInsumoErro);
					}

					_uowSciex.CommandStackSciex.Save();
				}
			}

			_uowSciex.CommandStackSciex.LEInsumo.Salvar(regInsumo);
			_uowSciex.CommandStackSciex.Save();


			vm.Mensagem = "Salvo com sucesso";


			return vm;

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

		public bool VerificaExisteInsumo(LEInsumoVM vm)
		{
			var codigoUnidadeMedidaInt = Convert.ToInt32(vm.CodigoUnidadeMedida);
			var obj = _uowSciex.QueryStackSciex.LEInsumo.Listar(o =>
																	o.IdLeInsumo != vm.IdLeInsumo
																	&&
																	o.CodigoNCM.Equals(vm.CodigoNCM)
																	&&
																	o.CodigoDetalhe.Equals(vm.CodigoDetalhe)
																	&&
																	o.TipoInsumo.Equals(vm.TipoInsumo)
																	&&
																	o.DescricaoInsumo.Equals(vm.DescricaoInsumo)
																	&&
																	o.DescricaoEspecTecnica.Equals(vm.DescricaoEspecTecnica)
																	&&
																	o.ValorCoeficienteTecnico.Equals(vm.ValorCoeficienteTecnico)
																	&&
																	o.CodigoUnidadeMedida == codigoUnidadeMedidaInt
																);
			if (obj != null && obj.Count > 0)
				return true;
			else
				return false;
		}

		public LEInsumoVM Selecionar(int id)
		{
			var leInsumo = _uowSciex.QueryStackSciex.LEInsumo.SelecionarGrafo(q=> new LEInsumoVM() 
			{ 
				IdLe = q.IdLe,
				IdLeInsumo = q.IdLeInsumo,
				SituacaoInsumo = q.SituacaoInsumo,
				listaInsumoErro = q.listaLEInsumoErro.Select(w=> new LEInsumoErroVM()
				{
					IdLeInsumo = w.LEInsumo.IdLeInsumo,
					IdLeInsumoErro = w.IdLeInsumoErro,
					CpfResponsavel = w.CpfResponsavel,
					DataErroRegistro = w.DataErroRegistro,
					DescricaoErro = w.DescricaoErro,
					NomeResponsavel = w.NomeResponsavel,
					
				}).ToList(),
			} ,x => x.IdLeInsumo == id);

			leInsumo.UltimoInsumoErro = leInsumo.listaInsumoErro.LastOrDefault();

			return leInsumo;

			//leInsumo.DescCodigoProdutoSuframa = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
			//				.Listar(
			//						o => (
			//								leProd.Cnpj == null || (o.Cnpj.Equals(leProd.Cnpj))
			//							 )
			//					   )
			//				.Where(o =>
			//							(
			//								leProd.CodigoProdutoSuframa == 0 || (o.CodigoProduto.ToString().Contains(leProd.CodigoProdutoSuframa.ToString()))
			//							)
			//						)
			//				.GroupBy(o => new { o.CodigoProduto, o.DescricaoProduto })
			//				.Select(
			//							s => new
			//							{
			//								id = s.Key.CodigoProduto,
			//								text = (s.Key.CodigoProduto.ToString("D4") + " | " + s.Key.DescricaoProduto)
			//							}
			//					   ).FirstOrDefault().text;

			//leInsumo.DescCodigoTipoProduto = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
			//			.Listar(
			//					o => (
			//							leProd.Cnpj == null || (o.Cnpj.Equals(leProd.Cnpj))
			//						 )
			//				   )
			//			.Where(o =>
			//						(
			//							leProd.CodigoProdutoSuframa == 0 || (o.CodigoProduto.ToString().Contains(leProd.CodigoProdutoSuframa.ToString()))
			//						)
			//						&&
			//						(
			//							leProd.CodigoTipoProduto == 0 || (o.CodigoTipoProduto.ToString().Contains(leProd.CodigoTipoProduto.ToString()))
			//						)
			//					)
			//			.GroupBy(o => new { o.CodigoTipoProduto, o.DescricaoTipoProduto })
			//			.Select(
			//						s => new
			//						{
			//							id = s.Key.CodigoTipoProduto,
			//							text = (s.Key.CodigoTipoProduto.ToString("D4") + " | " + s.Key.DescricaoTipoProduto)
			//						}
			//				   ).FirstOrDefault().text;

			//var ncm = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
			//			.Listar(
			//					o => (
			//							leProd.Cnpj == null || (o.Cnpj.Equals(leProd.Cnpj))
			//						 )
			//				   )
			//			.Where(o =>
			//						(
			//							leProd.CodigoProdutoSuframa == 0 || (o.CodigoProduto.ToString().Contains(leProd.CodigoProdutoSuframa.ToString()))
			//						)
			//						&&
			//						(
			//							leProd.CodigoTipoProduto == 0 || (o.CodigoTipoProduto.ToString().Contains(leProd.CodigoTipoProduto.ToString()))
			//						)
			//						&&
			//						(
			//							string.IsNullOrEmpty(leProd.CodigoNCM) || (o.CodigoNCM.Equals(leProd.CodigoNCM))
			//						)
			//					)
			//			.GroupBy(o => o.CodigoNCM)
			//			.Select(
			//						s => new
			//						{
			//							id = s.Select(x => x.IdProdutoEmpresaExportacao).FirstOrDefault(),
			//							text = string.Format(s.Select(x => x.CodigoNCM).FirstOrDefault(), "D4") + " | " + s.Select(x => x.DescricaoNCM).FirstOrDefault()
			//						}
			//					).FirstOrDefault();

			//leProd.DescCodigoNCM = ncm.text;

			//var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida
			//	.Listar(o =>
			//						(
			//							leProd.IdUnidadeMedida == 0 || o.CodigoUnidadeMedida.Equals(leProd.CodigoUnidadeMedida)
			//						)
			//				   ).FirstOrDefault();

			//leProd.DescCodigoUnidadeMedida = undMed.Sigla + " | " + undMed.Descricao;

			//leProd.DataCadastroFormatada = leProd.DataCadastro.Value.ToShortDateString();
			//leProd.DescStatusLE = leProd.StatusLE == 1 ? "Em Elaboração" : leProd.StatusLE == 2 ? "Entregue" : leProd.StatusLE == 3 ? "Aguardando Aprovação" : "";


		}

		public void Deletar(long id)
		{
			var leInsumo = _uowSciex.QueryStackSciex.LEInsumo.Selecionar(s => s.IdLeInsumo == id);

			var listaAtualizar = _uowSciex.QueryStackSciex.LEInsumo.Listar(o => o.IdLe == leInsumo.IdLe && o.CodigoInsumo > leInsumo.CodigoInsumo);
			foreach (var item in listaAtualizar)
			{
				item.CodigoInsumo--;
				_uowSciex.CommandStackSciex.LEInsumo.Salvar(item);
			}
			_uowSciex.CommandStackSciex.Save();

			if (leInsumo != null)
			{
				_uowSciex.CommandStackSciex.LEInsumo.Apagar(leInsumo.IdLeInsumo);
			}
			_uowSciex.CommandStackSciex.Save();
		}

	}
}