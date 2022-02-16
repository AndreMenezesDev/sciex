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
	public class InstituicaoFinanceiraBll : IInstituicaoFinanceiraBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public InstituicaoFinanceiraBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<object> ListarChave(InstituicaoFinanceiraVM instituicaoFinanceiraVM)
		{
				if (instituicaoFinanceiraVM.Descricao == null && instituicaoFinanceiraVM.Id == null)
				{
					return new List<object>();
				}
				try
				{
					string descricao = instituicaoFinanceiraVM.Descricao;
					if (descricao == null) throw new ArgumentException("Descricao nula");

					int valor = Convert.ToInt32(instituicaoFinanceiraVM.Descricao);
					instituicaoFinanceiraVM.Descricao = valor.ToString();

					var lista = _uowSciex.QueryStackSciex.InstituicaoFinanceira
						.Listar().Where(o =>
								(instituicaoFinanceiraVM.Descricao == null || (o.Descricao.ToLower().Contains(instituicaoFinanceiraVM.Descricao.ToLower())) || o.Codigo.ToString().Contains(instituicaoFinanceiraVM.Descricao.ToLower()))
							)
						.OrderBy(o => o.Descricao)
						.Select(
							s => new
							{
								id = s.IdInstituicaoFinanceira,
								text = s.Codigo.ToString("D2") + " | " + s.Descricao
							}).Where(o => o.text.Contains(descricao))
							;
					return lista;
				}
				catch
				{
					var lista = _uowSciex.QueryStackSciex.InstituicaoFinanceira
						.Listar().Where(o =>
								(instituicaoFinanceiraVM.Descricao == null || (o.Descricao.ToLower().Contains(instituicaoFinanceiraVM.Descricao.ToLower())))
							&&
								(instituicaoFinanceiraVM.Id == null || o.IdInstituicaoFinanceira == instituicaoFinanceiraVM.Id)
							)
						.OrderBy(o => o.Descricao)
						.Select(
							s => new
							{
								id = s.IdInstituicaoFinanceira,
								text = s.Codigo.ToString("D2") + " | " + s.Descricao
							});
					return lista;
				}

			}
		



		public IEnumerable<InstituicaoFinanceiraVM> Listar(InstituicaoFinanceiraVM instituicaoFinanceiraVM)
		{
			var instituicaoFinanceira = _uowSciex.QueryStackSciex.InstituicaoFinanceira.Listar<InstituicaoFinanceiraVM>();
			return AutoMapper.Mapper.Map<IEnumerable<InstituicaoFinanceiraVM>>(instituicaoFinanceira);
		}

		public PagedItems<InstituicaoFinanceiraVM> ListarPaginado(InstituicaoFinanceiraVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<InstituicaoFinanceiraVM>(); }

			var instituicaoFinanceira = _uowSciex.QueryStackSciex.InstituicaoFinanceira.ListarPaginado<InstituicaoFinanceiraVM>(o =>
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

			return instituicaoFinanceira;
		}

		public void RegrasSalvar(InstituicaoFinanceiraVM instituicaoFinanceira)
		{
			if (instituicaoFinanceira == null) { return; }

			// Salva InstituicaoFinanceira
			var instituicaoFinanceiraEntity = AutoMapper.Mapper.Map<InstituicaoFinanceiraEntity>(instituicaoFinanceira);

			if (instituicaoFinanceiraEntity == null) { return; }

			if (instituicaoFinanceira.IdInstituicaoFinanceira.HasValue)
			{
				instituicaoFinanceiraEntity = _uowSciex.QueryStackSciex.InstituicaoFinanceira.Selecionar(x => x.IdInstituicaoFinanceira == instituicaoFinanceira.IdInstituicaoFinanceira);

				instituicaoFinanceiraEntity = AutoMapper.Mapper.Map(instituicaoFinanceira, instituicaoFinanceiraEntity);
			}

			_uowSciex.CommandStackSciex.InstituicaoFinanceira.Salvar(instituicaoFinanceiraEntity);
		}

		public void Salvar(InstituicaoFinanceiraVM instituicaoFinanceiraVM)
		{
			RegrasSalvar(instituicaoFinanceiraVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public InstituicaoFinanceiraVM Selecionar(int? idInstituicaoFinanceira)
		{
			var instituicaoFinanceiraVM = new InstituicaoFinanceiraVM();

			if (!idInstituicaoFinanceira.HasValue) { return instituicaoFinanceiraVM; }

			var instituicaoFinanceira = _uowSciex.QueryStackSciex.InstituicaoFinanceira.Selecionar(x => x.IdInstituicaoFinanceira == idInstituicaoFinanceira);

			if (instituicaoFinanceira == null) { return instituicaoFinanceiraVM; }

			instituicaoFinanceiraVM = AutoMapper.Mapper.Map<InstituicaoFinanceiraVM>(instituicaoFinanceira);

			return instituicaoFinanceiraVM;
		}

		public void Deletar(int id)
		{
			var instituicaoFinanceira = _uowSciex.QueryStackSciex.InstituicaoFinanceira.Selecionar(s => s.IdInstituicaoFinanceira == id);

			if (instituicaoFinanceira != null)
			{
				//_validation._unidadeCadastradoraDeletarValidation.ValidateAndThrow(new ManterUnidadeCadastradoraDto
				//{
				//	TotalEncontradoRequerimento = unidadeCadastradora.Requerimento.Count
				//});
			}

			if (instituicaoFinanceira != null)
			{
				_uowSciex.CommandStackSciex.InstituicaoFinanceira.Apagar(instituicaoFinanceira.IdInstituicaoFinanceira);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}