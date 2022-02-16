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

namespace Suframa.Sciex.BusinessLogic
{
	public class ViewDetalheMercadoriaBll : IViewDetalheMercadoriaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public ViewDetalheMercadoriaBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}


	public IEnumerable<object> ListarChave(ViewDetalheMercadoriaVM viewDetalheMercadoriaVM)
	{
			if (viewDetalheMercadoriaVM.Descricao == null && viewDetalheMercadoriaVM.Id == null && viewDetalheMercadoriaVM.CodigoDetalheMercadoria == 0)
			{
				return new List<object>();
			}
			try
			{

				string descricao = viewDetalheMercadoriaVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				int valor = Convert.ToInt32(viewDetalheMercadoriaVM.Descricao);
				viewDetalheMercadoriaVM.CodigoDetalheMercadoria = valor;



				var lista = _uowSciex.QueryStackSciex.ViewDetalheMercadoria
				.Listar().Where(o =>
						(
							viewDetalheMercadoriaVM.CodigoNCMMercadoria == null || o.CodigoNCMMercadoria == viewDetalheMercadoriaVM.CodigoNCMMercadoria
						)
						&&
						(
							viewDetalheMercadoriaVM.CodigoProduto == 0 || o.CodigoProduto == viewDetalheMercadoriaVM.CodigoProduto
						) &&
						(
							viewDetalheMercadoriaVM.CodigoDetalheMercadoria == 0 || o.CodigoDetalheMercadoria.ToString().Contains(viewDetalheMercadoriaVM.CodigoDetalheMercadoria.ToString())
						)
						&&
						(
							viewDetalheMercadoriaVM.StatusDetalheMercadoria == 0 || o.StatusDetalheMercadoria == viewDetalheMercadoriaVM.StatusDetalheMercadoria
						)
					)
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.IdDetalheMercadoria,
						text = s.CodigoDetalheMercadoria.ToString("D4") + " | " + s.Descricao
					}).Where(o=>o.text.Contains(descricao));

				return lista;
			}
			catch (Exception ex)
			{
				var lista = _uowSciex.QueryStackSciex.ViewDetalheMercadoria
				.Listar().Where(o =>
						(
							viewDetalheMercadoriaVM.CodigoNCMMercadoria == null || o.CodigoNCMMercadoria == viewDetalheMercadoriaVM.CodigoNCMMercadoria
						)
						&&
						(
							viewDetalheMercadoriaVM.Descricao == null || (o.Descricao.ToLower().Contains(viewDetalheMercadoriaVM.Descricao.ToLower()) || o.CodigoDetalheMercadoria.ToString().Contains(viewDetalheMercadoriaVM.Descricao.ToString()))
						)
						&&
						(
							viewDetalheMercadoriaVM.Id == null || o.IdDetalheMercadoria == viewDetalheMercadoriaVM.Id
						)
						&&
						(
							viewDetalheMercadoriaVM.CodigoProduto == 0 || o.CodigoProduto == viewDetalheMercadoriaVM.CodigoProduto
						)
						&&
						(
							viewDetalheMercadoriaVM.CodigoDetalheMercadoria == 0 || o.CodigoDetalheMercadoria == viewDetalheMercadoriaVM.CodigoDetalheMercadoria
						)
						&&
						(
							viewDetalheMercadoriaVM.StatusDetalheMercadoria == 0 || o.StatusDetalheMercadoria == viewDetalheMercadoriaVM.StatusDetalheMercadoria
						)
					)
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.IdDetalheMercadoria,
						text = s.CodigoDetalheMercadoria.ToString("D4") + " | " + s.Descricao
					});

				return lista;

			}
		
			//return new List<object>();
		}

		public PagedItems<ViewDetalheMercadoriaVM> ListagemPadrao(ViewDetalheMercadoriaVM viewDetalheMercadoriaVM)
		{
			var ret = _uowSciex.QueryStackSciex.ViewDetalheMercadoria.ListarPaginado<ViewDetalheMercadoriaVM>(o => 
																												(
																													String.IsNullOrEmpty(viewDetalheMercadoriaVM.CodigoNCMMercadoria)
																													||
																													(o.CodigoNCMMercadoria.Equals(viewDetalheMercadoriaVM.CodigoNCMMercadoria))
																												)
																												&&
																												(
																													String.IsNullOrEmpty(viewDetalheMercadoriaVM.Descricao)
																													|| 
																													(o.Descricao.ToLower().Contains(viewDetalheMercadoriaVM.Descricao.ToLower()))
																												)
																												, viewDetalheMercadoriaVM);

			return ret;
		}
	}
}