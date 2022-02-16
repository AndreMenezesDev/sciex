using FluentValidation;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Mapping;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;


namespace Suframa.Sciex.BusinessLogic
{
	public class SetorArmazenamentoBll : ISetorArmazenamentoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		public SetorArmazenamentoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public IEnumerable<object> ListarRecintoAlfandega()
		{
		
			var cutds = _uowSciex.QueryStackSciex.RecintoAlfandega.Listar(o => o.Id > 0)
					.OrderBy(o => o.Id)
					.Select(
						s => new
						{
							id = s.Id,
							text = s.Codigo.ToString("D7") + " | " + s.Descricao
						});

			return cutds;
		}

		public PagedItems<SetorArmazenamentoVM> ListarPaginado(SetorArmazenamentoVM pagedFilter)
		{

			#region Trativa combobox
			if(pagedFilter.IdRecintoAlfandega == 0)
			{
				pagedFilter.IdRecintoAlfandega = -1;
			}
			#endregion

			#region Status
			var listaStatus = new List<byte>();
			if (pagedFilter.Status == 2)
			{
				listaStatus.Add(1);
				listaStatus.Add(0);
			}
			else
			{
				listaStatus.Add(pagedFilter.Status);
			}
			#endregion

			var resultadoEntity = _uowSciex.QueryStackSciex.SetorArmazenamento.ListarPaginado(o =>
				(
					(
						pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
					) &&						
					(
						pagedFilter.IdRecintoAlfandega == -1 || o.IdRecintoAlfandega == pagedFilter.IdRecintoAlfandega
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					) &&
					(
						listaStatus.Contains(o.Status)
					)
				),
				pagedFilter);

			if (resultadoEntity.Total == 0) { return new PagedItems<SetorArmazenamentoVM>(); }

			#region Tratamento Paginação
			var vmSetorArmazenamento = new SetorArmazenamentoVM();
			var retornoPaginado = new PagedItems<SetorArmazenamentoVM>();

			retornoPaginado.Items = new List<SetorArmazenamentoVM>();
			retornoPaginado.Total = resultadoEntity.Total;

			foreach (var item in resultadoEntity.Items)
			{
				vmSetorArmazenamento.Codigo = item.Codigo;
				vmSetorArmazenamento.Descricao = item.Descricao;
				vmSetorArmazenamento.Id = item.Id;
				vmSetorArmazenamento.IdRecintoAlfandega = item.IdRecintoAlfandega;
				vmSetorArmazenamento.Status = item.Status;
				vmSetorArmazenamento.CodigoRecintoAlfandega = item.RecintoAlfandega.Codigo;

				retornoPaginado.Items.Add(vmSetorArmazenamento);
				vmSetorArmazenamento = new SetorArmazenamentoVM();
			}
			#endregion

			return retornoPaginado;
		}

		public SetorArmazenamentoVM VerificaCodigoCadastrado(SetorArmazenamentoVM pagedFilter)
		{
			var resultadoEntity = _uowSciex.QueryStackSciex.SetorArmazenamento.Selecionar(o => o.Codigo == pagedFilter.Codigo && o.IdRecintoAlfandega == pagedFilter.IdRecintoAlfandega);
			if (resultadoEntity == null)
				return null;
			else
				return new SetorArmazenamentoVM
				{
					Codigo = resultadoEntity.Codigo,
					Descricao = resultadoEntity.Descricao,
					Id = resultadoEntity.Id,
					Status = resultadoEntity.Status,
					RowVersion = resultadoEntity.RowVersion,
					IdRecintoAlfandega = resultadoEntity.IdRecintoAlfandega	
				};
		}

		public SetorArmazenamentoVM SelecionarArmazenamento(int id)
		{
			var resultadoEntity = _uowSciex.QueryStackSciex.SetorArmazenamento.Selecionar(o => o.Id == id);

			if (resultadoEntity == null) { return null; }

			return new SetorArmazenamentoVM
			{
				Codigo = resultadoEntity.Codigo,
				Descricao = resultadoEntity.Descricao,
				Id = resultadoEntity.Id,
				Status = resultadoEntity.Status,
				RowVersion = resultadoEntity.RowVersion,
				IdRecintoAlfandega = resultadoEntity.IdRecintoAlfandega
			};
		}

		public int Salvar(SetorArmazenamentoVM objeto)
		{
			if (objeto == null) { return 0; }

			if (objeto.Id == null) { objeto.Id = 0; }

			var setorArmazenamentoEntity = AutoMapper.Mapper.Map<SetorArmazenamentoEntity>(objeto);

			if (setorArmazenamentoEntity == null) { return 0; }

			if (objeto.Id.HasValue)
			{
				setorArmazenamentoEntity = _uowSciex.QueryStackSciex.SetorArmazenamento.Selecionar(x => x.Id == objeto.Id);

				setorArmazenamentoEntity = AutoMapper.Mapper.Map(objeto, setorArmazenamentoEntity);
			}

			_uowSciex.CommandStackSciex.SetorArmazenamento.Salvar(setorArmazenamentoEntity);
			_uowSciex.CommandStackSciex.Save();
			return 1;
		}

		public int AtualizarStatus(SetorArmazenamentoVM objeto)
		{
			var resultadoEntity = _uowSciex.QueryStackSciex.SetorArmazenamento.Selecionar(x => x.Id == objeto.Id);
			resultadoEntity.Status = objeto.Status;
			_uowSciex.CommandStackSciex.SetorArmazenamento.Salvar(resultadoEntity);
			_uowSciex.CommandStackSciex.Save();
			return 1;
		}

	}
}
