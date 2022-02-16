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
	public class ViewSetorBll : IViewSetorBll
	{
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUnitOfWork _uow;
		private readonly Validation _validation;

		public ViewSetorBll(IUnitOfWork uowCadsuf, IUnitOfWork uow)
		{
			_uowCadsuf = uowCadsuf;
			_uow = uow;
			_validation = new Validation();
		}

		public IEnumerable<ViewSetorVM> Listar(ViewSetorVM viewSetorVM)
		{
			var viewSetor = _uowCadsuf.QueryStack.ViewSetor.Listar<ViewSetorVM>();
			return AutoMapper.Mapper.Map<IEnumerable<ViewSetorVM>>(viewSetor);
		}

	

		public IEnumerable<object> ListarViewSetor()
		{
			return _uow.QueryStack.Setor
				.Listar()
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.Codigo,
						text = s.Descricao
					});
		}

		

	}
}