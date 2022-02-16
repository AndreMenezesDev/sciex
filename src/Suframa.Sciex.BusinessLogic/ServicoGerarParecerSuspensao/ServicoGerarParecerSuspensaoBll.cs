using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;

namespace Suframa.Sciex.BusinessLogic
{
	public class ServicoGerarParecerSuspensaoBll : IServicoGerarParecerSuspensaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public ServicoGerarParecerSuspensaoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public ParecerTecnicoVM GerarParecerSuspensaoAlterado(GerarParecerSuspensaoVM view)
		{
			//Procedimento para calcular os dados do parecer Suspensao Alterado SUSAL.
			//PROCEDURE[dbo].[ST_SCIEX_PARECER_TECNICO_SUSAL]
			//(@PRC_ID integer, --ID do processo
			//@SOA_ID integer, --ID da solicitacao
			//@PCO_NU bigint output,
			//@PCO_ANO INTEGER OUTPUT) --número de controle

			_uowSciex.QueryStackSciex.IniciarStoreProcedureParecerSuspensaoAlterado(view.IdProcesso, view.IdSolicitacaoAlteracao);

			try
			{
				var result = _uowSciex.QueryStackSciex.ParecerTecnico.ListarGrafo(q => new ParecerTecnicoVM()
				{
					IdProcesso = q.IdProcesso,
					NumeroControle = q.NumeroControle,
					AnoControle = q.AnoControle
				},
				q=> q.IdProcesso == view.IdProcesso).LastOrDefault();

				return result;


			}
			catch (Exception e)
			{
				return new ParecerTecnicoVM();
			}

		}
		
		public ParecerTecnicoVM GerarParecerSuspensaoCancelado(GerarParecerSuspensaoVM view)
		{
			//Procedimento para calcular os dados do parecer Suspensao Cancelado SUSCA.
			//PROCEDURE[dbo].[ST_SCIEX_PARECER_TECNICO_SUSCA]
			//(@PRC_ID integer, --ID do processo
			//@PCO_NU bigint output,
			//@PCO_ANO INTEGER OUTPUT) --número de controle

			_uowSciex.QueryStackSciex.IniciarStoreProcedureParecerSuspensaoCancelado(view.IdProcesso);

			try
			{
				var result = _uowSciex.QueryStackSciex.ParecerTecnico.ListarGrafo(q => new ParecerTecnicoVM()
				{
					IdProcesso = q.IdProcesso,
					NumeroControle = q.NumeroControle,
					AnoControle = q.AnoControle
				},
				q => q.IdProcesso == view.IdProcesso).LastOrDefault();

				return result;

			}
			catch (Exception e)
			{
				return new ParecerTecnicoVM();
			}

		}

	}
}