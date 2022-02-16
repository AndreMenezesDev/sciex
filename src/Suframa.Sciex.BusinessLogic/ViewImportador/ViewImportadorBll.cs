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
	public class ViewImportadorBll : IViewImportadorBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public ViewImportadorBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<ViewImportadorVM> Listar(ViewImportadorVM viewImportadorVM)
		{
			var viewImportador = _uowSciex.QueryStackSciex.ViewImportador.Listar<ViewImportadorVM>();
			return AutoMapper.Mapper.Map<IEnumerable<ViewImportadorVM>>(viewImportador);
		}

		public IEnumerable<object> ListarChave(ViewImportadorVM viewImportadorVM)
		{
			return _uowSciex.QueryStackSciex.ViewImportador
				.Listar()
				.OrderBy(o => o.IdPessoaJuridica)
				.Select(
					s => new
					{
						id = s.IdPessoaJuridica,
						text = s.Cnpj + " - " + s.RazaoSocial
					});
		}

		public PagedItems<ViewImportadorVM> ListarPaginado(ViewImportadorVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<ViewImportadorVM>(); }	
			var viewImportador = _uowSciex.QueryStackSciex.ViewImportador.ListarPaginado<ViewImportadorVM>(o =>
				(
					(
						pagedFilter.Cnpj == null|| o.Cnpj == pagedFilter.Cnpj
					)
				),
				pagedFilter);

			return viewImportador;
		}

		public ViewImportadorVM Selecionar(string cnpj)
		{
			var viewImportadorVM = new ViewImportadorVM();
			if (cnpj == null) { return viewImportadorVM; }
			var viewImportador = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(x => x.Cnpj == cnpj);			
			viewImportadorVM = AutoMapper.Mapper.Map<ViewImportadorVM>(viewImportador);
			return viewImportadorVM;
		}

		public ViewImportadorVM SelecionarInscricao(int inscricao)
		{
			var viewImportadorVM = new ViewImportadorVM();
			var viewImportador = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(x => x.InscricaoCadastral == inscricao);

			viewImportadorVM = AutoMapper.Mapper.Map<ViewImportadorVM>(viewImportador);
			return viewImportadorVM;
		}

	}
}