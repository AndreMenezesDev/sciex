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
using System.Net.Http;
using System.Web.UI.WebControls;

namespace Suframa.Sciex.BusinessLogic
{
	public class ViewMercadoriaBll : IViewMercadoriaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uow;
		private readonly Validation _validation;

		public ViewMercadoriaBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uow)
		{
			_uowSciex = uowSciex;
			_uow = uow;
			_validation = new Validation();
		}

		public IEnumerable<object> ListarChave(ViewMercadoriaVM viewMercadoriaVM)
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

		public ViewMercadoriaVM SelecionarNCM(ViewMercadoriaVM viewMercadoriaVM)
		{
			if (viewMercadoriaVM == null)
			{
				return new ViewMercadoriaVM();
			}

			var retorno = _uowSciex.QueryStackSciex.ViewMercadoria.Selecionar<ViewMercadoriaVM>(
					o => (String.IsNullOrEmpty(viewMercadoriaVM.CodigoNCMMercadoria) || o.CodigoNCMMercadoria == viewMercadoriaVM.CodigoNCMMercadoria)
					&& (viewMercadoriaVM.CodigoProdutoMercadoria == 0 || o.CodigoProdutoMercadoria == viewMercadoriaVM.CodigoProdutoMercadoria)
					&& (String.IsNullOrEmpty(viewMercadoriaVM.Descricao) || o.Descricao.Equals(viewMercadoriaVM.Descricao))
					&& o.StatusMercadoria == 1);

			return retorno;
		}

		public PagedItems<ViewMercadoriaVM> ListarPaginado(ViewMercadoriaVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<ViewMercadoriaVM>(); }

			var aladi = _uowSciex.QueryStackSciex.ViewMercadoria.ListarPaginado<ViewMercadoriaVM>(o =>
				(
					(
						string.IsNullOrEmpty(pagedFilter.CodigoNCMMercadoria.ToString()) ||
						o.CodigoNCMMercadoria.Equals(pagedFilter.CodigoNCMMercadoria)
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