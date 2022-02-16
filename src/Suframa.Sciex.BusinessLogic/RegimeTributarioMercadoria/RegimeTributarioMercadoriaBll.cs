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
	public class RegimeTributarioMercadoriaBll : IRegimeTributarioMercadoriaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uow;
		private readonly Validation _validation;

		public RegimeTributarioMercadoriaBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uow)
		{
			_uowSciex = uowSciex;
			_uow = uow;
			_validation = new Validation();
		}

		public IEnumerable<object> ListarUF()
		{
			return _uow.QueryStack.UF
				.Listar()
				.OrderBy(o => o.SiglaUF)
				.Select(
					s => new
					{
						id = s.SiglaUF,
						text = s.SiglaUF
					});
		}

		public IEnumerable<object> ListarRegimeTributario()
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

		public IEnumerable<RegimeTributarioMercadoriaVM> Listar(RegimeTributarioMercadoriaVM regimeTributarioMercadoriaVM)
		{
			var regimeTributarioMercadoria = _uowSciex.QueryStackSciex.RegimeTributarioMercadoria.Listar<RegimeTributarioMercadoriaVM>();
			return AutoMapper.Mapper.Map<IEnumerable<RegimeTributarioMercadoriaVM>>(regimeTributarioMercadoria);
		}

		public PagedItems<RegimeTributarioMercadoriaVM> ListarPaginado(RegimeTributarioMercadoriaVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<RegimeTributarioMercadoriaVM>(); }

			var regimeTributarioMercadoria = _uowSciex.QueryStackSciex.RegimeTributarioMercadoria.ListarPaginado<RegimeTributarioMercadoriaVM>(o =>
				(
					(
						pagedFilter.CodigoMunicipio == -1 || o.CodigoMunicipio == pagedFilter.CodigoMunicipio
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.UF) || o.UF.Contains(pagedFilter.UF)
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.DescricaoFundamentoLegal) || o.FundamentoLegal.Descricao.Contains(pagedFilter.DescricaoFundamentoLegal.TrimStart())
					) &&
					(
						pagedFilter.IdRegimeTributario == 0 || o.IdRegimeTributario == pagedFilter.IdRegimeTributario
					) &&
					(
						pagedFilter.IdFundamentoLegal == 0 || o.IdFundamentoLegal == pagedFilter.IdFundamentoLegal
					) &&
					(
						pagedFilter.Status == 2 || o.Status == pagedFilter.Status
					)
					&& (
						pagedFilter.DataInicio == null || o.DataInicioVigencia >= pagedFilter.DataInicio && o.DataInicioVigencia <= pagedFilter.DataFim
					)
				),
				pagedFilter);

			return regimeTributarioMercadoria;
		}

		public bool validarRegraSalvar(RegimeTributarioMercadoriaEntity regimeTributarioMercadoriaEntity )
		{
			var regimeTributario = _uowSciex.QueryStackSciex.RegimeTributarioMercadoria.Listar(x =>

			x.CodigoMunicipio == regimeTributarioMercadoriaEntity.CodigoMunicipio &&
			x.UF == regimeTributarioMercadoriaEntity.UF &&
			x.IdRegimeTributario == regimeTributarioMercadoriaEntity.IdRegimeTributario &&
			x.IdFundamentoLegal == regimeTributarioMercadoriaEntity.IdFundamentoLegal);

			if (regimeTributario.Count > 0)
			{
				return true;
			}
			return false;
		}

		public RegimeTributarioMercadoriaVM RegrasSalvar(RegimeTributarioMercadoriaVM regimeTributarioMercadoria)
		{
			if (regimeTributarioMercadoria == null) { return null; }

			if(regimeTributarioMercadoria.CodigoMunicipio == 0)
			{
				regimeTributarioMercadoria.CodigoMunicipio = Convert.ToInt32(regimeTributarioMercadoria.codigoDoMunicipio);
			}
			// Salva RegimeTributarioMercadoria
			var regimeTributarioMercadoriaEntity = AutoMapper.Mapper.Map<RegimeTributarioMercadoriaEntity>(regimeTributarioMercadoria);

			if (regimeTributarioMercadoriaEntity == null) { return null; }

			if (validarRegraSalvar(regimeTributarioMercadoriaEntity)&& regimeTributarioMercadoria.isEditStatus != 1)
			{
				regimeTributarioMercadoria.MensagemErro = "Registro já existente";
				return regimeTributarioMercadoria;
			}
			    regimeTributarioMercadoria.MensagemErro = null;

			if (regimeTributarioMercadoria.IdRegimeTributarioMercadoria.HasValue)
			{
				regimeTributarioMercadoriaEntity = _uowSciex.QueryStackSciex.RegimeTributarioMercadoria.Selecionar(x => x.IdRegimeTributarioMercadoria == regimeTributarioMercadoria.IdRegimeTributarioMercadoria);

				regimeTributarioMercadoriaEntity = AutoMapper.Mapper.Map(regimeTributarioMercadoria, regimeTributarioMercadoriaEntity);
			}

			_uowSciex.CommandStackSciex.RegimeTributarioMercadoria.Salvar(regimeTributarioMercadoriaEntity);

			return regimeTributarioMercadoria;
		}

		public void Salvar(RegimeTributarioMercadoriaVM regimeTributarioMercadoriaVM)
		{
			RegrasSalvar(regimeTributarioMercadoriaVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public RegimeTributarioMercadoriaVM Selecionar(int? idRegimeTributarioMercadoria)
		{
			var regimeTributarioMercadoriaVM = new RegimeTributarioMercadoriaVM();

			if (!idRegimeTributarioMercadoria.HasValue) { return regimeTributarioMercadoriaVM; }

			var regimeTributarioMercadoria = _uowSciex.QueryStackSciex.RegimeTributarioMercadoria.Selecionar(x => x.IdRegimeTributarioMercadoria == idRegimeTributarioMercadoria);

			//if (regimeTributarioMercadoria == null)
			//{
			//	_validation._regimeTributarioMercadoriaExcluirValidation.ValidateAndThrow(new RegimeTributarioMercadoriaDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}

			regimeTributarioMercadoriaVM = AutoMapper.Mapper.Map<RegimeTributarioMercadoriaVM>(regimeTributarioMercadoria);

			return regimeTributarioMercadoriaVM;
		}

		public void Deletar(int id)
		{
			var regimeTributarioMercadoria = _uowSciex.QueryStackSciex.RegimeTributarioMercadoria.Selecionar(s => s.IdRegimeTributarioMercadoria == id);

			//if (regimeTributarioMercadoria != null)
			//{
			//	_validation._regimeTributarioMercadoriaExisteRelacionamentoValidation.ValidateAndThrow(new RegimeTributarioMercadoriaDto
			//	{
			//		TotalEncontradoRegimeTributarioMercadoria = regimeTributarioMercadoria.Parametros.Count,
			//	});
			//}
			//else
			//{
			//	_validation._regimeTributarioMercadoriaExcluirValidation.ValidateAndThrow(new RegimeTributarioMercadoriaDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}

			if (regimeTributarioMercadoria != null)
			{
				_uowSciex.CommandStackSciex.RegimeTributarioMercadoria.Apagar(regimeTributarioMercadoria.IdRegimeTributarioMercadoria);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}