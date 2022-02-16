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
	public class ModalidadePagamentoBll : IModalidadePagamentoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public ModalidadePagamentoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<object> ListarChave(ModalidadePagamentoVM modalidadePagamentoVM)
		{
				if (modalidadePagamentoVM.Descricao == null && modalidadePagamentoVM.Id == null)
				{
					return new List<object>();
				}
				try
				{
					string descricao = modalidadePagamentoVM.Descricao;
					if (descricao == null) throw new ArgumentException("Descricao nula");

					int valor = Convert.ToInt32(modalidadePagamentoVM.Descricao);
					modalidadePagamentoVM.Descricao = valor.ToString();

					var lista = _uowSciex.QueryStackSciex.ModalidadePagamento
						.Listar().Where(o =>
								(modalidadePagamentoVM.Descricao == null || (o.Descricao.ToLower().Contains(modalidadePagamentoVM.Descricao.ToLower())) || o.Codigo.ToString().Contains(modalidadePagamentoVM.Descricao.ToLower()))
							)
						.OrderBy(o => o.Descricao)
						.Select(
							s => new
							{
								id = s.IdModalidadePagamento,
								text = s.Codigo.ToString("D2") + " | " + s.Descricao
							}).Where(o => o.text.Contains(descricao))
							;
					return lista;
				}
				catch
				{
					var lista = _uowSciex.QueryStackSciex.ModalidadePagamento
						.Listar().Where(o =>
								(modalidadePagamentoVM.Descricao == null || (o.Descricao.ToLower().Contains(modalidadePagamentoVM.Descricao.ToLower())))
							&&
								(modalidadePagamentoVM.Id == null || o.IdModalidadePagamento == modalidadePagamentoVM.Id)
							)
						.OrderBy(o => o.Descricao)
						.Select(
							s => new
							{
								id = s.IdModalidadePagamento,
								text = s.Codigo.ToString("D2") + " | " + s.Descricao
							});
					return lista;
				}

			}
		

		public IEnumerable<ModalidadePagamentoVM> Listar(ModalidadePagamentoVM modalidadePagamentoVM)
		{
			var modalidadePagamento = _uowSciex.QueryStackSciex.ModalidadePagamento.Listar<ModalidadePagamentoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<ModalidadePagamentoVM>>(modalidadePagamento);
		}

		public PagedItems<ModalidadePagamentoVM> ListarPaginado(ModalidadePagamentoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<ModalidadePagamentoVM>(); }

			var modalidadePagamento = _uowSciex.QueryStackSciex.ModalidadePagamento.ListarPaginado<ModalidadePagamentoVM>(o =>
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

			return modalidadePagamento;
		}

		public void RegrasSalvar(ModalidadePagamentoVM modalidadePagamento)
		{
			if (modalidadePagamento == null) { return; }

			// Salva ModalidadePagamento
			var modalidadePagamentoEntity = AutoMapper.Mapper.Map<ModalidadePagamentoEntity>(modalidadePagamento);

			if (modalidadePagamentoEntity == null) { return; }

			if (modalidadePagamento.IdModalidadePagamento.HasValue)
			{
				modalidadePagamentoEntity = _uowSciex.QueryStackSciex.ModalidadePagamento.Selecionar(x => x.IdModalidadePagamento == modalidadePagamento.IdModalidadePagamento);

				modalidadePagamentoEntity = AutoMapper.Mapper.Map(modalidadePagamento, modalidadePagamentoEntity);
			}

			_uowSciex.CommandStackSciex.ModalidadePagamento.Salvar(modalidadePagamentoEntity);
		}

		public void Salvar(ModalidadePagamentoVM modalidadePagamentoVM)
		{
			RegrasSalvar(modalidadePagamentoVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public ModalidadePagamentoVM Selecionar(int? idModalidadePagamento)
		{
			var modalidadePagamentoVM = new ModalidadePagamentoVM();

			if (!idModalidadePagamento.HasValue) { return modalidadePagamentoVM; }

			var modalidadePagamento = _uowSciex.QueryStackSciex.ModalidadePagamento.Selecionar(x => x.IdModalidadePagamento == idModalidadePagamento);

			if (modalidadePagamento == null) { return modalidadePagamentoVM; }

			modalidadePagamentoVM = AutoMapper.Mapper.Map<ModalidadePagamentoVM>(modalidadePagamento);

			return modalidadePagamentoVM;
		}

		public void Deletar(int id)
		{
			var modalidadePagamento = _uowSciex.QueryStackSciex.ModalidadePagamento.Selecionar(s => s.IdModalidadePagamento == id);

			if (modalidadePagamento != null)
			{
				//_validation._unidadeCadastradoraDeletarValidation.ValidateAndThrow(new ManterUnidadeCadastradoraDto
				//{
				//	TotalEncontradoRequerimento = unidadeCadastradora.Requerimento.Count
				//});
			}

			if (modalidadePagamento != null)
			{
				_uowSciex.CommandStackSciex.ModalidadePagamento.Apagar(modalidadePagamento.IdModalidadePagamento);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}