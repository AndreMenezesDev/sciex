using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class ViewProdutoEmpresaBll : IViewProdutoEmpresaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public ViewProdutoEmpresaBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public ViewProdutoEmpresaVM Selecionar(string desc)
		{
			return  _uowSciex.QueryStackSciex.ViewProdutoEmpresa.Selecionar<ViewProdutoEmpresaVM>(o => o.Descricao == desc);
		}

		public IEnumerable<object> ListarChave(ViewProdutoEmpresaVM viewProdutoEmpresaVM)
		{

			if (viewProdutoEmpresaVM.Descricao == null && viewProdutoEmpresaVM.Id == null && viewProdutoEmpresaVM.Cnpj == null)
			{
				return new List<object>();
			}

			try
			{
				string descricao = viewProdutoEmpresaVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				long valor = Convert.ToInt64(viewProdutoEmpresaVM.Descricao);
				viewProdutoEmpresaVM.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.ViewProdutoEmpresa
				.Listar().Where(o =>
						(viewProdutoEmpresaVM.Descricao == null || (o.Descricao.ToLower().Contains(viewProdutoEmpresaVM.Descricao.ToLower()) || (o.CodigoProdutoZFM.Contains(valor.ToString())) ))
					&&
						(viewProdutoEmpresaVM.Cnpj == null || o.Cnpj == viewProdutoEmpresaVM.Cnpj.CnpjCpfUnformat())
					&&
						(viewProdutoEmpresaVM.Id == null || o.IdProdutoEmpresa == viewProdutoEmpresaVM.Id)
					)

				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.IdProdutoEmpresa,
						text = (s.CodigoProduto.ToString("D4") + s.CodigoTipoProduto.ToString("D3") + s.CodigoModeloProduto.ToString("D4")) + " | " + s.Descricao
					}).Where(o => o.text.Contains(descricao));

				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.ViewProdutoEmpresa
				.Listar().Where(o =>
						(viewProdutoEmpresaVM.Descricao == null || (o.Descricao.ToLower().Contains(viewProdutoEmpresaVM.Descricao.ToLower()) || (o.CodigoProduto.ToString() + o.CodigoTipoProduto.ToString() + o.CodigoModeloProduto.ToString()).Contains(viewProdutoEmpresaVM.Descricao.ToString())))
					&&
						(viewProdutoEmpresaVM.Cnpj == null || o.Cnpj == viewProdutoEmpresaVM.Cnpj.CnpjCpfUnformat())
					&&
						(viewProdutoEmpresaVM.CodigoModeloProduto == 0 || o.CodigoModeloProduto == viewProdutoEmpresaVM.CodigoModeloProduto)
					&&
						(viewProdutoEmpresaVM.CodigoTipoProduto == 0 || o.CodigoTipoProduto == viewProdutoEmpresaVM.CodigoTipoProduto)
						&&
						(viewProdutoEmpresaVM.CodigoProduto == 0 || o.CodigoProduto == viewProdutoEmpresaVM.CodigoProduto)
					&&
						(viewProdutoEmpresaVM.Id == null || o.IdProdutoEmpresa == viewProdutoEmpresaVM.Id)
					)

				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.IdProdutoEmpresa,
						DescricaoCRII = s.DescricaoCRII,
						CRII = s.CRII,
						text = (s.CodigoProduto.ToString("D4") + s.CodigoTipoProduto.ToString("D3") + s.CodigoModeloProduto.ToString("D4")) + " | " + s.Descricao
					});

				return lista;
			}
		}

		public PagedItems<ViewProdutoEmpresaVM> ListarPaginado(ViewProdutoEmpresaVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<ViewProdutoEmpresaVM>(); }

			var aladi = _uowSciex.QueryStackSciex.Moeda.ListarPaginado<ViewProdutoEmpresaVM>(o =>
				(
					(
						string.IsNullOrEmpty(pagedFilter.CodigoProdutoMontado.ToString()) ||
						o.CodigoMoeda.Equals(pagedFilter.CodigoProdutoMontado)
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					)
				),
				pagedFilter);

			return aladi;
		}




	}
}