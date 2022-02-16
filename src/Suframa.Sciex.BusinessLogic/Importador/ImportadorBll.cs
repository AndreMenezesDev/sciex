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
	public class ImportadorBll : IImportadorBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public ImportadorBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<ImportadorVM> Listar(ImportadorVM importadorVM)
		{
			var importador = _uowSciex.QueryStackSciex.Importador.Listar<ImportadorVM>();
			return AutoMapper.Mapper.Map<IEnumerable<ImportadorVM>>(importador);
		}
	

		public PagedItems<ImportadorVM> ListarPaginado(ImportadorVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<ImportadorVM>(); }

			var importador = _uowSciex.QueryStackSciex.Importador.ListarPaginado<ImportadorVM>(o =>
				(
					(
						string.IsNullOrEmpty(pagedFilter.IdImportador.ToString()) ||
						o.IdImportador == pagedFilter.IdImportador
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.CNPJ) ||
						o.CNPJ.Contains(pagedFilter.CNPJ)
					)
				),
				pagedFilter);

			return importador;
		}

		public void RegrasSalvar(ImportadorVM importador)
		{
			if (importador == null) { return; }

			// Salva Aladi
			var importadorEntity = AutoMapper.Mapper.Map<ImportadorEntity>(importador);

			if (importadorEntity == null) { return; }

			if (importador.IdImportador.HasValue)
			{
				importadorEntity = _uowSciex.QueryStackSciex.Importador.Selecionar(x => x.IdImportador == importador.IdImportador);

				importadorEntity = AutoMapper.Mapper.Map(importador, importadorEntity);
			}

			_uowSciex.CommandStackSciex.Importador.Salvar(importadorEntity);
		}

		public void Salvar(ImportadorVM importadorVM)
		{
			RegrasSalvar(importadorVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public ImportadorVM Selecionar(int? idImportador)
		{
			var importadorVM = new ImportadorVM();

			if (!idImportador.HasValue) { return importadorVM; }

			var importador = _uowSciex.QueryStackSciex.Importador.Selecionar(x => x.IdImportador == idImportador);

			if (importador == null) { return importadorVM; }

			importadorVM = AutoMapper.Mapper.Map<ImportadorVM>(importador);

			return importadorVM;
		}

		public void Deletar(int id)
		{
			var importador = _uowSciex.QueryStackSciex.Importador.Selecionar(s => s.IdImportador == id);

			if (importador != null)
			{
				//_validation._unidadeCadastradoraDeletarValidation.ValidateAndThrow(new ManterUnidadeCadastradoraDto
				//{
				//	TotalEncontradoRequerimento = unidadeCadastradora.Requerimento.Count
				//});
			}

			if (importador != null)
			{
				_uowSciex.CommandStackSciex.Importador.Apagar(importador.IdImportador);
			}

			_uowSciex.CommandStackSciex.Save();
		}

		public IEnumerable<object> Listar()
		{
			throw new NotImplementedException();
		}
	}
}