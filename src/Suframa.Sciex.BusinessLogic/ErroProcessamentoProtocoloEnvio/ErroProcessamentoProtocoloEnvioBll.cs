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
	public class ErroProcessamentoConsultarProtocoloEnvio : IErroProcessamentoProtocoloEnvioBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public ErroProcessamentoConsultarProtocoloEnvio(IUnitOfWorkSciex uowSciex)
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

			try
			{
				if (pagedFilter == null) { return new PagedItems<ErroProcessamentoVM>(); }


						var ErroProcessamento = _uowSciex.QueryStackSciex.ErroProcessamento.ListarPaginado<ErroProcessamentoVM>(o =>
							(
								(
									pagedFilter.IdSolicitacaoPli == null || o.IdSolicitacaoPli == pagedFilter.IdSolicitacaoPli
								)

							),
							pagedFilter);

						return ErroProcessamento;
			}
			catch (Exception ex)
			{

			}

			return new PagedItems<ErroProcessamentoVM>();
		}
	}

}