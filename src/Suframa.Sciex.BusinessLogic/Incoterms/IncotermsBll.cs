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

namespace Suframa.Sciex.BusinessLogic
{
	public class IncotermsBll : IIncotermsBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public IncotermsBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<object> ListarChave(IncotermsVM incotermsVM)
		{

			if (incotermsVM.Descricao == null && incotermsVM.Id == null)
			{
				return new List<object>();
			}

			var Incoterms =  _uowSciex.QueryStackSciex.Incoterms
				.Listar().Where(o =>
						(incotermsVM.Descricao == null || (o.Descricao.ToLower().Contains(incotermsVM.Descricao.ToLower()) || o.Codigo.ToLower().Contains(incotermsVM.Descricao.ToLower())))
					&&
						(incotermsVM.Id == null || o.IdIncoterms == incotermsVM.Id)
					)
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.IdIncoterms,
						text = ("00" + s.Codigo.ToString()).Slice(3) + " | " + s.Descricao
					});
										
			return Incoterms;
		}

		public IEnumerable<IncotermsVM> Listar(IncotermsVM incotermsVM)
		{
			var incoterms = _uowSciex.QueryStackSciex.Incoterms.Listar<IncotermsVM>();
			return AutoMapper.Mapper.Map<IEnumerable<IncotermsVM>>(incoterms);
		}

		public PagedItems<IncotermsVM> ListarPaginado(IncotermsVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<IncotermsVM>(); }

			var incoterms = _uowSciex.QueryStackSciex.Incoterms.ListarPaginado<IncotermsVM>(o =>
				(
					(
						string.IsNullOrEmpty(pagedFilter.Codigo) ||
						o.Codigo.Equals(pagedFilter.Codigo)
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					)
				),
				pagedFilter);

			return incoterms;
		}

		public void RegrasSalvar(IncotermsVM incoterms)
		{
			if (incoterms == null) { return; }

			// Salva Incoterms
			var incotermsEntity = AutoMapper.Mapper.Map<IncotermsEntity>(incoterms);

			if (incotermsEntity == null) { return; }

			if (incoterms.IdIncoterms.HasValue)
			{
				incotermsEntity = _uowSciex.QueryStackSciex.Incoterms.Selecionar(x => x.IdIncoterms == incoterms.IdIncoterms);

				incotermsEntity = AutoMapper.Mapper.Map(incoterms, incotermsEntity);
			}

			_uowSciex.CommandStackSciex.Incoterms.Salvar(incotermsEntity);
		}

		public void Salvar(IncotermsVM incotermsVM)
		{
			RegrasSalvar(incotermsVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public IncotermsVM Selecionar(int? idIncoterms)
		{
			var incotermsVM = new IncotermsVM();

			if (!idIncoterms.HasValue) { return incotermsVM; }

			var incoterms = _uowSciex.QueryStackSciex.Incoterms.Selecionar(x => x.IdIncoterms == idIncoterms);

			if (incoterms == null) { return incotermsVM; }

			incotermsVM = AutoMapper.Mapper.Map<IncotermsVM>(incoterms);

			return incotermsVM;
		}

		public void Deletar(int id)
		{
			var incoterms = _uowSciex.QueryStackSciex.Incoterms.Selecionar(s => s.IdIncoterms == id);

			if (incoterms != null)
			{
				//_validation._unidadeCadastradoraDeletarValidation.ValidateAndThrow(new ManterUnidadeCadastradoraDto
				//{
				//	TotalEncontradoRequerimento = unidadeCadastradora.Requerimento.Count
				//});
			}

			if (incoterms != null)
			{
				_uowSciex.CommandStackSciex.Incoterms.Apagar(incoterms.IdIncoterms);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}