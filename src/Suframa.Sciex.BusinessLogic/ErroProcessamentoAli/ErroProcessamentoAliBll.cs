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
	public class ErroProcessamentoAliBll : IErroProcessamentoAliBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public ErroProcessamentoAliBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;

		}


		public IEnumerable<ErroProcessamentoVM> Listar(ErroProcessamentoVM ErroProcessamentoVM)
		{
			var ErroProcessamento = _uowSciex.QueryStackSciex.ErroProcessamento.Listar<ErroProcessamentoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<ErroProcessamentoVM>>(ErroProcessamento);
		}

		public PagedItems<ErroProcessamentoVM> ListarPaginado(ErroProcessamentoVM pagedFilter)
		{

			//Verifica na tabela ErroProcessamento o pliMercadoria
			var listaErroProcessamento = _uowSciex.QueryStackSciex.ErroProcessamento.Listar(
				x => x.IdPliMercadoriaOuPliDetalheMercadoria == pagedFilter.IdPliMercadoriaOuPliDetalheMercadoria).FirstOrDefault();

			if (listaErroProcessamento != null)
			{
				var listaPli = _uowSciex.QueryStackSciex.ErroProcessamento.Listar(x => x.IdPli == listaErroProcessamento.IdPli);

				foreach (var item in listaPli)
				{

					//ERRO NO PLI DETALHE DA MERCADORIA
					if (item.CodigoNivelErro == 3)
					{
						//LISTA DETALHE DA MERCADORIA PARA BUSCAR O ID DA MERCADORIA
						var pliDetalheMercadoria = _uowSciex.QueryStackSciex.PliDetalheMercadoria.Listar
							(x => x.IdPliDetalheMercadoria == item.IdPliMercadoriaOuPliDetalheMercadoria).FirstOrDefault();

						// VERIFICA SE O PLIMERCADORIA DA TABELA DETALHE MERCADORIA É O MESMO DO PLIMERCADORIA SELECIONADO
						if (pliDetalheMercadoria.IdPliMercadoria == pagedFilter.IdPliMercadoriaOuPliDetalheMercadoria)
						{
							var ErroProcessamento = _uowSciex.QueryStackSciex.ErroProcessamento.ListarPaginado<ErroProcessamentoVM>(o =>
							(
								(
									item.IdPliMercadoriaOuPliDetalheMercadoria == null || o.IdPliMercadoriaOuPliDetalheMercadoria == item.IdPliMercadoriaOuPliDetalheMercadoria
								)
								&&
								(
									item.CodigoNivelErro == null || o.CodigoNivelErro == item.CodigoNivelErro
								)

							),
							pagedFilter);

							return ErroProcessamento;

						}
					}
				}

				//ERRO NA MERCADORIA
				if (listaErroProcessamento.CodigoNivelErro == 2)
				{
					if (pagedFilter == null) { return new PagedItems<ErroProcessamentoVM>(); }
					var ErroProcessamento = _uowSciex.QueryStackSciex.ErroProcessamento.ListarPaginado<ErroProcessamentoVM>(o =>
						(
								pagedFilter.IdPliMercadoriaOuPliDetalheMercadoria == null || o.IdPliMercadoriaOuPliDetalheMercadoria == pagedFilter.IdPliMercadoriaOuPliDetalheMercadoria

						),
						pagedFilter);
					return ErroProcessamento;
				}

				//ERRO NO PLI
				if (listaErroProcessamento.CodigoNivelErro == 1)
				{
					if (pagedFilter == null) { return new PagedItems<ErroProcessamentoVM>(); }
					var ErroProcessamento = _uowSciex.QueryStackSciex.ErroProcessamento.ListarPaginado<ErroProcessamentoVM>(o =>
						(
							(
								pagedFilter.IdPli == null || o.IdPli == pagedFilter.IdPli
							)

						),
						pagedFilter);
					return ErroProcessamento;
				}
			}

			return new PagedItems<ErroProcessamentoVM>();
		}

	}

}

				