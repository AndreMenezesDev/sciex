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
	public class ViewUnidadeMedidaBll : IViewUnidadeMedidaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;		

		public ViewUnidadeMedidaBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public IEnumerable<object> ListarChave(ViewUnidadeMedidaVM viewUnidadeMedidaVM)
		{

			if (viewUnidadeMedidaVM.Descricao == null && viewUnidadeMedidaVM.Id == null)
			{
				return new List<object>();
			}

			var lista = _uowSciex.QueryStackSciex.ViewUnidadeMedida
				.Listar().Where(o =>
						(viewUnidadeMedidaVM.Descricao == null || (o.Descricao.ToLower().Contains(viewUnidadeMedidaVM.Descricao.ToLower()) || o.Sigla.ToLower().Contains(viewUnidadeMedidaVM.Descricao.ToLower())))
					&&
						(viewUnidadeMedidaVM.Id == null || o.IdUnidadeMedida == viewUnidadeMedidaVM.Id)
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



	}
}