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
	public class ListaServicoBll : IListaServicoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public ListaServicoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;			
			_validation = new Validation();
		}
		
		
		public IEnumerable<ListaServicoVM> Listar(ListaServicoVM listaServicoVM)
		{
			var listaServico = _uowSciex.QueryStackSciex.ListaServico.Listar<ListaServicoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<ListaServicoVM>>(listaServico);
		}

		public PagedItems<ListaServicoVM> ListarPaginado(ListaServicoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<ListaServicoVM>(); }

			
			var ListaServico = _uowSciex.QueryStackSciex.ListaServico.ListarPaginado<ListaServicoVM>(o =>
				(
					(
						pagedFilter.IdListaServico.HasValue ||
						o.IdListaServico == pagedFilter.IdListaServico
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					) 
				),
				pagedFilter);

			return ListaServico;
		}

		public void RegrasSalvar(ListaServicoVM listaServico)
		{
			if (listaServico == null) { return; }

			// Salva ListaServico
			var listaServicoEntity = AutoMapper.Mapper.Map<ListaServicoEntity>(listaServico);

			if (listaServicoEntity == null) { return; }

			if (listaServico.IdListaServico.HasValue)
			{
				listaServicoEntity = _uowSciex.QueryStackSciex.ListaServico.Selecionar(x => x.IdListaServico == listaServico.IdListaServico);
						
				listaServicoEntity = AutoMapper.Mapper.Map(listaServico, listaServicoEntity);
			}

			_uowSciex.CommandStackSciex.ListaServico.Salvar(listaServicoEntity);			

		}

		public void Salvar(ListaServicoVM listaServicoVM)
		{
			RegrasSalvar(listaServicoVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public ListaServicoVM Selecionar(int? IdListaServico)
		{
			var ListaServicoVM = new ListaServicoVM();

			if (!IdListaServico.HasValue) { return ListaServicoVM; }

			var ListaServico = _uowSciex.QueryStackSciex.ListaServico.Selecionar(x => x.IdListaServico == IdListaServico);

			if (ListaServico == null) { return ListaServicoVM; }

			ListaServicoVM = AutoMapper.Mapper.Map<ListaServicoVM>(ListaServico);			
	
			return ListaServicoVM;
		}


		public void Deletar(int id)
		{
			var ListaServico = _uowSciex.QueryStackSciex.ListaServico.Selecionar(s => s.IdListaServico == id);

			if (ListaServico != null)
			{
				//_validation._unidadeCadastradoraDeletarValidation.ValidateAndThrow(new ManterUnidadeCadastradoraDto
				//{
				//	TotalEncontradoRequerimento = unidadeCadastradora.Requerimento.Count
				//});
			}
			
			if (ListaServico != null)
			{
				_uowSciex.CommandStackSciex.ListaServico.Apagar(ListaServico.IdListaServico);
			}

			_uowSciex.CommandStackSciex.Save();
		}

	}
}