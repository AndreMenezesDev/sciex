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
	public class TipoDeclaracaoBll : ITipoDeclaracaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public TipoDeclaracaoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<TipoDeclaracaoVM> Listar(TipoDeclaracaoVM codigoContaVM)
		{
			var tipoDeclaracao = _uowSciex.QueryStackSciex.CodigoConta.Listar<TipoDeclaracaoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<TipoDeclaracaoVM>>(tipoDeclaracao);
		}

		public IEnumerable<object> ListarChave(TipoDeclaracaoVM codigoContaVM)
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

		public IEnumerable<object> ListarCodigoConta(TipoDeclaracaoVM codigoContaVM)
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

		public PagedItems<TipoDeclaracaoVM> ListarPaginado(TipoDeclaracaoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<TipoDeclaracaoVM>(); }

			var tipodeclaracao = _uowSciex.QueryStackSciex.TipoDeclaracao.ListarPaginado<TipoDeclaracaoVM>(o =>
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

			return tipodeclaracao;
		}

		public bool verificaVinculo(TipoDeclaracaoVM codigoConta)
		{
			if(codigoConta.Inativar == 1)
			{
				var tipoDeclaracaoEntity = _uowSciex.QueryStackSciex.ControleImportacao.Listar(x => x.IdCodigoConta == codigoConta.IdTipoDeclaracao && x.Status == 1);
				if (tipoDeclaracaoEntity.Count > 0)
					return true;

			}
			return false;
		} 

		public TipoDeclaracaoVM RegrasSalvar(TipoDeclaracaoVM tipoDeclaracaoVM)
		{
			if (tipoDeclaracaoVM == null) { return null; }

			var tipoDecalaracaoEntity = AutoMapper.Mapper.Map<TipoDeclaracaoEntity>(tipoDeclaracaoVM);

			if (tipoDecalaracaoEntity == null) { return null; }


			if (tipoDeclaracaoVM.IdTipoDeclaracao.HasValue)
			{
				tipoDecalaracaoEntity = _uowSciex.QueryStackSciex.TipoDeclaracao.Selecionar(x => x.IdTipoDeclaracao == tipoDeclaracaoVM.IdTipoDeclaracao);

				tipoDecalaracaoEntity = AutoMapper.Mapper.Map(tipoDeclaracaoVM, tipoDecalaracaoEntity);
			}

			_uowSciex.CommandStackSciex.TipoDeclaracao.Salvar(tipoDecalaracaoEntity);

			return tipoDeclaracaoVM;
		}

		public void Salvar(TipoDeclaracaoVM codigoContaVM)
		{
			RegrasSalvar(codigoContaVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public TipoDeclaracaoVM Selecionar(int? idTipoDeclaracao)
		{
			var tipoDeclaracaoVM = new TipoDeclaracaoVM();

			if (!idTipoDeclaracao.HasValue) { return tipoDeclaracaoVM; }

			var codigoConta = _uowSciex.QueryStackSciex.TipoDeclaracao.Selecionar(x => x.IdTipoDeclaracao == idTipoDeclaracao);

			tipoDeclaracaoVM = AutoMapper.Mapper.Map<TipoDeclaracaoVM>(codigoConta);

			return tipoDeclaracaoVM;
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