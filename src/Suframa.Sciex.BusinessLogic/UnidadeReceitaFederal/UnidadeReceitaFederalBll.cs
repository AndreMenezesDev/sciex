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
	public class UnidadeReceitaFederalBll : IUnidadeReceitaFederalBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public UnidadeReceitaFederalBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<UnidadeReceitaFederalVM> Listar(UnidadeReceitaFederalVM unidadeReceitaFederalVM)
		{
			var unidadeReceitaFederal = _uowSciex.QueryStackSciex.UnidadeReceitaFederal.Listar<UnidadeReceitaFederalVM>();
			return AutoMapper.Mapper.Map<IEnumerable<UnidadeReceitaFederalVM>>(unidadeReceitaFederal);
		}


		public IEnumerable<object> ListarChave(UnidadeReceitaFederalVM unidadeReceitaFederalVM)
		{
				if (unidadeReceitaFederalVM.Descricao == null && unidadeReceitaFederalVM.Id == null)
				{
					return new List<object>();
				}
				try
				{
					string descricao = unidadeReceitaFederalVM.Descricao;
					if (descricao == null) throw new ArgumentException("Descricao nula");

					int valor = Convert.ToInt32(unidadeReceitaFederalVM.Descricao);
					unidadeReceitaFederalVM.Descricao = valor.ToString();

					var lista = _uowSciex.QueryStackSciex.UnidadeReceitaFederal
						.Listar().Where(o =>
								(unidadeReceitaFederalVM.Descricao == null || (o.Descricao.ToLower().Contains(unidadeReceitaFederalVM.Descricao.ToLower())) || o.Codigo.ToString().Contains(unidadeReceitaFederalVM.Descricao.ToLower()))
							)
						.OrderBy(o => o.Descricao)
						.Select(
							s => new
							{
								id = s.IdUnidadeReceitaFederal,
								text = s.Codigo.ToString("D7") + " | " + s.Descricao
							}).Where(o => o.text.Contains(descricao))
							;
					return lista;
				}
				catch
				{
					var lista = _uowSciex.QueryStackSciex.UnidadeReceitaFederal
						.Listar().Where(o =>
								(unidadeReceitaFederalVM.Descricao == null || (o.Descricao.ToLower().Contains(unidadeReceitaFederalVM.Descricao.ToLower())))
							&&
								(unidadeReceitaFederalVM.Id == null || o.IdUnidadeReceitaFederal == unidadeReceitaFederalVM.Id)
							)
						.OrderBy(o => o.Descricao)
						.Select(
							s => new
							{
								id = s.IdUnidadeReceitaFederal,
								text = s.Codigo.ToString("D7") + " | " + s.Descricao
							});
					return lista;
				}

			}

		public PagedItems<UnidadeReceitaFederalVM> ListarPaginado(UnidadeReceitaFederalVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<UnidadeReceitaFederalVM>(); }
			var unidadeReceitaFederal = _uowSciex.QueryStackSciex.UnidadeReceitaFederal.ListarPaginado<UnidadeReceitaFederalVM>(o =>
				(
					(
						pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					)
				),
				pagedFilter);

			return unidadeReceitaFederal;
		}

		public void RegrasSalvar(UnidadeReceitaFederalVM unidadeReceitaFederal)
		{
			if (unidadeReceitaFederal == null) { return; }

			// Salva UnidadeReceitaFederal
			var unidadeReceitaFederalEntity = AutoMapper.Mapper.Map<UnidadeReceitaFederalEntity>(unidadeReceitaFederal);

			if (unidadeReceitaFederalEntity == null) { return; }

			if (unidadeReceitaFederal.IdUnidadeReceitaFederal.HasValue)
			{
				unidadeReceitaFederalEntity = _uowSciex.QueryStackSciex.UnidadeReceitaFederal.Selecionar(x => x.IdUnidadeReceitaFederal == unidadeReceitaFederal.IdUnidadeReceitaFederal);

				unidadeReceitaFederalEntity = AutoMapper.Mapper.Map(unidadeReceitaFederal, unidadeReceitaFederalEntity);
			}

			_uowSciex.CommandStackSciex.UnidadeReceitaFederal.Salvar(unidadeReceitaFederalEntity);
		}

		public void Salvar(UnidadeReceitaFederalVM unidadeReceitaFederalVM)
		{
			RegrasSalvar(unidadeReceitaFederalVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public UnidadeReceitaFederalVM Selecionar(int? idUnidadeReceitaFederal)
		{
			var unidadeReceitaFederalVM = new UnidadeReceitaFederalVM();

			if (!idUnidadeReceitaFederal.HasValue) { return unidadeReceitaFederalVM; }

			var unidadeReceitaFederal = _uowSciex.QueryStackSciex.UnidadeReceitaFederal.Selecionar(x => x.IdUnidadeReceitaFederal == idUnidadeReceitaFederal);

			if (unidadeReceitaFederal == null) { return unidadeReceitaFederalVM; }

			unidadeReceitaFederalVM = AutoMapper.Mapper.Map<UnidadeReceitaFederalVM>(unidadeReceitaFederal);

			return unidadeReceitaFederalVM;
		}

		public void Deletar(int id)
		{
			var unidadeReceitaFederal = _uowSciex.QueryStackSciex.UnidadeReceitaFederal.Selecionar(s => s.IdUnidadeReceitaFederal == id);

			if (unidadeReceitaFederal != null)
			{

				_validation._unidadeReceitaFederalExisteRelacionamentoValidation.ValidateAndThrow(new UnidadeReceitaFederalDto
				{
					TotalEncontradoUnidadeReceitaFederal = unidadeReceitaFederal.ParametrosUnidadeEntrada.Count

				});

				_validation._unidadeReceitaFederalExisteRelacionamentoValidation.ValidateAndThrow(new UnidadeReceitaFederalDto
				{
					TotalEncontradoUnidadeReceitaFederal = unidadeReceitaFederal.ParametrosUnidadeDespacho.Count

				});

				_validation._unidadeReceitaFederalExisteRelacionamentoValidation.ValidateAndThrow(new UnidadeReceitaFederalDto
				{
					TotalEncontradoUnidadeReceitaFederal = unidadeReceitaFederal.PliMercadoriaUnidadeEntrada.Count

				});

				_validation._unidadeReceitaFederalExisteRelacionamentoValidation.ValidateAndThrow(new UnidadeReceitaFederalDto
				{
					TotalEncontradoUnidadeReceitaFederal = unidadeReceitaFederal.PliMercadoriaUnidadeDespacho.Count

				});

			}
			else
			{
				_validation._unidadeReceitaFederalExcluirValidation.ValidateAndThrow(new UnidadeReceitaFederalDto
				{
					ExisteRegistro = false
				});
			}


			if (unidadeReceitaFederal != null)
			{
				_uowSciex.CommandStackSciex.UnidadeReceitaFederal.Apagar(unidadeReceitaFederal.IdUnidadeReceitaFederal);
			}

			_uowSciex.CommandStackSciex.Save();

		}
	}
	
}