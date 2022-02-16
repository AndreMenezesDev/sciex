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
using System.Runtime.CompilerServices;

namespace Suframa.Sciex.BusinessLogic
{
	public class CodigoUtilizacaoBll : ICodigoUtilizacaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public CodigoUtilizacaoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<CodigoUtilizacaoVM> Listar(CodigoUtilizacaoVM codigoUtilizacaoVM)
		{
			var codigoUtilizacao = _uowSciex.QueryStackSciex.CodigoUtilizacao.Listar<CodigoUtilizacaoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<CodigoUtilizacaoVM>>(codigoUtilizacao);
		}

		public IEnumerable<object> ListarChave(CodigoUtilizacaoVM codigoUtilizacaoVM)
		{
			if (codigoUtilizacaoVM.Descricao == null && codigoUtilizacaoVM.Id == null)
			{
				return new List<object>();
			}
			try
			{
				string descricao = codigoUtilizacaoVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				int valor = Convert.ToInt32(codigoUtilizacaoVM.Descricao);
				codigoUtilizacaoVM.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.CodigoUtilizacao
					.Listar().Where(o =>
							(codigoUtilizacaoVM.Descricao == null || (o.Descricao.ToLower().Contains(codigoUtilizacaoVM.Descricao.ToLower())) || o.Codigo.ToString().Contains(codigoUtilizacaoVM.Descricao.ToLower()))
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdCodigoUtilizacao,
							text = s.Codigo.ToString("D2") + " | " + s.Descricao
						}).Where(o => o.text.Contains(descricao))
						;
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.CodigoUtilizacao
					.Listar().Where(o =>
							(codigoUtilizacaoVM.Descricao == null || (o.Descricao.ToLower().Contains(codigoUtilizacaoVM.Descricao.ToLower())))
						&&
							(codigoUtilizacaoVM.Id == null || o.IdCodigoUtilizacao == codigoUtilizacaoVM.Id)
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdCodigoUtilizacao,
							text = s.Codigo.ToString("D2") + " | " + s.Descricao
						});
				return lista;
			}
		

				var codigoConta = _uowSciex.QueryStackSciex.CodigoUtilizacao
					.Listar().Where(o =>
							(codigoUtilizacaoVM.Descricao == null ||
							(o.Descricao.ToLower().Contains(codigoUtilizacaoVM.Descricao.ToLower()) ||
							(o.Codigo != null && o.Codigo.ToString().Contains(codigoUtilizacaoVM.Descricao.ToString()))))
					&&
							(codigoUtilizacaoVM.Id == null || o.IdCodigoUtilizacao == codigoUtilizacaoVM.Id)
						)
					.OrderBy(o => o.Descricao)
					.Select(
					s => new
					{
						id = s.IdCodigoUtilizacao,
						text = (s.Codigo != null ? ("0" + s.Codigo.ToString()).Slice(2) : "") + " | " + s.Descricao
					});

					return codigoConta;
		}

		public IEnumerable<object> ListarCodigoUtilizacao(CodigoUtilizacaoVM codigoUtilizacaoVM)
		{
			var list = _uowSciex.QueryStackSciex.ControleImportacao.Listar(o => o.IdPliAplicacao == codigoUtilizacaoVM.IdAplicacaoPli);
			if (list == null)
			{
				return new List<object>();
			}
			List<int> listCutIds = new List<int>();
			foreach (var item in list)
			{
				listCutIds.Add(item.IdCodigoUtilizacao);
			}


			var cutds = _uowSciex.QueryStackSciex.CodigoUtilizacao.Listar(o => listCutIds.Contains(o.IdCodigoUtilizacao))
					.OrderBy(o => o.IdCodigoUtilizacao)
					.Select(
						s => new
						{
							id = s.IdCodigoUtilizacao,
							text = s.Codigo.ToString("D2") + " | " + s.Descricao
						});

			return cutds;
		}

		public PagedItems<CodigoUtilizacaoVM> ListarPaginado(CodigoUtilizacaoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<CodigoUtilizacaoVM>(); }

			var codigoUtilizacao = _uowSciex.QueryStackSciex.CodigoUtilizacao.ListarPaginado<CodigoUtilizacaoVM>(o =>
				(
					(
						pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					) &&
					(
						pagedFilter.Status == 2 || o.Status == pagedFilter.Status
					)
				),
				pagedFilter);

			return codigoUtilizacao;
		}


		public bool verificaVinculo(CodigoUtilizacaoVM codigoUtilizacao)
		{
			if (codigoUtilizacao.Inativar == 1)
			{
				var codigoUtilizacaoEntity = _uowSciex.QueryStackSciex.ControleImportacao.Listar(x => x.IdCodigoUtilizacao == codigoUtilizacao.IdCodigoUtilizacao && x.Status == 1);
				if (codigoUtilizacaoEntity.Count > 0)
					return true;

			}
			return false;
		}

		public CodigoUtilizacaoVM RegrasSalvar(CodigoUtilizacaoVM codigoUtilizacao)
		{

			if (codigoUtilizacao == null) { return null; }

			// Salva CodigoUtilizacao
			var codigoUtilizacaoEntity = AutoMapper.Mapper.Map<CodigoUtilizacaoEntity>(codigoUtilizacao);

			if (codigoUtilizacaoEntity == null) { return null; }

			//Verifica Vínculo
			if (verificaVinculo(codigoUtilizacao))
			{
				codigoUtilizacao.MensagemErro = "Operação não realizada. Item está associado a outro registro do sistema";
				return codigoUtilizacao;
			}


			if (codigoUtilizacao.IdCodigoUtilizacao.HasValue)
			{
				codigoUtilizacaoEntity = _uowSciex.QueryStackSciex.CodigoUtilizacao.Selecionar(x => x.IdCodigoUtilizacao == codigoUtilizacao.IdCodigoUtilizacao);

				codigoUtilizacaoEntity = AutoMapper.Mapper.Map(codigoUtilizacao, codigoUtilizacaoEntity);
			}

			_uowSciex.CommandStackSciex.CodigoUtilizacao.Salvar(codigoUtilizacaoEntity);

			return codigoUtilizacao;
		}

		public void Salvar(CodigoUtilizacaoVM codigoUtilizacaoVM)
		{
			RegrasSalvar(codigoUtilizacaoVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public CodigoUtilizacaoVM Selecionar(int? idCodigoUtilizacao)
		{
			var codigoUtilizacaoVM = new CodigoUtilizacaoVM();

			if (!idCodigoUtilizacao.HasValue) { return codigoUtilizacaoVM; }

			var codigoUtilizacao = _uowSciex.QueryStackSciex.CodigoUtilizacao.Selecionar(x => x.IdCodigoUtilizacao == idCodigoUtilizacao);

			//if (codigoUtilizacao == null)
			//{
			//	_validation._codigoUtilizacaoExcluirValidation.ValidateAndThrow(new CodigoUtilizacaoDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}

			codigoUtilizacaoVM = AutoMapper.Mapper.Map<CodigoUtilizacaoVM>(codigoUtilizacao);

			return codigoUtilizacaoVM;
		}

		public void Deletar(int id)
		{
			var codigoUtilizacao = _uowSciex.QueryStackSciex.CodigoUtilizacao.Selecionar(s => s.IdCodigoUtilizacao == id);

			//if (codigoUtilizacao != null)
			//{
			//	_validation._codigoUtilizacaoExisteRelacionamentoValidation.ValidateAndThrow(new CodigoUtilizacaoDto
			//	{
			//		TotalEncontradoCodigoUtilizacao = codigoUtilizacao.Parametros.Count,
			//	});
			//}
			//else
			//{
			//	_validation._codigoUtilizacaoExcluirValidation.ValidateAndThrow(new CodigoUtilizacaoDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}

			if (codigoUtilizacao != null)
			{
				_uowSciex.CommandStackSciex.CodigoUtilizacao.Apagar(codigoUtilizacao.IdCodigoUtilizacao);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}