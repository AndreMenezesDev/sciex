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
	public class ViewSetorMercadoriaBll : IViewSetorMercadoriaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uow;
		private readonly Validation _validation;

		public ViewSetorMercadoriaBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uow)
		{
			_uowSciex = uowSciex;
			_uow = uow;
			_validation = new Validation();
		}

	
	}
}