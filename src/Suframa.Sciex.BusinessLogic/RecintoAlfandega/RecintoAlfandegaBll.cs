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
	public class RecintoAlfandegaBll : IRecintoAlfandegaBll
	{

		private readonly IUnitOfWorkSciex _uowSciex;
		public RecintoAlfandegaBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public PagedItems<RecintoAlfandegaVM> ListarPaginado(RecintoAlfandegaVM pagedFilter)
		{

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

			var resultadoEntity = _uowSciex.QueryStackSciex.RecintoAlfandega.ListarPaginado(o =>
				(
					(
						pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
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

			if (resultadoEntity.Total == 0) { return new PagedItems<RecintoAlfandegaVM>(); }

			#region Tratamento Paginação
			var vmEmbalagem = new RecintoAlfandegaVM();
			var retornoPaginado = new PagedItems<RecintoAlfandegaVM>();

			retornoPaginado.Items = new List<RecintoAlfandegaVM>();
			retornoPaginado.Total = resultadoEntity.Total;

			foreach (var item in resultadoEntity.Items)
			{
				vmEmbalagem.Codigo = item.Codigo;
				vmEmbalagem.Descricao = item.Descricao;
				vmEmbalagem.Id = item.Id;
				vmEmbalagem.Status = item.Status;
				retornoPaginado.Items.Add(vmEmbalagem);
				vmEmbalagem = new RecintoAlfandegaVM();
			}
			#endregion

			return retornoPaginado;
		}

		public RecintoAlfandegaVM VerificaCodigoCadastrado(RecintoAlfandegaVM pagedFilter)
		{
			var resultadoEntity = _uowSciex.QueryStackSciex.RecintoAlfandega.Selecionar(o => o.Codigo == pagedFilter.Codigo);
			if (resultadoEntity == null)
				return null;
			else
				return new RecintoAlfandegaVM
				{
					Codigo = resultadoEntity.Codigo,
					Descricao = resultadoEntity.Descricao,
					Id = resultadoEntity.Id,
					Status = resultadoEntity.Status,
					RowVersion = resultadoEntity.RowVersion

				};
		}

		public RecintoAlfandegaVM SelecionarRecintoAlfandega(int id)
		{
			var resultadoEntity = _uowSciex.QueryStackSciex.RecintoAlfandega.Selecionar(o => o.Id == id);

			if (resultadoEntity == null) { return null; }

			return new RecintoAlfandegaVM
			{
				Codigo = resultadoEntity.Codigo,
				Descricao = resultadoEntity.Descricao,
				Id = resultadoEntity.Id,
				Status = resultadoEntity.Status,
				RowVersion = resultadoEntity.RowVersion

			};
		}

		public int Salvar(RecintoAlfandegaVM objeto)
		{
			if (objeto == null) { return 0; }

			if (objeto.Id == null) { objeto.Id = 0; }

			var tipoEmbalagemEntity = AutoMapper.Mapper.Map<RecintoAlfandegaEntity>(objeto);

			if (tipoEmbalagemEntity == null) { return 0; }

			if (objeto.Id.HasValue)
			{
				tipoEmbalagemEntity = _uowSciex.QueryStackSciex.RecintoAlfandega.Selecionar(x => x.Id == objeto.Id);

				tipoEmbalagemEntity = AutoMapper.Mapper.Map(objeto, tipoEmbalagemEntity);
			}

			_uowSciex.CommandStackSciex.RecintoAlfandega.Salvar(tipoEmbalagemEntity);
			_uowSciex.CommandStackSciex.Save();
			return 1;
		}

		public int AtualizarStatus(RecintoAlfandegaVM objeto)
		{
			var resultadoEntity = _uowSciex.QueryStackSciex.RecintoAlfandega.Selecionar(x => x.Id == objeto.Id);
			resultadoEntity.Status = objeto.Status;
			_uowSciex.CommandStackSciex.RecintoAlfandega.Salvar(resultadoEntity);
			_uowSciex.CommandStackSciex.Save();
			return 1;
		}

	}
}
