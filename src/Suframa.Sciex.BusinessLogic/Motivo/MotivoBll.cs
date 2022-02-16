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
	public class MotivoBll : IMotivoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public MotivoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<object> ListarChave(MotivoVM motivoVM)
		{

				if (motivoVM.Descricao == null && motivoVM.Id == null)
				{
					return new List<object>();
				}
				try
				{
					string descricao = motivoVM.Descricao;
					if (descricao == null) throw new ArgumentException("Descricao nula");

					int valor = Convert.ToInt32(motivoVM.Descricao);
					motivoVM.Descricao = valor.ToString();

					var lista = _uowSciex.QueryStackSciex.Motivo
						.Listar().Where(o =>
								(motivoVM.Descricao == null || (o.Descricao.ToLower().Contains(motivoVM.Descricao.ToLower())) || o.Codigo.ToString().Contains(motivoVM.Descricao.ToLower()))
							)
						.OrderBy(o => o.Descricao)
						.Select(
							s => new
							{
								id = s.IdMotivo,
								text = s.Codigo.ToString("D2") + " | " + s.Descricao
							}).Where(o => o.text.Contains(descricao))
							;
					return lista;
				}
				catch
				{
					var lista = _uowSciex.QueryStackSciex.Motivo
						.Listar().Where(o =>
								(motivoVM.Descricao == null || (o.Descricao.ToLower().Contains(motivoVM.Descricao.ToLower())))
							&&
								(motivoVM.Id == null || o.IdMotivo == motivoVM.Id)
							)
						.OrderBy(o => o.Descricao)
						.Select(
							s => new
							{
								id = s.IdMotivo,
								text = s.Codigo.ToString("D2") + " | " + s.Descricao
							});
					return lista;
				}

			}
		

		public IEnumerable<MotivoVM> Listar(MotivoVM motivoVM)
		{
			var motivo = _uowSciex.QueryStackSciex.Motivo.Listar<MotivoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<MotivoVM>>(motivo);
		}

		public PagedItems<MotivoVM> ListarPaginado(MotivoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<MotivoVM>(); }

			var motivo = _uowSciex.QueryStackSciex.Motivo.ListarPaginado<MotivoVM>(o =>
				(
					(
						string.IsNullOrEmpty(pagedFilter.Codigo) ||
						o.Codigo.Equals(pagedFilter.Codigo)
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					)
				),
				pagedFilter);

			return motivo;
		}

		public void RegrasSalvar(MotivoVM motivo)
		{
			if (motivo == null) { return; }

			// Salva Motivo
			var motivoEntity = AutoMapper.Mapper.Map<MotivoEntity>(motivo);

			if (motivoEntity == null) { return; }

			if (motivo.IdMotivo.HasValue)
			{
				motivoEntity = _uowSciex.QueryStackSciex.Motivo.Selecionar(x => x.IdMotivo == motivo.IdMotivo);

				motivoEntity = AutoMapper.Mapper.Map(motivo, motivoEntity);
			}

			_uowSciex.CommandStackSciex.Motivo.Salvar(motivoEntity);
		}

		public void Salvar(MotivoVM motivoVM)
		{
			RegrasSalvar(motivoVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public MotivoVM Selecionar(int? idMotivo)
		{
			var motivoVM = new MotivoVM();

			if (!idMotivo.HasValue) { return motivoVM; }

			var motivo = _uowSciex.QueryStackSciex.Motivo.Selecionar(x => x.IdMotivo == idMotivo);

			if (motivo == null) { return motivoVM; }

			motivoVM = AutoMapper.Mapper.Map<MotivoVM>(motivo);

			return motivoVM;
		}

		public void Deletar(int id)
		{
			var motivo = _uowSciex.QueryStackSciex.Motivo.Selecionar(s => s.IdMotivo == id);

			if (motivo != null)
			{
				//_validation._unidadeCadastradoraDeletarValidation.ValidateAndThrow(new ManterUnidadeCadastradoraDto
				//{
				//	TotalEncontradoRequerimento = unidadeCadastradora.Requerimento.Count
				//});
			}

			if (motivo != null)
			{
				_uowSciex.CommandStackSciex.Motivo.Apagar(motivo.IdMotivo);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}