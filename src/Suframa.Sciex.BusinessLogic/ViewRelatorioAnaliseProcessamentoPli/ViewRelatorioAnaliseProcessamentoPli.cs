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
	public class ViewRelatorioAnaliseProcessamentoPli : IViewRelatorioAnaliseProcessamentoPli
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public ViewRelatorioAnaliseProcessamentoPli(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public PagedItems<ViewRelatorioAnaliseProcessamentoPliVM> ListarPaginado(ViewRelatorioAnaliseProcessamentoPliVM pagedFilter)
		{
			try
			{
				if (pagedFilter == null) { return new PagedItems<ViewRelatorioAnaliseProcessamentoPliVM>(); }


				var viewRelatorioAnalise = _uowSciex.QueryStackSciex.ViewRelatorioAnaliseProcessamentoPli.ListarPaginado<ViewRelatorioAnaliseProcessamentoPliVM>(o =>
					(
						(
							pagedFilter.IdPli == -1 || o.IdPli == pagedFilter.IdPli
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.NumeroPli) ||
							o.NumeroPli.Contains(pagedFilter.NumeroPli)
						) &&
						(
							pagedFilter.NumeroAli == null || o.NumeroAli == pagedFilter.NumeroAli 
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.NomenclaturaComumMercosul) ||
							o.NomenclaturaComumMercosul.Contains(pagedFilter.NomenclaturaComumMercosul)
						) &&
						(
							pagedFilter.IdPliMercadoria == 0 || o.IdPliMercadoria == pagedFilter.IdPliMercadoria
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.CodigoProduto) ||
							o.CodigoProduto.Contains(pagedFilter.CodigoProduto)
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.TipoProduto) ||
							o.TipoProduto.Contains(pagedFilter.TipoProduto)
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.ModeloProduto) ||
							o.ModeloProduto.Contains(pagedFilter.ModeloProduto)
						) &&
						(
							pagedFilter.Item == null || o.Item == pagedFilter.Item
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.DescricaoErroProcessamento) ||
							o.DescricaoErroProcessamento.Contains(pagedFilter.DescricaoErroProcessamento)
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.Origem) ||
							o.Origem.Contains(pagedFilter.Origem)
						) &&
						(
							pagedFilter.QuantidadeErro == null || o.QuantidadeErro == pagedFilter.QuantidadeErro
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.Status) ||
							o.Status.Contains(pagedFilter.Status)
						)
					),
					pagedFilter);



				return viewRelatorioAnalise;
			}
			catch (Exception ex)
			{
				//ChamaErro("Sistema Aladi: Nenhum registro encontrado.");
			}

			return new PagedItems<ViewRelatorioAnaliseProcessamentoPliVM>();
		}
	}
}