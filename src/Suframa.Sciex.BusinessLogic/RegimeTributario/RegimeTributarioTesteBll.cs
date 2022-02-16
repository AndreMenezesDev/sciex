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
	public class RegimeTributarioTesteBll : IRegimeTributarioTesteBll
	{
		private readonly IUnitOfWork _uow;
		private readonly Validation _validation;

		public RegimeTributarioTesteBll(IUnitOfWork uow)
		{
			_uow = uow;
			_validation = new Validation();
		}

		/// <summary>Carregar DTO da Tela 1 - Tela Listar Natureza Grupo</summary>
		/// <returns></returns>
		public PagedItems<RegimeTributarioTesteVM> ListarPaginado(RegimeTributarioTestePagedFilterVM pagedFilter)
		{
			var lista = _uow.QueryStack.RegimeTributarioTeste.ListarPaginado<RegimeTributarioTesteVM>(o =>
				(string.IsNullOrEmpty(pagedFilter.codigo) || pagedFilter.codigo == o.Codigo)
					&& (string.IsNullOrEmpty(pagedFilter.descricao) || o.Descricao.Contains(pagedFilter.descricao)),
				(PagedOptions)pagedFilter);

			return lista;
		}

		public void Salvar(RegimeTributarioTesteVM regimeTributarioTesteVM)
		{
			var entityRegimeTributarioTeste = Mapper.Map<RegimeTributarioTesteEntity>(regimeTributarioTesteVM);

			_uow.CommandStack.RegimeTributarioTeste.Salvar(entityRegimeTributarioTeste);

			_uow.CommandStack.Save();
		}

		/// <summary>Carregar DTO da Tela 3 - Tela Visualizar Natureza Jurídica</summary>
		/// <returns></returns>
		public RegimeTributarioTesteVM Visualizar(RegimeTributarioTesteVM regimeTributarioTesteVM)
		{
			var entity = _uow.QueryStack.RegimeTributarioTeste.Selecionar(x => x.IdRegimeTributario == regimeTributarioTesteVM.IdRegimeTributario);

			var retorno = AutoMapper.Mapper.Map<RegimeTributarioTesteVM>(entity);

			return retorno;
		}

		public RegimeTributarioTesteVM Selecionar(int? idRegimeTributario)
		{
			var regimeTributarioTesteVM = new RegimeTributarioTesteVM();

			if (idRegimeTributario.HasValue)
			{
				var entity = _uow.QueryStack.RegimeTributarioTeste.Selecionar(x => x.IdRegimeTributario == idRegimeTributario);

				if (entity != null)
				{
					regimeTributarioTesteVM = AutoMapper.Mapper.Map<RegimeTributarioTesteVM>(entity);
				}
			}

			return regimeTributarioTesteVM;
		}

		public void Deletar(int id)
		{
			var regimeTributario = _uow.QueryStack.RegimeTributarioTeste.Selecionar(s => s.IdRegimeTributario == id);

			if (regimeTributario != null)
			{
				// Validar
				_uow.CommandStack.RegimeTributarioTeste.Apagar(regimeTributario.IdRegimeTributario);

				_uow.CommandStack.Save();
			}
		}
	}
}