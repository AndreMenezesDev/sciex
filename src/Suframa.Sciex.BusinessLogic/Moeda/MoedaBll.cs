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

namespace Suframa.Sciex.BusinessLogic
{
	public class MoedaBll : IMoedaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public MoedaBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<object> ListarChave(MoedaVM moedaVM)
		{
				if (moedaVM.Descricao == null && moedaVM.Id == null)
				{
					return new List<object>();
				}
				try
				{
					string descricao = moedaVM.Descricao;
					if (descricao == null) throw new ArgumentException("Descricao nula");

					int valor = Convert.ToInt32(moedaVM.Descricao);
					moedaVM.Descricao = valor.ToString();

					var lista = _uowSciex.QueryStackSciex.Moeda
						.Listar().Where(o =>
								(moedaVM.Descricao == null || (o.Descricao.ToLower().Contains(moedaVM.Descricao.ToLower())) || o.CodigoMoeda.ToString().Contains(moedaVM.Descricao.ToLower()))
							)
						.OrderBy(o => o.Descricao)
						.Select(
							s => new
							{
								id = s.IdMoeda,
								text = s.CodigoMoeda.ToString("D3") + " | " + s.Descricao
							}).Where(o => o.text.Contains(descricao))
							;
					return lista;
				}
				catch
				{
					var lista = _uowSciex.QueryStackSciex.Moeda
						.Listar().Where(o =>
								(moedaVM.Descricao == null || (o.Descricao.ToLower().Contains(moedaVM.Descricao.ToLower())))
							&&
								(moedaVM.Id == null || o.IdMoeda == moedaVM.Id)
							)
						.OrderBy(o => o.Descricao)
						.Select(
							s => new
							{
								id = s.IdMoeda,
								text = s.CodigoMoeda.ToString("D3") + " | " + s.Descricao
							});
					return lista;
				}

			}

		public PagedItems<MoedaVM> ListarPaginado(MoedaVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<MoedaVM>(); }

			var aladi = _uowSciex.QueryStackSciex.Moeda.ListarPaginado<MoedaVM>(o =>
				(
					(
						string.IsNullOrEmpty(pagedFilter.CodigoMoeda.ToString()) ||
						o.CodigoMoeda.Equals(pagedFilter.CodigoMoeda)
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					)
				),
				pagedFilter);

			return aladi;
		}

		public void RegrasSalvar(MoedaVM moeda)
		{
			if (moeda == null) { return; }

			// Salva Aladi
			var moedaEntity = AutoMapper.Mapper.Map<MoedaEntity>(moeda);

			if (moedaEntity == null) { return; }

			if (moeda.IdMoeda.HasValue)
			{
				moedaEntity = _uowSciex.QueryStackSciex.Moeda.Selecionar(x => x.IdMoeda == moeda.IdMoeda);

				moedaEntity = AutoMapper.Mapper.Map(moeda, moedaEntity);
			}

			_uowSciex.CommandStackSciex.Moeda.Salvar(moedaEntity);
		}

		public void Salvar(MoedaVM moedaVM)
		{
			RegrasSalvar(moedaVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public MoedaVM Selecionar(int? idMoeda)
		{
			var moedaVM = new MoedaVM();

			if (!idMoeda.HasValue) { return moedaVM; }

			var moeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(x => x.IdMoeda == idMoeda);

			if (moeda == null) { return moedaVM; }

			moedaVM = AutoMapper.Mapper.Map<MoedaVM>(moeda);

			return moedaVM;
		}

		public void Deletar(int id)
		{
			var moeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(s => s.IdMoeda == id);

			if (moeda != null)
			{
				//_validation._unidadeCadastradoraDeletarValidation.ValidateAndThrow(new ManterUnidadeCadastradoraDto
				//{
				//	TotalEncontradoRequerimento = unidadeCadastradora.Requerimento.Count
				//});
			}

			if (moeda != null)
			{
				_uowSciex.CommandStackSciex.Moeda.Apagar(moeda.IdMoeda);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}