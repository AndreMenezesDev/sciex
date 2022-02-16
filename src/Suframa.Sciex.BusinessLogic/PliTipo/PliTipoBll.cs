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
	public class PliTipoBll : IPliTipoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public PliTipoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<object> ListarTipoPli()
		{
			List<PliTipoVM> lista = new List<PliTipoVM>();


			foreach (var item in Enum.GetValues(typeof(EnumTipoPli)))
			{
				PliTipoVM pliTipoVM = new PliTipoVM();

				pliTipoVM.Id = ((int)item);
				pliTipoVM.Text = item.ToString(); /*.Replace("_", " ")*/
				lista.Add(pliTipoVM);
			}

			lista.Insert(0, new PliTipoVM() { Id = 0, Text = "TODOS" });

			return lista;
		}

		public IEnumerable<object> ListarTipoPliFixo()
		{
			List<PliTipoVM> lista = new List<PliTipoVM>();


			foreach (var item in Enum.GetValues(typeof(EnumTipoPli)))
			{
				PliTipoVM pliTipoVM = new PliTipoVM();

				pliTipoVM.Id = ((int)item);
				pliTipoVM.Text = item.ToString();
				lista.Add(pliTipoVM);
			}

			return lista;
		}

	}
}