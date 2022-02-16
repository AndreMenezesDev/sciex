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
	public class ViewInsumoPadraoBll : IViewInsumoPadraoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uow;
		private readonly Validation _validation;

		public ViewInsumoPadraoBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uow)
		{
			_uowSciex = uowSciex;
			_uow = uow;
			_validation = new Validation();
		}

		public IEnumerable<object> ListarChave(ViewInsumoPadraoVM insumoPadraoVM)
		{
			if (insumoPadraoVM.Descricao == null && insumoPadraoVM.Id == null)
			{
				return new List<object>();
			}

			try
			{
				string descricao = insumoPadraoVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				int valor = Convert.ToInt32(insumoPadraoVM.Descricao);
				insumoPadraoVM.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.ViewInsumoPadrao
					.Listar()
					.Where(o =>
							(
								insumoPadraoVM.CodigoNCMMercadoria == null || o.CodigoNCMMercadoria == insumoPadraoVM.CodigoNCMMercadoria
							)
							&&
							(
								insumoPadraoVM.CodigoProduto == 0 || o.CodigoProduto == insumoPadraoVM.CodigoProduto
							) 
							&&
							(
								insumoPadraoVM.CodigoDetalheMercadoria == 0 || o.CodigoDetalheMercadoria.ToString().Contains(insumoPadraoVM.CodigoDetalheMercadoria.ToString())
							)
						  )
					.OrderBy(o => o.DescricaoDetalheMercadoria)
					.GroupBy(o => o.CodigoDetalheMercadoria)
					.Select(
								s => new
								{
									id = s.Select(x => x.IdInsumoPadrao).FirstOrDefault(),
									text = string.Format(s.Select(x => x.CodigoDetalheMercadoria).FirstOrDefault().ToString(), "D4") + " | " + s.Select(x => x.DescricaoDetalheMercadoria).FirstOrDefault()
								}
							).Where(o => o.text.Contains(descricao));
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.ViewInsumoPadrao
					.Listar().Where(o =>
									(
										insumoPadraoVM.CodigoNCMMercadoria == null || o.CodigoNCMMercadoria == insumoPadraoVM.CodigoNCMMercadoria
									)
									&&
									(
										insumoPadraoVM.Descricao == null || (o.DescricaoDetalheMercadoria.ToLower().Contains(insumoPadraoVM.Descricao.ToLower()) || o.CodigoDetalheMercadoria.ToString().Contains(insumoPadraoVM.Descricao.ToString()))
									)
									&&
									(
										insumoPadraoVM.Id == null || o.IdInsumoPadrao == insumoPadraoVM.Id
									)
									&&
									(
										insumoPadraoVM.CodigoProduto == 0 || o.CodigoProduto == insumoPadraoVM.CodigoProduto
									)
									&&
									(
										insumoPadraoVM.CodigoDetalheMercadoria == 0 || o.CodigoDetalheMercadoria == insumoPadraoVM.CodigoDetalheMercadoria
									)
								   )
					.OrderBy(o => o.DescricaoDetalheMercadoria)
					.GroupBy(o => o.CodigoDetalheMercadoria)
					.Select(
								s => new
								{
									id = s.Select(x => x.IdInsumoPadrao).FirstOrDefault(),
									text = string.Format(s.Select(x => x.CodigoDetalheMercadoria).FirstOrDefault().ToString(), "D4") + " | " + s.Select(x => x.DescricaoDetalheMercadoria).FirstOrDefault()
								}
							);
				return lista;
			}
		}
		public IEnumerable<object> ListarChaveParaNCM(ViewInsumoPadraoDropDown insumoPadraoVM)
		{
			if (insumoPadraoVM.Descricao == null && insumoPadraoVM.Id == null)
			{
				return new List<object>();
			}

			try
			{
				string descricao = insumoPadraoVM.Descricao;
				if (descricao == null)
				{
					throw new ArgumentException("Descricao nula");
				}
				else
				{
					insumoPadraoVM.ValorCodigoDetalheMercadoria = 0;
					insumoPadraoVM.ValorCodigoNCM = null;
					insumoPadraoVM.ValorCodigoProdutoSuframa = 0;
				}

				var lista = _uowSciex.QueryStackSciex.ViewInsumoPadrao
					.Listar()
					.Where(o =>
							(
								string.IsNullOrEmpty(insumoPadraoVM.ValorCodigoNCM) || o.CodigoNCMMercadoria == insumoPadraoVM.ValorCodigoNCM
							)
							&&
							(
								insumoPadraoVM.ValorCodigoProdutoSuframa == 0 || o.CodigoProduto == insumoPadraoVM.ValorCodigoProdutoSuframa
							)
							&&
							(
								insumoPadraoVM.ValorCodigoDetalheMercadoria == 0 || o.CodigoDetalheMercadoria == insumoPadraoVM.ValorCodigoDetalheMercadoria
							)
							&&
							(
								o.DescricaoDetalheMercadoria.ToLower().Contains(insumoPadraoVM.Descricao.ToLower())
								||
								o.CodigoNCMMercadoria.ToString().Contains(insumoPadraoVM.Descricao.ToString())
							)
						  )
					.OrderBy(o => o.DescricaoDetalheMercadoria)
					.GroupBy(o => o.CodigoNCMMercadoria)
					.Select(
								s => new
								{
									id = s.Select(x => x.IdInsumoPadrao).FirstOrDefault(),
									text = string.Format(s.Select(x => x.CodigoNCMMercadoria).FirstOrDefault().ToString(), "D4") + " | " + s.Select(x => x.DescricaoDetalheMercadoria).FirstOrDefault()
								}
							);
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.ViewInsumoPadrao
					.Listar().Where(o =>
									(
										string.IsNullOrEmpty(insumoPadraoVM.ValorCodigoNCM) || o.CodigoNCMMercadoria == insumoPadraoVM.ValorCodigoNCM
									)
									&&
									(
										insumoPadraoVM.ValorCodigoProdutoSuframa == 0 || o.CodigoProduto == insumoPadraoVM.ValorCodigoProdutoSuframa
									)
									&&
									(
										insumoPadraoVM.ValorCodigoDetalheMercadoria == 0 || o.CodigoDetalheMercadoria == insumoPadraoVM.ValorCodigoDetalheMercadoria
									)
								   )
					.OrderBy(o => o.DescricaoDetalheMercadoria)
					.GroupBy(o => o.CodigoNCMMercadoria)
					.Select(
								s => new
								{
									id = s.Select(x => x.IdInsumoPadrao).FirstOrDefault(),
									text = string.Format(s.Select(x => x.CodigoNCMMercadoria).FirstOrDefault().ToString(), "D4") + " | " + s.Select(x => x.DescricaoDetalheMercadoria).FirstOrDefault()
								}
							);
				return lista;
			}
		}

		public ViewInsumoPadraoVM SelecionarNCM(ViewInsumoPadraoVM insumoPadraoVM)
		{
			if (insumoPadraoVM == null)
			{
				return new ViewInsumoPadraoVM();
			}

			var retorno = _uowSciex.QueryStackSciex.ViewInsumoPadrao.Selecionar<ViewInsumoPadraoVM>(
					o => (String.IsNullOrEmpty(insumoPadraoVM.CodigoNCMMercadoria) || o.CodigoNCMMercadoria == insumoPadraoVM.CodigoNCMMercadoria)
					&& (insumoPadraoVM.CodigoProduto == 0 || o.CodigoProduto == insumoPadraoVM.CodigoProduto));
					//&& (String.IsNullOrEmpty(insumoPadraoVM.Descricao) || o..Equals(insumoPadraoVM.Descricao))
					//&& o.StatusMercadoria == 1);

			return retorno;
		}

		public PagedItems<ViewInsumoPadraoVM> ListarPaginado(ViewInsumoPadraoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<ViewInsumoPadraoVM>(); }

			var aladi = _uowSciex.QueryStackSciex.ViewInsumoPadrao.ListarPaginado<ViewInsumoPadraoVM>(o =>
				(
					(
						string.IsNullOrEmpty(pagedFilter.CodigoNCMMercadoria.ToString()) ||
						o.CodigoNCMMercadoria.Equals(pagedFilter.CodigoNCMMercadoria)
					) /*&&*/
					//(
					//	string.IsNullOrEmpty(pagedFilter.Descricao) ||
					//	o.Descricao.Contains(pagedFilter.Descricao)
					//)
				),
				pagedFilter);

			return aladi;
		}


	}
}