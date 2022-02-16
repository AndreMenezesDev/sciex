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
	public class TipoCoberturaCambialBll : ITipoCoberturaCambialBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public TipoCoberturaCambialBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<object> Listar()
		{
			List<TipoCoberturaCambialVM> lista = new List<TipoCoberturaCambialVM>();


			foreach (var item in Enum.GetValues(typeof(EnumTipoCoberturaCambial)))
			{
				TipoCoberturaCambialVM tipoCoberturaCambialVM = new TipoCoberturaCambialVM();

				tipoCoberturaCambialVM.Id = ((int)item);
				tipoCoberturaCambialVM.Text = item.ToString().Replace("_", " ");
				lista.Add(tipoCoberturaCambialVM);
			}

			return lista;

		}

	}
}