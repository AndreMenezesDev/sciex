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
	public class FundamentoLegalBll : IFundamentoLegalBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public FundamentoLegalBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<object> ListarChave(FundamentoLegalVM fundamentoLegalVM)
		{
			if (fundamentoLegalVM.Descricao == null && fundamentoLegalVM.Id == null)
			{
				return new List<object>();
			}
			try
			{
				string descricao = fundamentoLegalVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				int valor = Convert.ToInt32(fundamentoLegalVM.Descricao);
				fundamentoLegalVM.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.FundamentoLegal
					.Listar().Where(o =>
							(fundamentoLegalVM.Descricao == null || (o.Descricao.ToLower().Contains(fundamentoLegalVM.Descricao.ToLower())) || o.Codigo.ToString().Contains(fundamentoLegalVM.Descricao.ToLower()))
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdFundamentoLegal,
							text = s.Codigo.ToString("D2") + " | " + s.Descricao
						}).Where(o => o.text.Contains(descricao))
						;
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.FundamentoLegal
					.Listar().Where(o =>
							(fundamentoLegalVM.Descricao == null || (o.Descricao.ToLower().Contains(fundamentoLegalVM.Descricao.ToLower())))
						&&
							(fundamentoLegalVM.Id == null || o.IdFundamentoLegal == fundamentoLegalVM.Id)
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdFundamentoLegal,
							text = s.Codigo.ToString("D2") + " | " + s.Descricao
						});
				return lista;
			}

		}


		/// <summary>Carregar DTO da Tela 1 - Tela Listar Natureza Grupo</summary>
		/// <returns></returns>
		public PagedItems<FundamentoLegalVM> ListarPaginado(FundamentoLegalPagedFilterVM pagedFilter)
		{
			if (pagedFilter == null)
			{
				return new PagedItems<FundamentoLegalVM>();
			}

			var fundamentoLegal = _uowSciex.QueryStackSciex.FundamentoLegal.ListarPaginado<FundamentoLegalVM>(o =>
				(
					(
						pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					) &&

					(
						pagedFilter.TipoAreaBeneficio == null || o.TipoAreaBeneficio == pagedFilter.TipoAreaBeneficio
					)
				),
				pagedFilter);

			foreach (FundamentoLegalVM item in fundamentoLegal.Items)
			{
				switch (item.TipoAreaBeneficio)
				{
					case 1: item.DescricaoArea = "ZFM - Zona Franca de Manaus"; break;
					case 2: item.DescricaoArea = "ALC - Área de Livre Comércio"; break;
					case 3: item.DescricaoArea = "AO - Amazônia Ocidental"; break;
					default:
						item.DescricaoArea = "";
						break;
				}
			}

			return fundamentoLegal;
		}

		public void Salvar(FundamentoLegalVM fundamentoLegalVM)
		{
			var entityFundamentoLegal = Mapper.Map<FundamentoLegalEntity>(fundamentoLegalVM);

			_uowSciex.CommandStackSciex.FundamentoLegal.Salvar(entityFundamentoLegal);

			_uowSciex.CommandStackSciex.Save();
		}

		/// <summary>Carregar DTO da Tela 3 - Tela Visualizar Natureza Jurídica</summary>
		/// <returns></returns>
		public FundamentoLegalVM Visualizar(FundamentoLegalVM fundamentoLegalVM)
		{
			var entity = _uowSciex.QueryStackSciex.FundamentoLegal.Selecionar(x => x.IdFundamentoLegal == fundamentoLegalVM.IdFundamentoLegal);

			var retorno = AutoMapper.Mapper.Map<FundamentoLegalVM>(entity);

			return retorno;
		}

		public FundamentoLegalVM Selecionar(int? idFundamentoLegal)
		{
			var fundamentoLegalVM = new FundamentoLegalVM();

			if (idFundamentoLegal.HasValue)
			{
				var entity = _uowSciex.QueryStackSciex.FundamentoLegal.Selecionar(x => x.IdFundamentoLegal == idFundamentoLegal);

				if (entity != null)
				{
					fundamentoLegalVM = AutoMapper.Mapper.Map<FundamentoLegalVM>(entity);
				}
			}

			return fundamentoLegalVM;
		}

		public void Deletar(int id)
		{
			var fundamentoLegal = _uowSciex.QueryStackSciex.FundamentoLegal.Selecionar(s => s.IdFundamentoLegal == id);

			if (fundamentoLegal != null)
			{

				_validation._fundamentoLegalExisteRelacionamentoValidation.ValidateAndThrow(new FundamentoLegalDto
				{
					TotalEncontradoFundamentoLegal = fundamentoLegal.Parametros.Count,

				});

				_validation._fundamentoLegalExisteRelacionamentoValidation.ValidateAndThrow(new FundamentoLegalDto
				{
					TotalEncontradoFundamentoLegal = fundamentoLegal.PliMercadoria.Count,

				});

			}
			else
			{
				_validation._fundamentoLegalExcluirValidation.ValidateAndThrow(new FundamentoLegalDto
				{
					ExisteRegistro = false
				});
			}


			if (fundamentoLegal != null)
			{
				_uowSciex.CommandStackSciex.FundamentoLegal.Apagar(fundamentoLegal.IdFundamentoLegal);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}