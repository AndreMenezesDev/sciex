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
	public class NcmExcecaoBll : INcmExcecaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public NcmExcecaoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		//Atenção
		public IEnumerable<NcmExcecaoVM> Listar(NcmExcecaoVM ncmExcecaoVM)
		{
			var dataInicio = ncmExcecaoVM.DataVigenciaInicio == null ? new DateTime(1, 1, 1) : new DateTime(ncmExcecaoVM.DataVigenciaInicio.Value.Year, ncmExcecaoVM.DataVigenciaInicio.Value.Month, ncmExcecaoVM.DataVigenciaInicio.Value.Day);
			var dataFim = ncmExcecaoVM.DataVigenciaFim == null ? new DateTime(1, 1, 1) : new DateTime(ncmExcecaoVM.DataVigenciaFim.Value.Year, ncmExcecaoVM.DataVigenciaFim.Value.Month, ncmExcecaoVM.DataVigenciaFim.Value.Day, 23, 59, 59);

			var ncmExcecao = 
				_uowSciex.QueryStackSciex.NcmExcecao.Listar<NcmExcecaoVM>().
				Where
				(
					o=>					   
						(string.IsNullOrEmpty(ncmExcecaoVM.Codigo) || o.Codigo == ncmExcecaoVM.Codigo.Trim()) &&

						(string.IsNullOrEmpty(ncmExcecaoVM.DescricaoNcm) || o.DescricaoNcm.ToLower().Contains(ncmExcecaoVM.DescricaoNcm.ToLower())) &&

						(ncmExcecaoVM.CodigoSetor == 0 || o.CodigoSetor == ncmExcecaoVM.CodigoSetor) &&

						( string.IsNullOrEmpty(ncmExcecaoVM.DescricaoMunicipio) || o.DescricaoMunicipio.Contains(ncmExcecaoVM.DescricaoMunicipio) ) &&						

						( string.IsNullOrEmpty(ncmExcecaoVM.DescricaoSetor) || o.DescricaoSetor.ToLower().Contains(ncmExcecaoVM.DescricaoSetor.ToLower())) &&

						(string.IsNullOrEmpty(ncmExcecaoVM.UF) || o.UF.ToLower().Contains(ncmExcecaoVM.UF.ToLower())) &&

						(ncmExcecaoVM.Status == 2 || o.Status == ncmExcecaoVM.Status) &&
					
						(
							(ncmExcecaoVM.DataVigenciaInicio == null && ncmExcecaoVM.DataVigenciaFim == null) || (o.DataInicioVigencia >= dataInicio.Date && o.DataInicioVigencia <= dataFim.Date)
						)
				);
			return AutoMapper.Mapper.Map<IEnumerable<NcmExcecaoVM>>(ncmExcecao);
		}

		public IEnumerable<object> ListarChave(NcmExcecaoVM ncmExcecaoVM)
		{

			if (ncmExcecaoVM.DescricaoNcm == null)
			{
				return new List<object>();
			}

			var pais = _uowSciex.QueryStackSciex.NcmExcecao
				.Listar().Where(o => o.DescricaoNcm.ToLower().Contains(ncmExcecaoVM.DescricaoNcm.ToLower())
					|| (o.Codigo != null && o.Codigo.ToString().Contains(ncmExcecaoVM.DescricaoNcm.ToString()))
					|| (o.IdNcmExcecao != null && o.IdNcmExcecao.ToString().Contains(ncmExcecaoVM.DescricaoNcm.ToString()))
					)
				.OrderBy(o => o.DescricaoNcm)
				.Select(
					s => new
					{
						id = s.IdNcmExcecao,
						text = s.DescricaoNcm
					});

			return pais;
		}

		public IEnumerable<object> ListarChave(ViewNcmVM viewNcmVM)
		{

			if (viewNcmVM.Descricao == null && viewNcmVM.Id == 0)
			{
				return new List<object>();
			}

			var ncm = _uowSciex.QueryStackSciex.ViewNcm
				.Listar().Where(o =>
						(viewNcmVM.Descricao == null || (o.Descricao.ToLower().Contains(viewNcmVM.Descricao.ToLower()) || o.CodigoNCM.ToString().Contains(viewNcmVM.Descricao.ToString())))
					&&
						(viewNcmVM.Id == 0 || o.CodigoNCM == viewNcmVM.Id.ToString())
					)
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.CodigoNCM,
						text = s.Descricao
					});

			return ncm;
		}

		public PagedItems<NcmExcecaoVM> ListarPaginado(NcmExcecaoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<NcmExcecaoVM>(); }

			var dataInicio = pagedFilter.DataVigenciaInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataVigenciaInicio.Value.Year, pagedFilter.DataVigenciaInicio.Value.Month, pagedFilter.DataVigenciaInicio.Value.Day);
			var dataFim = pagedFilter.DataVigenciaFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataVigenciaFim.Value.Year, pagedFilter.DataVigenciaFim.Value.Month, pagedFilter.DataVigenciaFim.Value.Day, 23, 59, 59);

			var ncmAmazoniaOcidental = _uowSciex.QueryStackSciex.NcmExcecao.ListarPaginado<NcmExcecaoVM>(o =>
				(
					(
						string.IsNullOrEmpty(pagedFilter.Codigo) ||
						o.Codigo == pagedFilter.Codigo
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.DescricaoNcm) ||
						o.DescricaoNcm.ToLower().Contains(pagedFilter.DescricaoNcm.ToLower())
					) &&
					(
						pagedFilter.CodigoSetor == 0 || o.CodigoSetor == pagedFilter.CodigoSetor
					)
					&&
					/*(
						string.IsNullOrEmpty(pagedFilter.DescricaoMunicipio) ||
						o.DescricaoMunicipio == pagedFilter.DescricaoMunicipio
					)*/
					(
						pagedFilter.CodigoMunicipio == 0 || o.CodigoMunicipio == pagedFilter.CodigoMunicipio
					)
					&&
					(
						string.IsNullOrEmpty(pagedFilter.DescricaoSetor) ||
						o.DescricaoSetor.ToLower().Contains(pagedFilter.DescricaoSetor.ToLower())
					)
					&&
					(
						string.IsNullOrEmpty(pagedFilter.UF) ||
						o.UF.ToLower().Contains(pagedFilter.UF.ToLower())
					)
					&&
					(
						pagedFilter.Status == 2 || o.Status == pagedFilter.Status
					)
					&&
					(
						(pagedFilter.DataVigenciaInicio == null  && pagedFilter.DataVigenciaFim == null) || (o.DataInicioVigencia >= dataInicio.Date && o.DataInicioVigencia <= dataFim.Date)
					)
				),
				pagedFilter);

			foreach (NcmExcecaoVM item in ncmAmazoniaOcidental.Items)
			{
				item.DescricaoMunicipioCodigo = item.CodigoMunicipio.ToString() + " - " + item.DescricaoMunicipio;
			}

			return ncmAmazoniaOcidental;
		}

		private NcmExcecaoVM ValidarRegrasSalvar(NcmExcecaoVM ncmExcecaoVM)
		{
			var ncm = _uowSciex.QueryStackSciex.NcmExcecao.Listar(x => x.Codigo == ncmExcecaoVM.Codigo
						&& x.DescricaoMunicipio.ToLower().Contains(ncmExcecaoVM.DescricaoMunicipio.ToLower())
						&& x.DescricaoSetor.ToLower().Contains(ncmExcecaoVM.DescricaoSetor.ToLower()));

			if (ncm.Any())
			{

				ncmExcecaoVM.MensagemErro = "Registro já existente para a relação" + ncmExcecaoVM.Codigo + ", "
					+ ncmExcecaoVM.DescricaoMunicipio + ", " + ncmExcecaoVM.DescricaoSetor + ".";

			}

			return ncmExcecaoVM;

		}

		public void RegrasSalvar(NcmExcecaoVM ncmExcecao)
		{
			if (ncmExcecao == null) { return; }

			// Salva NcmAmazoniaOcidental
			var ncmExcecaoEntity = AutoMapper.Mapper.Map<NcmExcecaoEntity>(ncmExcecao);

			if (ncmExcecaoEntity == null) { return; }

			if (ncmExcecao.IdNcmExcecao.HasValue)
			{			
				ncmExcecaoEntity = _uowSciex.QueryStackSciex.NcmExcecao.Selecionar(x => x.IdNcmExcecao == ncmExcecao.IdNcmExcecao);
				ncmExcecaoEntity = AutoMapper.Mapper.Map(ncmExcecao, ncmExcecaoEntity);


				ncmExcecaoEntity.Status = ncmExcecao.Status;
				_uowSciex.CommandStackSciex.NcmExcecao.Salvar(ncmExcecaoEntity);
				_uowSciex.CommandStackSciex.Save();

				//ncmExcecaoEntity.Status = ncmExcecao.Status;
			}
			else
			{

				// deletar itens
				var uf = ncmExcecao.UF;
				var ncm = _uowSciex.QueryStackSciex.NcmExcecao.Listar(o => o.Codigo == ncmExcecao.Codigo
					&& o.DescricaoSetor.ToLower().Contains(ncmExcecao.DescricaoSetor.ToLower())
					&& o.UF.ToLower().Contains(uf.ToLower())
					);

				foreach (var item in ncm)
				{
					Deletar(item.IdNcmExcecao);
				}

				if (ncmExcecao.ListaMunicipios != null && ncmExcecao.ListaMunicipios.Any())
				{
					foreach (var item in ncmExcecao.ListaMunicipios)
					{
						ncmExcecaoEntity = AutoMapper.Mapper.Map<NcmExcecaoEntity>(ncmExcecao);
						ncmExcecaoEntity.Codigo = ncmExcecaoEntity.Codigo.Trim();
						ncmExcecaoEntity.CodigoMunicipio = item.CodigoMunicipio;
						ncmExcecaoEntity.DescricaoMunicipio = item.DescricaoMunicipio;
						ncmExcecaoEntity.UF = item.UF;
						ncmExcecaoEntity.Status = item.Status;
						_uowSciex.CommandStackSciex.NcmExcecao.Salvar(ncmExcecaoEntity);
						_uowSciex.CommandStackSciex.Save();
					}
					return;
				}
			}
			
		}

		public NcmExcecaoVM Salvar(NcmExcecaoVM ncmExcecaoVM)
		{

			//ncmExcecaoVM = ValidarRegrasSalvar(ncmExcecaoVM);

			//if (!String.IsNullOrEmpty(ncmExcecaoVM.MensagemErro))
			//{

			//	return ncmExcecaoVM;

			//}

			RegrasSalvar(ncmExcecaoVM);

			return ncmExcecaoVM;

		}

		public NcmExcecaoVM Selecionar(int? idNcmExcecao)
		{
			var NcmExcecaoVM = new NcmExcecaoVM();

			if (!idNcmExcecao.HasValue) { return NcmExcecaoVM; }

			var ncmExcecao = _uowSciex.QueryStackSciex.NcmExcecao.Selecionar(x => x.IdNcmExcecao == idNcmExcecao);

			//if (ncmAmazoniaOcidental == null)
			//{
			//	_validation._ncmAmazoniaOcidentalExcluirValidation.ValidateAndThrow(new NcmAmazoniaOcidentalDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}

			NcmExcecaoVM = AutoMapper.Mapper.Map<NcmExcecaoVM>(ncmExcecao);

			return NcmExcecaoVM;
		}

		public void Deletar(int id)
		{
			var ncmExcecao = _uowSciex.QueryStackSciex.NcmExcecao.Selecionar(s => s.IdNcmExcecao == id);

			//if (ncmAmazoniaOcidental != null)
			//{
			//	_validation._ncmAmazoniaOcidentalExisteRelacionamentoValidation.ValidateAndThrow(new NcmAmazoniaOcidentalDto
			//	{
			//		TotalEncontradoNcmAmazoniaOcidental = ncmAmazoniaOcidental.Parametros.Count,
			//	});
			//}
			//else
			//{
			//	_validation._ncmAmazoniaOcidentalExcluirValidation.ValidateAndThrow(new NcmAmazoniaOcidentalDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}

			if (ncmExcecao != null)
			{
				_uowSciex.CommandStackSciex.NcmExcecao.Apagar(ncmExcecao.IdNcmExcecao);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}