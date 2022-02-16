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
	public class TipoAcordoTarifarioBll : ITipoAcordoTarifarioBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public TipoAcordoTarifarioBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<object> Listar()
		{
			List<TipoAcordoTarifarioVM> lista = new List<TipoAcordoTarifarioVM>();


			foreach (var item in Enum.GetValues(typeof(EnumTipoAcordoTarifario)))
			{
				TipoAcordoTarifarioVM tipoAcordoTarifarioVM = new TipoAcordoTarifarioVM();

				tipoAcordoTarifarioVM.Id = ((int)item);
				tipoAcordoTarifarioVM.Text = item.ToString().Replace("_", " ");
				lista.Add(tipoAcordoTarifarioVM);
			}

			return lista;

		}

	}
}