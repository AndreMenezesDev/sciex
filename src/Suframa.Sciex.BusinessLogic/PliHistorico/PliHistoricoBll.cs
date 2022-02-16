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
	public class PliHistoricoBll : IPliHistoricoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public PliHistoricoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<PliHistoricoVM> Listar(PliHistoricoVM pliHistoricoVM)
		{
			var pliHistorico = _uowSciex.QueryStackSciex.PliHistorico.Listar<PliHistoricoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<PliHistoricoVM>>(pliHistorico);
		}


		public PagedItems<PliHistoricoVM> ListarPaginado(PliHistoricoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<PliHistoricoVM>(); }
			

			var pliHistorico = _uowSciex.QueryStackSciex.PliHistorico.ListarPaginado<PliHistoricoVM>(o =>
				(
					(
						pagedFilter.IdPliHistorico == -1 || o.IdPliHistorico == pagedFilter.IdPliHistorico
					) 
				),
				pagedFilter);

			return pliHistorico;
		}

		public void RegrasSalvar(PliHistoricoVM pliHistorico)
		{
			if (pliHistorico == null) { return; }

			// Salva PliHistorico
			var pliHistoricoEntity = AutoMapper.Mapper.Map<PliHistoricoEntity>(pliHistorico);

			if (pliHistoricoEntity == null) { return; }

			if (pliHistorico.IdPliHistorico.HasValue)
			{
				pliHistoricoEntity = _uowSciex.QueryStackSciex.PliHistorico.Selecionar(x => x.IdPliHistorico == pliHistorico.IdPliHistorico);

				pliHistoricoEntity = AutoMapper.Mapper.Map(pliHistorico, pliHistoricoEntity);
			}

			_uowSciex.CommandStackSciex.PliHistorico.Salvar(pliHistoricoEntity);
		}

		public void Salvar(PliHistoricoVM pliHistoricoVM)
		{
			RegrasSalvar(pliHistoricoVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public PliHistoricoVM Selecionar(int? idPliHistorico)
		{
			var pliHistoricoVM = new PliHistoricoVM();

			if (!idPliHistorico.HasValue) { return pliHistoricoVM; }

			var pliHistorico = _uowSciex.QueryStackSciex.PliHistorico.Selecionar(x => x.IdPliHistorico == idPliHistorico);

			if (pliHistorico == null){

				_validation._pliHistoricoExcluirValidation.ValidateAndThrow(new PliHistoricoDto
				{
					ExisteRegistro = false
				});
			}

			pliHistoricoVM = AutoMapper.Mapper.Map<PliHistoricoVM>(pliHistorico);

			return pliHistoricoVM;
		}

		public void Deletar(int id)
		{	

			var pliHistorico = _uowSciex.QueryStackSciex.PliHistorico.Selecionar(s => s.IdPliHistorico == id);

			if (pliHistorico != null)
			{

				//_validation._pliHistoricoExisteRelacionamentoValidation.ValidateAndThrow(new PliHistoricoDto
				//{
				//	TotalEncontradoPliHistorico = pliHistorico.PliEntity.Count,

				//});

				//_validation._pliHistoricoExisteRelacionamentoValidation.ValidateAndThrow(new PliHistoricoDto
				//{
				//	TotalEncontradoPliHistorico = pliHistorico.PliStatusEntity.Count,

				//});


			}
			else
			{
				_validation._pliHistoricoExcluirValidation.ValidateAndThrow(new PliHistoricoDto
				{
					ExisteRegistro = false
				});
			}


			if (pliHistorico != null)
			{
				_uowSciex.CommandStackSciex.PliHistorico.Apagar(pliHistorico.IdPliHistorico);
			}

			_uowSciex.CommandStackSciex.Save();

		
		}
	}
}