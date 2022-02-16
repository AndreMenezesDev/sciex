using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Suframa.Sciex.BusinessLogic
{
	public class EstruturaPropriaPli : IEstruturaPropriaPliBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public EstruturaPropriaPli(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<EstruturaPropriaPLIVM> Listar(EstruturaPropriaPLIVM estruturaPropriaPLIVM)
		{
			var estruturapropria = _uowSciex.QueryStackSciex.Aladi.Listar<EstruturaPropriaPLIVM>();
			return AutoMapper.Mapper.Map<IEnumerable<EstruturaPropriaPLIVM>>(estruturapropria);
		}

		public IEnumerable<object> ListarChave(EstruturaPropriaPLIVM estruturaPropriaPLIVM)
		{
			return new List<object>();
		}

		public PagedItems<EstruturaPropriaPLIVM> ListarPaginado(EstruturaPropriaPLIVM pagedFilter)
		{
			try
			{
				if (pagedFilter == null) { return new PagedItems<EstruturaPropriaPLIVM>(); }
				var aladi = _uowSciex.QueryStackSciex.Aladi.ListarPaginado<EstruturaPropriaPLIVM>(o =>
					(
						(
							pagedFilter.IdEstruturaPropria == -1 || o.Codigo == pagedFilter.IdEstruturaPropria
						)
					),
					pagedFilter);

				return aladi;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message.ToString());

			}
			return new PagedItems<EstruturaPropriaPLIVM>();
		}

		public void RegrasSalvar(EstruturaPropriaPLIVM estruturapropria)
		{
			if (estruturapropria == null)
			{
				return;
			}
			var estruturaPropriaEntity = AutoMapper.Mapper.Map<EstruturaPropriaPliEntity>(estruturapropria);

			if (estruturaPropriaEntity == null)
			{
				return;
			}

			if (estruturapropria.IdEstruturaPropria != 0)
			{
				estruturaPropriaEntity = _uowSciex.QueryStackSciex.EstruturaPropriaPLI.Selecionar(x => x.IdEstruturaPropria == estruturapropria.IdEstruturaPropria);
				estruturaPropriaEntity = AutoMapper.Mapper.Map(estruturapropria, estruturaPropriaEntity);
			}

			_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(estruturaPropriaEntity);
		}

		public void Salvar(EstruturaPropriaPLIVM estruturaPropriaPLIVM)
		{
			RegrasSalvar(estruturaPropriaPLIVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public EstruturaPropriaPLIVM Selecionar(int? idEstruturaPropriaPli)
		{
			var estruturaPropriaPLIVM = new EstruturaPropriaPLIVM();
			if (!idEstruturaPropriaPli.HasValue)
			{
				return estruturaPropriaPLIVM;
			}

			var estruturapropria = _uowSciex.QueryStackSciex.EstruturaPropriaPLI.Selecionar(x => x.IdEstruturaPropria == idEstruturaPropriaPli);
			//if (estruturapropria == null)
			//{
			//	_validation._aladiExcluirValidation.ValidateAndThrow(new AladiDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}

			var estruturapropriaVM = AutoMapper.Mapper.Map<EstruturaPropriaPLIVM>(estruturapropria);

			return estruturapropriaVM;
		}

		public void Deletar(int id)
		{
			var estruturapropria = _uowSciex.QueryStackSciex.EstruturaPropriaPLI.Selecionar(s => s.IdEstruturaPropria == id);
			//if (aladi != null)
			//{

			//	_validation._aladiExisteRelacionamentoValidation.ValidateAndThrow(new AladiDto
			//	{
			//		TotalEncontradoAladi = aladi.Parametros.Count,

			//	});

			//	_validation._aladiExisteRelacionamentoValidation.ValidateAndThrow(new AladiDto
			//	{
			//		TotalEncontradoAladi = aladi.PliMercadoria.Count,

			//	});

			//}
			//else
			//{
			//	_validation._aladiExcluirValidation.ValidateAndThrow(new AladiDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}
			if (estruturapropria != null)
			{
				_uowSciex.CommandStackSciex.EstruturaPropriaPli.Apagar(estruturapropria.IdEstruturaPropria);
			}
			_uowSciex.CommandStackSciex.Save();
		}

	}
}