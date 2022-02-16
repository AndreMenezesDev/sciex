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
	public class PliProcessoAnuenteBll : IPliProcessoAnuenteBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public PliProcessoAnuenteBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<PliProcessoAnuenteVM> Listar(PliProcessoAnuenteVM pliProcessoAnuenteVM)
		{
			var pliProcessoAnuente = _uowSciex.QueryStackSciex.PliProcessoAnuente.Listar<PliProcessoAnuenteVM>();
			return AutoMapper.Mapper.Map<IEnumerable<PliProcessoAnuenteVM>>(pliProcessoAnuente);
		}

		public IEnumerable<object> Listar()
		{
			return _uowSciex.QueryStackSciex.PliProcessoAnuente
				.Listar()
				.OrderBy(o => o.NumeroProcesso)
				.Select(
					s => new
					{
						id = s.IdPliProcessoAnuente,
						text = s.IdPliProcessoAnuente + " - " + s.NumeroProcesso
					});
		}

		public PagedItems<PliProcessoAnuenteVM> ListarPaginado(PliProcessoAnuenteVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<PliProcessoAnuenteVM>(); }
			

			var pliProcessoAnuente = _uowSciex.QueryStackSciex.PliProcessoAnuente.ListarPaginado<PliProcessoAnuenteVM>(o =>
				(
					(
						pagedFilter.IdPliProcessoAnuente == -1 || o.IdPliProcessoAnuente == pagedFilter.IdPliProcessoAnuente
					) &&
					(
						pagedFilter.IdPliMercadoria == -1 || o.IdPliMercadoria == pagedFilter.IdPliMercadoria
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.NumeroProcesso) || 
						o.NumeroProcesso.Contains(pagedFilter.NumeroProcesso)
					)
				),
				pagedFilter);

			return pliProcessoAnuente;
		}

		public void RegrasSalvar(PliProcessoAnuenteVM pliProcessoAnuente)
		{

			if (pliProcessoAnuente == null) { return; }

			// Salva PliProcessoAnuente
			var pliProcessoAnuenteEntity = AutoMapper.Mapper.Map<PliProcessoAnuenteEntity>(pliProcessoAnuente);

			if (pliProcessoAnuenteEntity == null) { return; }

			if (pliProcessoAnuente.IdPliProcessoAnuente.HasValue)
			{
				pliProcessoAnuenteEntity = _uowSciex.QueryStackSciex.PliProcessoAnuente.Selecionar(x => x.IdPliProcessoAnuente == pliProcessoAnuente.IdPliProcessoAnuente);

				pliProcessoAnuenteEntity = AutoMapper.Mapper.Map(pliProcessoAnuente, pliProcessoAnuenteEntity);
			}

			_uowSciex.CommandStackSciex.PliProcessoAnuente.Salvar(pliProcessoAnuenteEntity);
		}

		public void Salvar(PliProcessoAnuenteVM pliProcessoAnuenteVM)
		{
			RegrasSalvar(pliProcessoAnuenteVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public PliProcessoAnuenteVM Selecionar(int? idPliProcessoAnuente)
		{
			var pliProcessoAnuenteVM = new PliProcessoAnuenteVM();

			if (!idPliProcessoAnuente.HasValue) { return pliProcessoAnuenteVM; }

			var pliProcessoAnuente = _uowSciex.QueryStackSciex.PliProcessoAnuente.Selecionar(x => x.IdPliProcessoAnuente == idPliProcessoAnuente);

			if (pliProcessoAnuente == null){

				_validation._pliProcessoAnuenteExcluirValidation.ValidateAndThrow(new PliProcessoAnuenteDto
				{
					ExisteRegistro = false
				});
			}

			pliProcessoAnuenteVM = AutoMapper.Mapper.Map<PliProcessoAnuenteVM>(pliProcessoAnuente);

			return pliProcessoAnuenteVM;
		}

		public void Deletar(int id)
		{	

			var pliProcessoAnuente = _uowSciex.QueryStackSciex.PliProcessoAnuente.Selecionar(s => s.IdPliProcessoAnuente == id);

			if (pliProcessoAnuente != null)
			{

				//_validation._pliProcessoAnuenteExisteRelacionamentoValidation.ValidateAndThrow(new PliProcessoAnuenteDto
				//{
				//	TotalEncontradoPliProcessoAnuente = pliProcessoAnuente.OrgaoAnuente.Count,

				//});

				//_validation._pliProcessoAnuenteExisteRelacionamentoValidation.ValidateAndThrow(new PliProcessoAnuenteDto
				//{
				//	TotalEncontradoPliProcessoAnuente = pliProcessoAnuente.PLIMercadoria.Count,

				//});


			}
			else
			{
				_validation._pliProcessoAnuenteExcluirValidation.ValidateAndThrow(new PliProcessoAnuenteDto
				{
					ExisteRegistro = false
				});
			}


			if (pliProcessoAnuente != null)
			{
				_uowSciex.CommandStackSciex.PliProcessoAnuente.Apagar(pliProcessoAnuente.IdPliProcessoAnuente);
			}

			_uowSciex.CommandStackSciex.Save();

		
		}
	}
}