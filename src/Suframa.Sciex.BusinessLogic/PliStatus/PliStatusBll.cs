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
	public class PliStatusBll : IPliStatusBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public PliStatusBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}		

		public IEnumerable<object> ListarPli()
		{
			List<PliStatusVM> lista = new List<PliStatusVM>();
		

			foreach (var item in Enum.GetValues(typeof(EnumPliStatus)))
			{
				PliStatusVM pliStatusVM = new PliStatusVM();

				pliStatusVM.Id = ((int)item);
				pliStatusVM.Text = item.ToString().Replace("_", " ");
				lista.Add(pliStatusVM);
			}

			lista = lista.Where(s => s.Id >= 19 && s.Id < 22)
				.OrderBy(o => o.Text).ToList();

			lista.Insert(0, new PliStatusVM() { Id = 0, Text = "TODOS" } );

			return lista;				
		}

		public IEnumerable<object> ListarConsultarPli()
		{
			List<PliStatusVM> lista = new List<PliStatusVM>();


			foreach (var item in Enum.GetValues(typeof(EnumPliStatus)))
			{
				PliStatusVM pliStatusVM = new PliStatusVM();

				pliStatusVM.Id = ((int)item);
				pliStatusVM.Text = item.ToString().Replace("_", " ");
				lista.Add(pliStatusVM);
			}

			return lista.Where(s => s.Id >= 21 && s.Id <= 25)
				.OrderBy(o => o.Text);

		}

	}
}