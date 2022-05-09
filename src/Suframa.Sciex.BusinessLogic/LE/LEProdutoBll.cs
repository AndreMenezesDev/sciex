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
	public class LEProdutoBll : ILEProdutoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;

		public LEProdutoBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf,
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

		public IEnumerable<object> ListarChave(LEProdutoVM vm)
		{

			if (vm.Descricao == null && vm.Id == 0)
			{
				return new List<object>();
			}

			try
			{
				string descricao = vm.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				long valor = Convert.ToInt32(vm.Descricao);
				vm.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.LEProduto
					.Listar(
									o => (
											vm.Cnpj == null || (o.Cnpj.Equals(vm.Cnpj))
										 )
								   )
							.Where(o =>
										(
											vm.Descricao == null || (o.CodigoProduto.ToString().Contains(valor.ToString()))
										)
										&&
										(
											vm.Id == 0 || (o.IdLe.Equals(vm.Id))
										)
									)
							.GroupBy(o => new { o.CodigoProduto, o.DescricaoModelo })
							.Select(
										s => new
										{
											id = s.Select(x => x.IdLe).FirstOrDefault(),
											text = s.Select(x => x.CodigoProduto).FirstOrDefault().ToString("D4") + " | " + s.Select(x => x.DescricaoModelo).FirstOrDefault()
										}
								   );

				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.LEProduto
				.Listar(
									o => (
											vm.Cnpj == null || (o.Cnpj.Equals(vm.Cnpj))
										 )
								   )
							.Where(o =>
										(
											vm.Descricao == null 
											|| 
											(	
												(o.DescricaoModelo != null && o.DescricaoModelo.ToLower().Contains(vm.Descricao.ToLower()))
												|| 
												(o.CodigoNCM != null && o.CodigoNCM.ToLower().Contains(vm.Descricao.ToLower()))
											)
										)
										&&
										(
											vm.Id == 0 || (o.IdLe.Equals(vm.Id))
										)
									)
							.GroupBy(o => new { o.CodigoProduto, o.DescricaoModelo })
							.Select(
										s => new
										{
											id = s.Select(x => x.IdLe).FirstOrDefault(),
											text = s.Select(x => x.CodigoProduto).FirstOrDefault().ToString("D4") + " | " + s.Select(x => x.DescricaoModelo).FirstOrDefault()
										}
								   );

				return lista;
			}
		}

		public PagedItems<LEProdutoVM> ListarPaginado(LEProdutoVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

			//var usuCpfCnpjEmpresaOuLogado = _usuarioPssBll.ObterUsuarioLogado().empresaRepresentadaCnpj.CnpjCpfUnformat();
			var usuCpfCnpjEmpresaOuLogado = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();

			if (pagedFilter.IdCodigoProdutoSuframa > 0)
			{
				var codigoTipoProdutoEntity = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Selecionar(o => o.IdProdutoEmpresaExportacao == pagedFilter.IdCodigoProdutoSuframa);
				if (codigoTipoProdutoEntity != null)
				{
					pagedFilter.CodigoProdutoSuframa = codigoTipoProdutoEntity.CodigoProduto;
				}
			}

			if (pagedFilter == null) { return new PagedItems<LEProdutoVM>(); }

			var le = _uowSciex.QueryStackSciex.LEProduto.ListarPaginadoGrafo(q=> new LEProdutoVM()
				{
					IdLe = q.IdLe,
					InscricaoCadastral = q.InscricaoCadastral,
					CodigoProduto = q.CodigoProduto,
					CodigoProdutoSuframa = q.CodigoProdutoSuframa,
					CodigoTipoProduto = q.CodigoTipoProduto,
					CodigoUnidadeMedida = q.CodigoUnidadeMedida,
					DescricaoModelo = q.DescricaoModelo,
					StatusLE = q.StatusLE,
					StatusLEAlteracao = q.StatusLEAlteracao != null ? (int)q.StatusLEAlteracao : (int)0,
					DataEnvio = q.DataEnvio,
					DataAprovacao = q.DataAprovacao,
					CodigoModeloEmpresa = q.CodigoModeloEmpresa,
					DescricaoCentroCusto = q.DescricaoCentroCusto,
					CpfResponsavel = q.CpfResponsavel,
					NomeResponsavel = q.NomeResponsavel,
					CodigoNCM = q.CodigoNCM,
					DataCadastro = q.DataCadastro,
					RazaoSocial = q.RazaoSocial,
					Cnpj = q.Cnpj
			}
				,
				o =>
				(
					(
						pagedFilter.CodigoProdutoSuframa == 0 || o.CodigoProdutoSuframa.Equals(pagedFilter.CodigoProdutoSuframa)
					)
					&&
					(
						pagedFilter.StatusLE == 0 || o.StatusLE.Equals(pagedFilter.StatusLE)
					)
					&&
					(
						(pagedFilter.DataInicio == null) || (o.DataCadastro >= dataInicio && o.DataCadastro <= dataFim)
					)
					&&
					(
						(o.Cnpj == usuCpfCnpjEmpresaOuLogado && usuCpfCnpjEmpresaOuLogado.Length == 14)
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
				item.DescStatusLE = item.StatusLE == 1 ? "Em Elaboração" 
									: item.StatusLE == 2 ? "Entregue" 
									: item.StatusLE == 3 ? "Aguardando Aprovação" 
									: item.StatusLE == 4 ? "Aprovada" 
									: item.StatusLE == 5 ? "Bloqueada" 
									: "-";

				item.DescStatusLEAlteracao = item.StatusLEAlteracao == 1 ? "Em Elaboração" 
											: item.StatusLEAlteracao == 2 ? "Entregue" 
											: item.StatusLEAlteracao == 3 ? "Aguardando Aprovação" 
											: item.StatusLEAlteracao == 4 ? "Concluído" 
											: "-";
			}


			return le;

		}

		public LEProdutoVM Salvar(LEProdutoVM vm)
		{
			if (vm == null)
				return new LEProdutoVM() { MensagemErro = "Não foi possivel incluir o produto!" };

			var usuarioLogado = _usuarioPssBll.ObterUsuarioLogado();
			//vm.Cnpj = usuarioLogado.empresaRepresentadaCnpj.CnpjCpfUnformat();
			vm.Cnpj = usuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();
			vm.InscricaoCadastral = usuarioLogado.usuInscricaoCadastral;
			vm.RazaoSocial = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(w => w.Cnpj == vm.Cnpj).RazaoSocial;

			if (vm.IdCodigoProdutoSuframa > 0)
			{
				var codigoTipoProdutoEntity = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Selecionar(o => o.IdProdutoEmpresaExportacao == vm.IdCodigoProdutoSuframa);
				if (codigoTipoProdutoEntity != null)
				{
					vm.CodigoProdutoSuframa = codigoTipoProdutoEntity.CodigoProduto;
				}
			}
			if (vm.IdCodigoTipoProduto > 0)
			{
				var codigoTipoProdutoEntity = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Selecionar(o => o.IdProdutoEmpresaExportacao == vm.IdCodigoTipoProduto);
				if(codigoTipoProdutoEntity != null)
				{
					vm.CodigoTipoProduto = codigoTipoProdutoEntity.CodigoTipoProduto;
				}
			}
			if (vm.IdCodigoNCM > 0)
			{
				var codigoNcmEntity = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Selecionar(o => o.IdProdutoEmpresaExportacao == vm.IdCodigoNCM);
				if (codigoNcmEntity != null)
				{
					vm.CodigoNCM = codigoNcmEntity.CodigoNCM;
				}
			}
			if(vm.IdUnidadeMedida > 0)
			{
				var codigoUnidadeMedida = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.IdUnidadeMedida == vm.IdUnidadeMedida);
				if (codigoUnidadeMedida != null)
				{
					vm.CodigoUnidadeMedida = codigoUnidadeMedida.CodigoUnidadeMedida;
				}
			}

			if (VerificaExisteProduto(vm))
				return new LEProdutoVM() { MensagemErro = "Produto já cadastrado!" };

			var ultimoProdEmpresa = _uowSciex.QueryStackSciex.LEProduto.Listar(o => o.Cnpj == vm.Cnpj).LastOrDefault();

			if (ultimoProdEmpresa == null)
				vm.CodigoProduto = 1;
			else
				vm.CodigoProduto = ultimoProdEmpresa.CodigoProduto + 1;

			var prod = new LEProdutoEntity();
			prod.CodigoProduto = vm.CodigoProduto;
			prod.CodigoProdutoSuframa = vm.CodigoProdutoSuframa;
			prod.CodigoTipoProduto = vm.CodigoTipoProduto;
			prod.CodigoNCM = vm.CodigoNCM;
			prod.DescricaoModelo = vm.DescricaoModelo;
			prod.CodigoUnidadeMedida = vm.CodigoUnidadeMedida;
			prod.StatusLE = 1;
			prod.DataCadastro = DateTime.Now;
			prod.CodigoModeloEmpresa = vm.CodigoModeloEmpresa;
			prod.DescricaoCentroCusto = vm.DescricaoCentroCusto;
			prod.Cnpj = vm.Cnpj;
			prod.InscricaoCadastral = vm.InscricaoCadastral;
			prod.RazaoSocial = vm.RazaoSocial;

			_uowSciex.CommandStackSciex.LEProduto.Salvar(prod);
			_uowSciex.CommandStackSciex.Save();

			vm.Mensagem = "Salvo com sucesso";

			return vm;
		}

		public LEProdutoVM Entregar(LEProdutoVM vm)
		{
			if (vm == null)
				return new LEProdutoVM();

			var leProd = _uowSciex.QueryStackSciex.LEProduto.Selecionar(o => o.IdLe == vm.IdLe);

			bool possuiInsumoComTipoAlteracao = false;
			foreach (var regInsumo in leProd.LEInsumo)
			{
				if (regInsumo.TipoInsumoAlteracao == 2 || regInsumo.TipoInsumoAlteracao == 3)
				{
					possuiInsumoComTipoAlteracao = true;
				}
			}

			if (possuiInsumoComTipoAlteracao || leProd.StatusLE == 1 || leProd.StatusLE == 5)
			{


				if (leProd.StatusLE == 1 || leProd.StatusLE == 5)
				{
					if (leProd.StatusLE == 5)
					{
						leProd.StatusLEAlteracao = 2;
						leProd.DataEnvio = DateTime.Now;
					}
					leProd.StatusLE = 2;
					leProd.DataEnvio = DateTime.Now;
				}
				else if (leProd.StatusLEAlteracao == 1 )
				{
					leProd.StatusLEAlteracao = 2;
					leProd.DataEnvio = DateTime.Now;
				}


				_uowSciex.CommandStackSciex.LEProduto.Salvar(leProd);
				_uowSciex.CommandStackSciex.Save();



				return new LEProdutoVM() { Mensagem = "Entrega realizada com sucesso!" };
			}
			else
			{
				return new LEProdutoVM() { Mensagem = "Não existem solicitações de alteração de insumo" };
			}
		}
		

		public LEProdutoVM SolicitarAlteracaoLE(LEProdutoVM vm)
		{
			if (vm == null)
				return new LEProdutoVM();

			var leProd = _uowSciex.QueryStackSciex.LEProduto.Selecionar(o => o.IdLe == vm.IdLe);

			leProd.StatusLEAlteracao = 1;


			_uowSciex.CommandStackSciex.LEProduto.Salvar(leProd);
			_uowSciex.CommandStackSciex.Save();



			return new LEProdutoVM() { Mensagem = "Sucesso" };
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
										leProd.CodigoProdutoSuframa == 0 || (o.CodigoProduto == leProd.CodigoProdutoSuframa)
									)
									&&
									(
										leProd.CodigoTipoProduto == 0 || (o.CodigoTipoProduto == leProd.CodigoTipoProduto)
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
			leProd.IdCodigoNCM = ncm.id;

			var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida
				.Listar(o =>
									(
										leProd.CodigoUnidadeMedida == 0 || o.CodigoUnidadeMedida.Equals(leProd.CodigoUnidadeMedida)
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


				var listaRegHistoricoProduto = _uowSciex.QueryStackSciex.LEProdutoHistorico.Listar(q => q.IdLe == leProduto.IdLe);

				if (listaRegHistoricoProduto.Count > 0)
				{
					foreach (var regHist in listaRegHistoricoProduto)
					{
						_uowSciex.CommandStackSciex.LEProdutoHistorico.Apagar(regHist.IdLeh);
					}
				}

				_uowSciex.CommandStackSciex.LEProduto.Apagar(leProduto.IdLe);
			}
			_uowSciex.CommandStackSciex.Save();
		}

	}
}