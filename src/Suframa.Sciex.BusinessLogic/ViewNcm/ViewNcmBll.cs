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
	public class ViewNcmBll : IViewNcmBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uow;
		private readonly Validation _validation;

		public ViewNcmBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uow)
		{
			_uowSciex = uowSciex;
			_uow = uow;
			_validation = new Validation();
		}

	

		public IEnumerable<object> ListarChave(ViewNcmVM viewNcmVM)
		{

			if (string.IsNullOrEmpty(viewNcmVM.Descricao) && viewNcmVM.Id == 0)
			{
				return new List<object>();
			}

			var ncm = _uowSciex.QueryStackSciex.ViewNcm
				.Listar()
				.Where(o =>
						(string.IsNullOrEmpty(viewNcmVM.Descricao) || o.Descricao.ToLower().Contains(viewNcmVM.Descricao.ToLower()) || o.CodigoNCM.ToString().Contains(viewNcmVM.Descricao.ToString()))
						&&
						(viewNcmVM.Id == 0 || o.IdNcm == viewNcmVM.Id)
					   )
				.Select(
					s => new
					{
						id = s.IdNcm,
						text = s.CodigoNCM + " | " + s.Descricao
					});

			return ncm;
		}

		public ViewNcmVM Selecionar(int id)
		{
			var viewNcmVM = new ViewNcmVM();

			if (id == 0) { return viewNcmVM; }

			viewNcmVM = _uowSciex.QueryStackSciex.ViewNcm.Selecionar<ViewNcmVM>(x => x.IdNcm.Equals(id));
			if(viewNcmVM != null && viewNcmVM.IdNcmUnidadeMedida > 0)
			{
				var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.IdUnidadeMedida.Equals(viewNcmVM.IdNcmUnidadeMedida));
				viewNcmVM.DescricaoUnidadeMedida = undMed.Sigla + " | " + undMed.Descricao;
			}

			return viewNcmVM;
		}
	}
}