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
	public class PliDetalheMercadoriaBll : IPliDetalheMercadoriaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public PliDetalheMercadoriaBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<PliDetalheMercadoriaVM> Listar(PliDetalheMercadoriaVM pliDetalheMercadoriaVM)
		{
			var pliDetalheMercadoria = _uowSciex.QueryStackSciex.PliDetalheMercadoria.Listar(o => pliDetalheMercadoriaVM.IdPliMercadoria == null || o.IdPliMercadoria == pliDetalheMercadoriaVM.IdPliMercadoria);
			return AutoMapper.Mapper.Map<IEnumerable<PliDetalheMercadoriaVM>>(pliDetalheMercadoria);
		}

		public IEnumerable<object> Listar()
		{
			return _uowSciex.QueryStackSciex.PliDetalheMercadoria
				.Listar()
				.OrderBy(o => o.DescricaoDetalhe)
				.Select(
					s => new
					{
						id = s.DescricaoDetalhe,
						text = s.CodigoDetalheMercadoria + " - " + s.DescricaoDetalhe
					});
		} 

		public PagedItems<PliDetalheMercadoriaVM> ListarPaginado(PliDetalheMercadoriaVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<PliDetalheMercadoriaVM>(); }


			var pliDetalheMercadoria = _uowSciex.QueryStackSciex.PliDetalheMercadoria.ListarPaginado<PliDetalheMercadoriaVM>(o =>
				(
					(
						pagedFilter.IdPliMercadoria == 0 || o.IdPliMercadoria == pagedFilter.IdPliMercadoria
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.DescricaoDetalhe) ||
						o.DescricaoDetalhe.Contains(pagedFilter.DescricaoDetalhe)
					)
				),
				pagedFilter);

			return pliDetalheMercadoria;
		}

		public void RegrasSalvar(PliDetalheMercadoriaVM pliDetalheMercadoria)
		{
			if (pliDetalheMercadoria == null) { return; }

			if (pliDetalheMercadoria != null && pliDetalheMercadoria.IdPliMercadoria != null)
			{
				if (QuantidadeDetalhesDaMercadoria(pliDetalheMercadoria.IdPliMercadoria.Value) >= 99)
				{
					pliDetalheMercadoria.MensagemErro = "Uma mercadoria não pode ter mais de 99 itens.";
				}
			}

			// Salva PliDetalheMercadoria
			pliDetalheMercadoria.ValorCondicaoVenda = pliDetalheMercadoria.QuantidadeComercializada * pliDetalheMercadoria.ValorUnitarioCondicaoVenda;
			var pliDetalheMercadoriaEntity = AutoMapper.Mapper.Map<PliDetalheMercadoriaEntity>(pliDetalheMercadoria);

			ViewUnidadeMedidaEntity vwUnidadeMedidaEntidade = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o=>o.IdUnidadeMedida == pliDetalheMercadoria.IdUnidadeMedida);
			if (vwUnidadeMedidaEntidade != null)
			{
				pliDetalheMercadoriaEntity.DescricaoUnidadeMedida = vwUnidadeMedidaEntidade.Descricao;
				pliDetalheMercadoriaEntity.SiglaUnidadeMedida = vwUnidadeMedidaEntidade.Sigla;
			}

			if (pliDetalheMercadoriaEntity == null) { return; }

			if (pliDetalheMercadoria.IdPliDetalheMercadoria.HasValue)
			{
				pliDetalheMercadoriaEntity = _uowSciex.QueryStackSciex.PliDetalheMercadoria.Selecionar(x => x.IdPliDetalheMercadoria == pliDetalheMercadoria.IdPliDetalheMercadoria);

				pliDetalheMercadoriaEntity = AutoMapper.Mapper.Map(pliDetalheMercadoria, pliDetalheMercadoriaEntity);
			}

			_uowSciex.CommandStackSciex.PliDetalheMercadoria.Salvar(pliDetalheMercadoriaEntity);
		}

		public void Salvar(PliDetalheMercadoriaVM pliDetalheMercadoriaVM)
		{
			RegrasSalvar(pliDetalheMercadoriaVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public PliDetalheMercadoriaVM Selecionar(int? idPliDetalheMercadoria)
		{
			var pliDetalheMercadoriaVM = new PliDetalheMercadoriaVM();

			if (!idPliDetalheMercadoria.HasValue) { return pliDetalheMercadoriaVM; }

			var pliDetalheMercadoria = _uowSciex.QueryStackSciex.PliDetalheMercadoria.Selecionar(x => x.IdPliDetalheMercadoria == idPliDetalheMercadoria);

			if (pliDetalheMercadoria == null)
			{

				_validation._pliDetalheMercadoriaExcluirValidation.ValidateAndThrow(new PliDetalheMercadoriaDto
				{
					ExisteRegistro = false
				});
			}

			pliDetalheMercadoriaVM = AutoMapper.Mapper.Map<PliDetalheMercadoriaVM>(pliDetalheMercadoria);

			return pliDetalheMercadoriaVM;
		}

		public void Deletar(long id)
		{
			var pliDetalheMercadoria = _uowSciex.QueryStackSciex.PliDetalheMercadoria.Selecionar(s => s.IdPliDetalheMercadoria == id);
			if (pliDetalheMercadoria != null)
			{

			}
			else
			{
				_validation._pliDetalheMercadoriaExcluirValidation.ValidateAndThrow(new PliDetalheMercadoriaDto
				{
					ExisteRegistro = false
				});
			}
			if (pliDetalheMercadoria != null)
			{
				_uowSciex.CommandStackSciex.PliDetalheMercadoria.Apagar(Convert.ToInt32(pliDetalheMercadoria.IdPliDetalheMercadoria));
			}
			_uowSciex.CommandStackSciex.Save();
		}

		public int QuantidadeDetalhesDaMercadoria(long idMercadoria)
		{
			return _uowSciex.QueryStackSciex.PliDetalheMercadoria.Listar(s => s.IdPliMercadoria == idMercadoria).Count;
		}
	}
}