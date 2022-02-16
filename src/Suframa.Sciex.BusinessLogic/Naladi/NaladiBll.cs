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
	public class NaladiBll : INaladiBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public NaladiBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<NaladiVM> Listar(NaladiVM naladiVM)
		{
			var naladi = _uowSciex.QueryStackSciex.Naladi.Listar<NaladiVM>();
			return AutoMapper.Mapper.Map<IEnumerable<NaladiVM>>(naladi);
		}

		public IEnumerable<object> ListarChave(NaladiVM naladiVM)
		{
			if (naladiVM.Descricao == null && naladiVM.Id == null)
			{
				return new List<object>();
			}
			try
			{
				string descricao = naladiVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				int valor = Convert.ToInt32(naladiVM.Descricao);
				naladiVM.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.Naladi
					.Listar().Where(o =>
							(naladiVM.Descricao == null || (o.Descricao.ToLower().Contains(naladiVM.Descricao.ToLower())) || o.Codigo.ToString().Contains(naladiVM.Descricao.ToLower()))
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdNaladi,
							text = s.Codigo.ToString("D8") + " | " + s.Descricao
						}).Where(o => o.text.Contains(descricao))
						;
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.Naladi
					.Listar().Where(o =>
							(naladiVM.Descricao == null || (o.Descricao.ToLower().Contains(naladiVM.Descricao.ToLower())))
						&&
							(naladiVM.Id == null || o.IdNaladi == naladiVM.Id)
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdNaladi,
							text = s.Codigo.ToString("D8") + " | " + s.Descricao
						});
				return lista;
			}

		}

		public PagedItems<NaladiVM> ListarPaginado(NaladiVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<NaladiVM>(); }

			var naladi = _uowSciex.QueryStackSciex.Naladi.ListarPaginado<NaladiVM>(o =>
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

			return naladi;
		}

		public void RegrasSalvar(NaladiVM naladi)
		{
			if (naladi == null) { return; }

			// Salva Aladi
			var naladiEntity = AutoMapper.Mapper.Map<NaladiEntity>(naladi);

			if (naladiEntity == null) { return; }

			if (naladi.IdNaladi.HasValue)
			{
				naladiEntity = _uowSciex.QueryStackSciex.Naladi.Selecionar(x => x.IdNaladi == naladi.IdNaladi);

				naladiEntity = AutoMapper.Mapper.Map(naladi, naladiEntity);
			}

			_uowSciex.CommandStackSciex.Naladi.Salvar(naladiEntity);
		}

		public void Salvar(NaladiVM naladiVM)
		{
			RegrasSalvar(naladiVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public NaladiVM Selecionar(int? idNaladi)
		{
			var naladiVM = new NaladiVM();

			if (!idNaladi.HasValue) { return naladiVM; }

			var naladi = _uowSciex.QueryStackSciex.Naladi.Selecionar(x => x.IdNaladi == idNaladi);

			if (naladi == null) { return naladiVM; }

			naladiVM = AutoMapper.Mapper.Map<NaladiVM>(naladi);

			return naladiVM;
		}

		public void Deletar(int id)
		{
			var naladi = _uowSciex.QueryStackSciex.Naladi.Selecionar(s => s.IdNaladi == id);

			if (naladi != null)
			{

				_validation._naladiExisteRelacionamentoValidation.ValidateAndThrow(new NaladiDto
				{
					TotalEncontradoNaladi = naladi.Parametros.Count,

				});

				_validation._naladiExisteRelacionamentoValidation.ValidateAndThrow(new NaladiDto
				{
					TotalEncontradoNaladi = naladi.PliMercadoria.Count,

				});

			}
			else
			{
				_validation._naladiExcluirValidation.ValidateAndThrow(new NaladiDto
				{
					ExisteRegistro = false
				});
			}


			if (naladi != null)
			{
				_uowSciex.CommandStackSciex.Naladi.Apagar(naladi.IdNaladi);
			}

			_uowSciex.CommandStackSciex.Save();

		}
	
	}
}