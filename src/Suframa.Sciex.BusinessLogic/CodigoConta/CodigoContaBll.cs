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
	public class CodigoContaBll : ICodigoContaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public CodigoContaBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<CodigoContaVM> Listar(CodigoContaVM codigoContaVM)
		{
			var codigoConta = _uowSciex.QueryStackSciex.CodigoConta.Listar<CodigoContaVM>();
			return AutoMapper.Mapper.Map<IEnumerable<CodigoContaVM>>(codigoConta);
		}

		public IEnumerable<object> ListarChave(CodigoContaVM codigoContaVM)
		{

			if (codigoContaVM.Descricao == null && codigoContaVM.Id == null)
			{
				return new List<object>();
			}
			//1 Retorna somente os ativos
			if (codigoContaVM.FiltrarCaptura == 1)
			{
				var codigoContaAtivos = _uowSciex.QueryStackSciex.CodigoConta
					.Listar().Where(o =>
							(codigoContaVM.Descricao == null ||
							(o.Descricao.ToLower().Contains(codigoContaVM.Descricao.ToLower()) ||
							(o.Codigo != null && o.Codigo.ToString().Contains(codigoContaVM.Descricao.ToString()))) && o.Status == 1)
						&&
							(codigoContaVM.Id == null || o.IdCodigoConta == codigoContaVM.Id)
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdCodigoConta,
							text = (s.Codigo != null ? ("0" + s.Codigo.ToString()).Slice(2) : "") + " | " + s.Descricao
						});

				return codigoContaAtivos;
			}
					var codigoConta = _uowSciex.QueryStackSciex.CodigoConta
					.Listar().Where(o =>
							(codigoContaVM.Descricao == null ||
							(o.Descricao.ToLower().Contains(codigoContaVM.Descricao.ToLower()) ||
							(o.Codigo != null && o.Codigo.ToString().Contains(codigoContaVM.Descricao.ToString()))))
						&&
							(codigoContaVM.Id == null || o.IdCodigoConta == codigoContaVM.Id)
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdCodigoConta,
							text = (s.Codigo != null ? ("0" + s.Codigo.ToString()).Slice(2) : "") + " | " + s.Descricao
						});

				return codigoConta;
		}

		public IEnumerable<object> ListarCodigoConta(CodigoContaVM codigoContaVM)
		{
			var list = _uowSciex.QueryStackSciex.ControleImportacao.Listar(o => o.IdPliAplicacao == codigoContaVM.IdAplicacaoPli && o.IdCodigoUtilizacao == codigoContaVM.IdCodigoUtilizacao);
			if (list == null)
			{
				return new List<object>();
			}
			List<int> listCco = new List<int>();
			foreach (var item in list)
			{
				listCco.Add(item.IdCodigoConta);
			}


			var ccods = _uowSciex.QueryStackSciex.CodigoConta.Listar(o => listCco.Contains(o.IdCodigoConta))
					.OrderBy(o => o.IdCodigoConta)
					.Select(
						s => new
						{
							id = s.IdCodigoConta,
							text = s.Codigo.ToString("D2") + " | " + s.Descricao
						});

			return ccods;
		}

		public PagedItems<CodigoContaVM> ListarPaginado(CodigoContaVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<CodigoContaVM>(); }

			var codigoConta = _uowSciex.QueryStackSciex.CodigoConta.ListarPaginado<CodigoContaVM>(o =>
				(
					(
						pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					)&&
					(
						pagedFilter.Status == 2 || o.Status == pagedFilter.Status
					)
				),
				pagedFilter);

			return codigoConta;
		}


		public bool verificaVinculo(CodigoContaVM codigoConta)
		{
			if(codigoConta.Inativar == 1)
			{
				var codigoContaEntity = _uowSciex.QueryStackSciex.ControleImportacao.Listar(x => x.IdCodigoConta == codigoConta.IdCodigoConta && x.Status == 1);
				if (codigoContaEntity.Count > 0)
					return true;

			}
			return false;
		} 

		public CodigoContaVM RegrasSalvar(CodigoContaVM codigoConta)
		{
			if (codigoConta == null) { return null; }

			var codigoContaEntity = AutoMapper.Mapper.Map<CodigoContaEntity>(codigoConta);

			if (codigoContaEntity == null) { return null; }


			//Verifica Vínculo
			if (verificaVinculo(codigoConta))
			{
				codigoConta.MensagemErro = "Operação não realizada. Item está associado a outro registro do sistema";
				return codigoConta;
			}


			if (codigoConta.IdCodigoConta.HasValue)
			{
				codigoContaEntity = _uowSciex.QueryStackSciex.CodigoConta.Selecionar(x => x.IdCodigoConta == codigoConta.IdCodigoConta);

				codigoContaEntity = AutoMapper.Mapper.Map(codigoConta, codigoContaEntity);
			}

			_uowSciex.CommandStackSciex.CodigoConta.Salvar(codigoContaEntity);

			return codigoConta;
		}

		public void Salvar(CodigoContaVM codigoContaVM)
		{
			RegrasSalvar(codigoContaVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public CodigoContaVM Selecionar(int? idCodigoConta)
		{
			var codigoContaVM = new CodigoContaVM();

			if (!idCodigoConta.HasValue) { return codigoContaVM; }

			var codigoConta = _uowSciex.QueryStackSciex.CodigoConta.Selecionar(x => x.IdCodigoConta == idCodigoConta);

			//if (codigoConta == null)
			//{
			//	_validation._codigoContaExcluirValidation.ValidateAndThrow(new CodigoContaDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}

			codigoContaVM = AutoMapper.Mapper.Map<CodigoContaVM>(codigoConta);

			return codigoContaVM;
		}

		public void Deletar(int id)
		{
			var codigoConta = _uowSciex.QueryStackSciex.CodigoConta.Selecionar(s => s.IdCodigoConta == id);

			//if (codigoConta != null)
			//{
			//	_validation._codigoContaExisteRelacionamentoValidation.ValidateAndThrow(new CodigoContaDto
			//	{
			//		TotalEncontradoCodigoConta = codigoConta.Parametros.Count,
			//	});
			//}
			//else
			//{
			//	_validation._codigoContaExcluirValidation.ValidateAndThrow(new CodigoContaDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}

			if (codigoConta != null)
			{
				_uowSciex.CommandStackSciex.CodigoConta.Apagar(codigoConta.IdCodigoConta);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}