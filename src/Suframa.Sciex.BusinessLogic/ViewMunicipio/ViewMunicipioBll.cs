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
	public class ViewMunicipioBll : IViewMunicipioBll
	{
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUnitOfWork _uow;
		private readonly Validation _validation;

		public ViewMunicipioBll(IUnitOfWork uowCadsuf, IUnitOfWork uow)
		{
			_uowCadsuf = uowCadsuf;
			_uow = uow;
			_validation = new Validation();
		}

		public IEnumerable<ViewMunicipioVM> Listar(ViewMunicipioVM viewMunicipioVM)
		{
			var viewMunicipio = _uowCadsuf.QueryStack.ViewMunicipio.Listar()
				.Where(o => o.SiglaUF == null || o.SiglaUF.ToLower().Contains(viewMunicipioVM.SiglaUF.ToLower()));
			return AutoMapper.Mapper.Map<IEnumerable<ViewMunicipioVM>>(viewMunicipio);
		}

	

		public IEnumerable<object> ListarViewMunicipio()
		{
			return _uow.QueryStack.ViewMunicipio
				.Listar()
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.CodigoMunicipio,
						text = s.CodigoMunicipio + " | "+ s.Descricao
					});
		}
	}
}