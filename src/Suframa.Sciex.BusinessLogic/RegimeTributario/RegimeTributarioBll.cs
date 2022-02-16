using AutoMapper;
using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Resources;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class RegimeTributarioBll : IRegimeTributarioBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public RegimeTributarioBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<object> ListarDropOpcoesFixas()
		{
			var lista = _uowSciex.QueryStackSciex.RegimeTributario
				.Listar()
				.Where(o => o.Codigo == 3 || o.Codigo == 5 )
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.IdRegimeTributario,
						text = s.Codigo.ToString() + " | " +  s.Descricao
					});
			return lista;
		}

		public IEnumerable<object> ListarDrop()
		{
			return _uowSciex.QueryStackSciex.RegimeTributario
				.Listar()
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.IdRegimeTributario,
						text = s.Descricao
					});
		}

		public IEnumerable<object> ListarChave(RegimeTributarioVM regimeTributarioVM)
		{
			if (regimeTributarioVM.Descricao == null && regimeTributarioVM.Id == null)
			{
				return new List<object>();
			}
			try
			{
				string descricao = regimeTributarioVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				int valor = Convert.ToInt32(regimeTributarioVM.Descricao);
				regimeTributarioVM.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.RegimeTributario
					.Listar().Where(o =>
							(regimeTributarioVM.Descricao == null || (o.Descricao.ToLower().Contains(regimeTributarioVM.Descricao.ToLower())) || o.Codigo.ToString().Contains(regimeTributarioVM.Descricao.ToLower()))
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdRegimeTributario,
							text = s.Codigo.ToString("D1") + " | " + s.Descricao
						}).Where(o => o.text.Contains(descricao))
						;
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.RegimeTributario
					.Listar().Where(o =>
							(regimeTributarioVM.Descricao == null || (o.Descricao.ToLower().Contains(regimeTributarioVM.Descricao.ToLower())))
						&&
							(regimeTributarioVM.Id == null || o.IdRegimeTributario == regimeTributarioVM.Id)
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdRegimeTributario,
							text = s.Codigo.ToString("D1") + " | " + s.Descricao
						});
				return lista;
			}
		}

		public IEnumerable<object> ListarRegimeTributario()
		{
			return _uowSciex.QueryStackSciex.RegimeTributario
				.Listar()
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						key = s.IdRegimeTributario,
						value = s.Descricao
					});
		}

		/// <summary>Carregar DTO da Tela 1 - Tela Listar Natureza Grupo</summary>
		/// <returns></returns>
		public PagedItems<RegimeTributarioVM> ListarPaginado(RegimeTributarioVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<RegimeTributarioVM>(); }


			var lista = _uowSciex.QueryStackSciex.RegimeTributario.ListarPaginado<RegimeTributarioVM>(o =>
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

			return lista;
		}

		public void Salvar(RegimeTributarioVM regimeTributarioVM)
		{
			var entityRegimeTributario = Mapper.Map<RegimeTributarioEntity>(regimeTributarioVM);

			_uowSciex.CommandStackSciex.RegimeTributario.Salvar(entityRegimeTributario);

			_uowSciex.CommandStackSciex.Save();
		}

		/// <summary>Carregar DTO da Tela 3 - Tela Visualizar Natureza Jurídica</summary>
		/// <returns></returns>
		public RegimeTributarioVM Visualizar(RegimeTributarioVM regimeTributarioVM)
		{
			var entity = _uowSciex.QueryStackSciex.RegimeTributario.Selecionar(x => x.IdRegimeTributario == regimeTributarioVM.IdRegimeTributario);

			var retorno = AutoMapper.Mapper.Map<RegimeTributarioVM>(entity);

			return retorno;
		}

		public RegimeTributarioVM Selecionar(int? idRegimeTributario)
		{
			var regimeTributarioVM = new RegimeTributarioVM();

			if (idRegimeTributario.HasValue)
			{
				var entity = _uowSciex.QueryStackSciex.RegimeTributario.Selecionar(x => x.IdRegimeTributario == idRegimeTributario);

				if (entity != null)
				{
					regimeTributarioVM = AutoMapper.Mapper.Map<RegimeTributarioVM>(entity);
				}
			}

			return regimeTributarioVM;
		}

		public void Deletar(int id)
		{
			var regimeTributario = _uowSciex.QueryStackSciex.RegimeTributario.Selecionar(s => s.IdRegimeTributario == id);

			if (regimeTributario != null)
			{

				_validation._regimeTributarioExisteRelacionamentoValidation.ValidateAndThrow(new RegimeTributarioDto
				{
					TotalEncontradoRegimeTributario = regimeTributario.Parametros.Count,

				});

				_validation._regimeTributarioExisteRelacionamentoValidation.ValidateAndThrow(new RegimeTributarioDto
				{
					TotalEncontradoRegimeTributario = regimeTributario.PliMercadoria.Count,

				});

			}
			else
			{
				_validation._regimeTributarioExcluirValidation.ValidateAndThrow(new RegimeTributarioDto
				{
					ExisteRegistro = false
				});
			}


			if (regimeTributario != null)
			{
				_uowSciex.CommandStackSciex.RegimeTributario.Apagar(regimeTributario.IdRegimeTributario);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}