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
	public class ControleExecucaoServicoBll : IControleExecucaoServicoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public ControleExecucaoServicoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;			
			_validation = new Validation();
		}
		
		
		public IEnumerable<ControleExecucaoServicoVM> Listar(ControleExecucaoServicoVM controleExecucaoServicoVM)
		{
			var ControleExecucaoServico = _uowSciex.QueryStackSciex.ControleExecucaoServico.Listar<ControleExecucaoServicoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<ControleExecucaoServicoVM>>(ControleExecucaoServico);
		}

		public PagedItems<ControleExecucaoServicoVM> ListarPaginado(ControleExecucaoServicoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<ControleExecucaoServicoVM>(); }

			
			var ControleExecucaoServico = _uowSciex.QueryStackSciex.ControleExecucaoServico.ListarPaginado<ControleExecucaoServicoVM>(o =>
				(
					(
						pagedFilter.IdControleExecucaoServico.HasValue ||
						o.IdControleExecucaoServico == pagedFilter.IdControleExecucaoServico
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.NomeCPFCNPJInteressado) ||
						o.NomeCPFCNPJInteressado.Contains(pagedFilter.NomeCPFCNPJInteressado)
					) 
				),
				pagedFilter);

			return ControleExecucaoServico;
		}

		public void RegrasSalvar(ControleExecucaoServicoVM controleExecucaoServico)
		{
			if (controleExecucaoServico == null) { return; }

			// Salva ControleExecucaoServico
			var controleExecucaoServicoEntity = AutoMapper.Mapper.Map<ControleExecucaoServicoEntity>(controleExecucaoServico);

			if (controleExecucaoServicoEntity == null) { return; }

			if (controleExecucaoServico.IdControleExecucaoServico.HasValue)
			{
				controleExecucaoServicoEntity = _uowSciex.QueryStackSciex.ControleExecucaoServico.Selecionar(x => x.IdControleExecucaoServico == controleExecucaoServico.IdControleExecucaoServico);
						
				controleExecucaoServicoEntity = AutoMapper.Mapper.Map(controleExecucaoServico, controleExecucaoServicoEntity);
			}

			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(controleExecucaoServicoEntity);			

		}

		public void Salvar(ControleExecucaoServicoVM controleExecucaoServicoVM)
		{
			RegrasSalvar(controleExecucaoServicoVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public ControleExecucaoServicoVM Selecionar(int? IdControleExecucaoServico)
		{
			var ControleExecucaoServicoVM = new ControleExecucaoServicoVM();

			if (!IdControleExecucaoServico.HasValue) { return ControleExecucaoServicoVM; }

			var ControleExecucaoServico = _uowSciex.QueryStackSciex.ControleExecucaoServico.Selecionar(x => x.IdControleExecucaoServico == IdControleExecucaoServico);

			if (ControleExecucaoServico == null) { return ControleExecucaoServicoVM; }

			ControleExecucaoServicoVM = AutoMapper.Mapper.Map<ControleExecucaoServicoVM>(ControleExecucaoServico);			
	
			return ControleExecucaoServicoVM;
		}


		public void Deletar(int id)
		{
			var controleExecucaoServico = _uowSciex.QueryStackSciex.ControleExecucaoServico.Selecionar(s => s.IdListaServico == id);

			if (controleExecucaoServico != null)
			{
				//_validation._unidadeCadastradoraDeletarValidation.ValidateAndThrow(new ManterUnidadeCadastradoraDto
				//{
				//	TotalEncontradoRequerimento = unidadeCadastradora.Requerimento.Count
				//});
			}
			
			if (controleExecucaoServico != null)
			{
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Apagar(controleExecucaoServico.IdControleExecucaoServico);
			}

			_uowSciex.CommandStackSciex.Save();
		}

	}
}