using FluentValidation;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Compilation;

namespace Suframa.Sciex.BusinessLogic
{
	public class ViewProdEmpSufBll : IViewProdEmpSufBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly Validation _validation;

		public ViewProdEmpSufBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioPssBll = usuarioPssBll;
			_validation = new Validation();
		}

		public ViewProdEmpSufVM Selecionar(string desc)
		{
			return null;
		}

		public IEnumerable<object> ListarChaveProduto(ViewProdEmpSufVM vm)
		{

			if (vm.Descricao == null && vm.Id == null)
			{
				return new List<object>();
			}

			var usuarioLogado = _usuarioPssBll.ObterUsuarioLogado();
			vm.Cnpj = !PrivateSettings.DEVELOPMENT_MODE ? usuarioLogado.empresaRepresentadaCnpj.CnpjCpfUnformat() 
														: usuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();

			try
			{
				string descricao = vm.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				short valor = Convert.ToInt16(vm.Descricao);
				vm.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
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
											vm.Id == null || (o.IdProdutoEmpresaExportacao.Equals(vm.Id.Value))
										)
									)
							.GroupBy(o => new { o.CodigoProduto, o.DescricaoProduto })
							.Select(
										s => new
										{
											id = s.Select(x => x.IdProdutoEmpresaExportacao).FirstOrDefault(),
											text = s.Select(x => x.CodigoProduto).FirstOrDefault().ToString("D4") + " | " + s.Select(x => x.DescricaoProduto).FirstOrDefault()
										}
								   );
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
							.Listar(
									o => (
											vm.Cnpj == null || (o.Cnpj.Equals(vm.Cnpj))
										 )
								   )
							.Where(o =>
										(
											vm.Descricao == null || (o.DescricaoProduto.ToLower().Contains(vm.Descricao.ToLower())) || (o.CodigoProduto.ToString().ToLower().Contains(vm.Descricao.ToLower()))
										)
										&&
										(
											vm.Id == null || (o.IdProdutoEmpresaExportacao.Equals(vm.Id.Value))
										)
									)
							.GroupBy(o => new { o.CodigoProduto, o.DescricaoProduto })
							.Select(
										s => new
										{
											id = s.Select(x => x.IdProdutoEmpresaExportacao).FirstOrDefault(),
											text = s.Select(x => x.CodigoProduto).FirstOrDefault().ToString("D4") + " | " + s.Select(x => x.DescricaoProduto).FirstOrDefault()
										}
								   );
				return lista;
			}
		}

		public IEnumerable<object> ListarChaveTipoProduto(ViewProdEmpSufVM vm)
		{

			if (vm.Descricao == null && vm.Id == null)
			{
				return new List<object>();
			}

			var usuarioLogado = _usuarioPssBll.ObterUsuarioLogado();
			vm.Cnpj = !PrivateSettings.DEVELOPMENT_MODE ? usuarioLogado.empresaRepresentadaCnpj.CnpjCpfUnformat()
														: usuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();

			if (vm.IdCodigoProdutoSuframa > 0)
			{
				var codigoProdutoEntity = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Selecionar(o => o.IdProdutoEmpresaExportacao == vm.IdCodigoProdutoSuframa);
				if (codigoProdutoEntity != null)
					vm.CodigoProduto = codigoProdutoEntity.CodigoProduto;
			}
			try
			{
				string descricao = vm.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				short valor = Convert.ToInt16(vm.Descricao);
				vm.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
							.Listar(
									o => (
											vm.Cnpj == null || (o.Cnpj.Equals(vm.Cnpj))
										 )
								   )
							.Where(o =>
										(
											vm.CodigoProduto == null || vm.CodigoProduto == 0 || o.CodigoProduto.Equals(vm.CodigoProduto)
										)
										&&
										(
											vm.Descricao == null || (o.CodigoTipoProduto.ToString().Contains(valor.ToString()))
										)
										&&
										(
											vm.Id == null || (o.IdProdutoEmpresaExportacao.Equals(vm.Id.Value))
										)
									)
							.GroupBy(o => o.CodigoTipoProduto)
							.Select(
										s => new
										{
											id = s.Select(x => x.IdProdutoEmpresaExportacao).FirstOrDefault(),
											text = s.Select(x => x.CodigoTipoProduto).FirstOrDefault().ToString("D4") + " | " + s.Select(x => x.DescricaoTipoProduto).FirstOrDefault()
										}
								   );
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
							.Listar(
									o => (
											vm.Cnpj == null || (o.Cnpj.Equals(vm.Cnpj))
										 )
								   )
							.Where(o =>
										(
											vm.CodigoProduto == null || vm.CodigoProduto == 0 || o.CodigoProduto.Equals(vm.CodigoProduto)
										)
										&&
										(
											vm.Descricao == null || (o.DescricaoTipoProduto.ToLower().Contains(vm.Descricao.ToLower()))
										)
										&&
										(
											vm.Id == null || (o.IdProdutoEmpresaExportacao.Equals(vm.Id.Value))
										)
									)
							.GroupBy(o => o.CodigoTipoProduto)
							.Select(
										s => new
										{
											id = s.Select(x => x.IdProdutoEmpresaExportacao).FirstOrDefault(),
											text = s.Select(x => x.CodigoTipoProduto).FirstOrDefault().ToString("D4") + " | " + s.Select(x => x.DescricaoTipoProduto).FirstOrDefault()
										}
								   );
				return lista;
			}
		}

		public IEnumerable<object> ListarChaveNCM(ViewProdEmpSufVM vm)
		{

			if (vm.Descricao == null && vm.Id == null)
			{
				return new List<object>();
			}

			var usuarioLogado = _usuarioPssBll.ObterUsuarioLogado();
			vm.Cnpj = !PrivateSettings.DEVELOPMENT_MODE ? usuarioLogado.empresaRepresentadaCnpj.CnpjCpfUnformat()
														: usuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();

			if (vm.IdCodigoProdutoSuframa > 0)
			{
				var codigoProdutoEntity = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Selecionar(o => o.IdProdutoEmpresaExportacao == vm.IdCodigoProdutoSuframa);
				if (codigoProdutoEntity != null)
					vm.CodigoProduto = codigoProdutoEntity.CodigoProduto;
			}

			if (vm.IdCodigoTipoProduto > 0)
			{
				var codigoProdutoEntity = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Selecionar(o => o.IdProdutoEmpresaExportacao == vm.IdCodigoTipoProduto);
				if (codigoProdutoEntity != null)
					vm.CodigoTipoProduto = codigoProdutoEntity.CodigoTipoProduto;
			}

			try
			{
				string descricao = vm.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				short valor = Convert.ToInt16(vm.Descricao);
				vm.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
							.Listar(
									o => (
											vm.Cnpj == null || (o.Cnpj.Equals(vm.Cnpj))
										 )
								   )
							.Where(o =>
										(
											vm.CodigoProduto == 0 || o.CodigoProduto.Equals(vm.CodigoProduto)
										)
										&&
										(
											vm.CodigoTipoProduto == 0 || o.CodigoTipoProduto.Equals(vm.CodigoTipoProduto)
										)
										&&
										(
											vm.Descricao == null || (o.CodigoNCM.ToString().Contains(valor.ToString()))
										)
										&&
										(
											vm.Id == null || (o.IdProdutoEmpresaExportacao.Equals(vm.Id.Value))
										)
									)
							.GroupBy(o => o.CodigoNCM)
							.Select(
										s => new
										{
											id = s.Select(x => x.IdProdutoEmpresaExportacao).FirstOrDefault(),
											text = string.Format(s.Select(x => x.CodigoNCM).FirstOrDefault(), "D4") + " | " + s.Select(x => x.DescricaoNCM).FirstOrDefault()
										}
								   );
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao
							.Listar(
									o => (
											vm.Cnpj == null || (o.Cnpj.Equals(vm.Cnpj))
										 )
								   )
							.Where(o =>
										(
											vm.CodigoProduto == 0 || o.CodigoProduto.Equals(vm.CodigoProduto)
										)
										&&
										(
											vm.CodigoTipoProduto == 0 || o.CodigoTipoProduto.Equals(vm.CodigoTipoProduto)
										)
										&&
										(
											vm.Descricao == null || (o.DescricaoNCM.ToLower().Contains(vm.Descricao.ToLower()) || (o.CodigoNCM.ToLower().Contains(vm.Descricao.ToLower())))
										)
										&&
										(
											vm.Id == null || (o.IdProdutoEmpresaExportacao.Equals(vm.Id.Value))
										)
									)
							.GroupBy(o => o.CodigoNCM)
							.Select(
										s => new
										{
											id = s.Select(x => x.IdProdutoEmpresaExportacao).FirstOrDefault(),
											text = string.Format(s.Select(x => x.CodigoNCM).FirstOrDefault(),"D4") + " | " + s.Select(x => x.DescricaoNCM).FirstOrDefault()
										}
								   );
				return lista;
			}
		}

		public IEnumerable<object> ListarChaveUnidadeMedida(ViewProdEmpSufVM vm)
		{

			if (vm.Descricao == null && vm.Id == null)
			{
				return new List<object>();
			}

			if(vm.IdCodigoNCM > 0)
			{
				var codigoNcmEntitiy = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Selecionar(o => o.IdProdutoEmpresaExportacao == vm.IdCodigoNCM);
				if(codigoNcmEntitiy != null)
				{
					vm.IdUnidadeMedida = codigoNcmEntitiy.IdUnidadeMedida;
				}
			}

			var lista = _uowSciex.QueryStackSciex.ViewUnidadeMedida
				.Listar().Where(o =>
									(
										vm.Descricao == null || (o.Descricao.ToLower().Contains(vm.Descricao.ToLower()) || o.Sigla.ToLower().Contains(vm.Descricao.ToLower()))
									)
									&&
									(
										vm.Id == null || o.IdUnidadeMedida == vm.Id
									)
									&&
									(
										vm.IdUnidadeMedida == 0 || o.CodigoUnidadeMedida.Equals(vm.IdUnidadeMedida)
									)
							   )
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.IdUnidadeMedida,
						text = s.Sigla + " | " + s.Descricao
					});



			return lista;
		}

		public IEnumerable<object> ListarChaveInsumoPadrao(ViewMercadoriaVM viewMercadoriaVM)
		{
			if (viewMercadoriaVM.Descricao == null && viewMercadoriaVM.Id == null)
			{
				return new List<object>();
			}

			try
			{
				string descricao = viewMercadoriaVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				int valor = Convert.ToInt32(viewMercadoriaVM.Descricao);
				viewMercadoriaVM.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.ViewMercadoria
					.Listar().Where(o =>
							(viewMercadoriaVM.Descricao == null || (o.Descricao.ToLower().Contains(viewMercadoriaVM.Descricao.ToLower()) || o.CodigoNCMMercadoria.Contains(viewMercadoriaVM.Descricao.ToString())))
						&&
							(viewMercadoriaVM.Id == null || o.IdMercadoria == viewMercadoriaVM.Id)
						&& (viewMercadoriaVM.CodigoProdutoMercadoria == 0 || o.CodigoProdutoMercadoria == viewMercadoriaVM.CodigoProdutoMercadoria)
						&& (viewMercadoriaVM.StatusMercadoria == 0 || o.StatusMercadoria == viewMercadoriaVM.StatusMercadoria)
						)
					.OrderBy(o => o.Descricao)
					.GroupBy(o => o.CodigoNCMMercadoria)
					.Select(
								s => new
								{
									id = s.Select(x => x.IdMercadoria).FirstOrDefault(),
									text = string.Format(s.Select(x => x.CodigoNCMMercadoria).FirstOrDefault(), "D4") + " | " + s.Select(x => x.Descricao).FirstOrDefault()
								}
							).Where(o => o.text.Contains(descricao));
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.ViewMercadoria
					.Listar().Where(o =>
							(viewMercadoriaVM.Descricao == null || (o.Descricao.ToLower().Contains(viewMercadoriaVM.Descricao.ToLower()) || o.CodigoNCMMercadoria.Contains(viewMercadoriaVM.Descricao.ToString())))
						&&
							(viewMercadoriaVM.Id == null || o.IdMercadoria == viewMercadoriaVM.Id)
						&& (viewMercadoriaVM.CodigoProdutoMercadoria == 0 || o.CodigoProdutoMercadoria == viewMercadoriaVM.CodigoProdutoMercadoria)
						&& (viewMercadoriaVM.StatusMercadoria == 0 || o.StatusMercadoria == viewMercadoriaVM.StatusMercadoria)
						)
					.OrderBy(o => o.Descricao)
					.GroupBy(o => o.CodigoNCMMercadoria)
					.Select(
								s => new
								{
									id = s.Select(x => x.IdMercadoria).FirstOrDefault(),
									text = string.Format(s.Select(x => x.CodigoNCMMercadoria).FirstOrDefault(), "D4") + " | " + s.Select(x => x.Descricao).FirstOrDefault()
								}
							);
				return lista;
			}
		}

	}
}