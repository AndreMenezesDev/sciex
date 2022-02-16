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
	public class OrgaoAnuenteBll : IOrgaoAnuenteBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public OrgaoAnuenteBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;						
		}


		public IEnumerable<object> ListarChave(OrgaoAnuenteVM orgaoAnuenteVM)
		{

			if (orgaoAnuenteVM.Descricao == null && orgaoAnuenteVM.Id == null)
			{
				return new List<object>();
			}

			var lista = _uowSciex.QueryStackSciex.OrgaoAnuente
				.Listar().Where(o =>
						(orgaoAnuenteVM.Descricao == null || (o.Descricao.ToLower().Contains(orgaoAnuenteVM.Descricao.ToLower()) || o.IdOrgaoAnuente.ToString().Contains(orgaoAnuenteVM.Descricao.ToString())))
					&&
						(orgaoAnuenteVM.Id == null || o.IdOrgaoAnuente == orgaoAnuenteVM.Id)
					)
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.IdOrgaoAnuente,
						text = s.IdOrgaoAnuente + " | " + s.Descricao
					});



			return lista;
		}


	}
}