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
	public class PliAplicacaoBll : IPliAplicacaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public PliAplicacaoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<PliAplicacaoVM> Listar(PliAplicacaoVM pliAplicacaoVM)
		{
			var pliAplicacao = _uowSciex.QueryStackSciex.PliAplicacao.Listar<PliAplicacaoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<PliAplicacaoVM>>(pliAplicacao);
		}

		public IEnumerable<object> Listar()
		{
			List<PliAplicacaoEntity> lista = _uowSciex.QueryStackSciex.PliAplicacao
				.Listar();

			lista.Insert(0, new PliAplicacaoEntity() { IdPliAplicacao = 0, Codigo = 0, Descricao = "TODOS" });

			return lista.OrderBy(o => o.IdPliAplicacao)
				.Select(
					s => new
					{
						id = s.IdPliAplicacao,
						cod = s.Codigo,
						text = s.Descricao
					});						
		}

		public IEnumerable<object> ListarSemTodos()
		{
			List<PliAplicacaoEntity> lista = _uowSciex.QueryStackSciex.PliAplicacao.Listar();

			return lista.OrderBy(o => o.IdPliAplicacao)
				.Select(
					s => new
					{
						id = s.IdPliAplicacao,
						cod = s.Codigo,
						text = s.Descricao
					});
		}

		public PagedItems<PliAplicacaoVM> ListarPaginado(PliAplicacaoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<PliAplicacaoVM>(); }
			

			var pliAplicacao = _uowSciex.QueryStackSciex.PliAplicacao.ListarPaginado<PliAplicacaoVM>(o =>
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

			return pliAplicacao;
		}

		public void RegrasSalvar(PliAplicacaoVM pliAplicacao)
		{
			if (pliAplicacao == null) { return; }

			// Salva PliAplicacao
			var pliAplicacaoEntity = AutoMapper.Mapper.Map<PliAplicacaoEntity>(pliAplicacao);

			if (pliAplicacaoEntity == null) { return; }

			if (pliAplicacao.IdPliAplicacao.HasValue)
			{
				pliAplicacaoEntity = _uowSciex.QueryStackSciex.PliAplicacao.Selecionar(x => x.IdPliAplicacao == pliAplicacao.IdPliAplicacao);

				pliAplicacaoEntity = AutoMapper.Mapper.Map(pliAplicacao, pliAplicacaoEntity);
			}

			_uowSciex.CommandStackSciex.PliAplicacao.Salvar(pliAplicacaoEntity);
		}

		public void Salvar(PliAplicacaoVM pliAplicacaoVM)
		{
			RegrasSalvar(pliAplicacaoVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public PliAplicacaoVM Selecionar(int? idPliAplicacao)
		{
			var pliAplicacaoVM = new PliAplicacaoVM();

			if (!idPliAplicacao.HasValue) { return pliAplicacaoVM; }

			var pliAplicacao = _uowSciex.QueryStackSciex.PliAplicacao.Selecionar(x => x.IdPliAplicacao == idPliAplicacao);

			if (pliAplicacao == null){

				_validation._pliAplicacaoExcluirValidation.ValidateAndThrow(new PliAplicacaoDto
				{
					ExisteRegistro = false
				});
			}

			pliAplicacaoVM = AutoMapper.Mapper.Map<PliAplicacaoVM>(pliAplicacao);

			return pliAplicacaoVM;
		}

		public void Deletar(int id)
		{	

			var pliAplicacao = _uowSciex.QueryStackSciex.PliAplicacao.Selecionar(s => s.IdPliAplicacao == id);

			if (pliAplicacao != null)
			{

				_validation._pliAplicacaoExisteRelacionamentoValidation.ValidateAndThrow(new PliAplicacaoDto
				{
					TotalEncontradoPliAplicacao = pliAplicacao.PliEntity.Count,

				});
								
			}
			else
			{
				_validation._pliAplicacaoExcluirValidation.ValidateAndThrow(new PliAplicacaoDto
				{
					ExisteRegistro = false
				});
			}


			if (pliAplicacao != null)
			{
				_uowSciex.CommandStackSciex.PliAplicacao.Apagar(pliAplicacao.IdPliAplicacao);
			}

			_uowSciex.CommandStackSciex.Save();

		
		}
	}
}