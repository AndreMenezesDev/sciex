using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliStatusRelatorioBll : IPliStatusRelatorioBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public PliStatusRelatorioBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();

		
		}
	
		public PagedItems<PliMercadoriaVM> ListarPaginado(PliMercadoriaVM pagedFilter)
		{
			try
			{
				if (pagedFilter == null) { return new PagedItems<PliMercadoriaVM>(); }


				var pliMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.ListarPaginado<PliMercadoriaVM>(o =>
					(
						(
							pagedFilter.IdPLI == -1 || o.IdPLI == pagedFilter.IdPLI
						) &&
						(
							pagedFilter.IdPliProduto == -1 || o.IdPliProduto == pagedFilter.IdPliProduto
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.CodigoNCMMercadoria) ||
							o.CodigoNCMMercadoria.Contains(pagedFilter.CodigoNCMMercadoria)
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.DescricaoNCMMercadoria) ||
							o.DescricaoNCMMercadoria.Contains(pagedFilter.DescricaoNCMMercadoria)
						)
					),
					pagedFilter);
				return pliMercadoria;
			}
			catch (Exception ex )
			{
		
			}

			return new PagedItems<PliMercadoriaVM>();
			
		}
		
	}
}