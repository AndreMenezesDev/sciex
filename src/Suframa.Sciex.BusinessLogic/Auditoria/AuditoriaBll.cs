using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.BusinessLogic.Pss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Suframa.Sciex.BusinessLogic
{
	public class AuditoriaBll : IAuditoriaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;
		private readonly IUsuarioPssBll _usuarioPss;
		
		private enum EnumCodigoAplicacao
		{
			NCM = 1,
			BENEFICIO = 2
		}

		public AuditoriaBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPss)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
			_usuarioPss = usuarioPss;
		}

		public PagedItems<AuditoriaVM> ListarPorIdNcm(NcmVM ncm)
		{
			var idAplicacao = (int)EnumCodigoAplicacao.NCM;
			if (ncm == null) { return new PagedItems<AuditoriaVM>(); }

			PagedItems<AuditoriaVM> historico = new PagedItems<AuditoriaVM>();
			if (ncm.IdNcm.HasValue)
			{
				historico = _uowSciex.QueryStackSciex.Auditoria.ListarPaginadoGrafo(q => new AuditoriaVM()
				{
					CpfCnpjResponsavel = q.CpfCnpjResponsavel,
					DataHoraAcao = q.DataHoraAcao,
					DescricaoAcao = q.DescricaoAcao,
					IdAuditoria = q.IdAuditoria,
					IdAuditoriaAplicacao = q.IdAuditoriaAplicacao,
					IdReferencia = q.IdReferencia,
					NomeResponsavel = q.NomeResponsavel,
					Justificativa = q.Justificativa,
					TipoAcao = q.TipoAcao
				},q => q.IdReferencia == ncm.IdNcm
				&&
				 q.IdAuditoriaAplicacao == idAplicacao
				, ncm
				);

				foreach (var item in historico.Items)
				{
					item.DataHoraAcaoString = item.DataHoraAcao != DateTime.MinValue 
													? $@"{item.DataHoraAcao.ToShortDateString()} às {item.DataHoraAcao.ToShortTimeString()}"
													:"-";
				}


				
			}

			return historico;
		}

		public PagedItems<AuditoriaVM> ListarPorIdBeneficio(TaxaGrupoBeneficioVM viewModel)
		{
			var idAplicacao = (int)EnumCodigoAplicacao.BENEFICIO;
			if (viewModel == null) { return new PagedItems<AuditoriaVM>(); }

			PagedItems<AuditoriaVM> historico = new PagedItems<AuditoriaVM>();
			if (viewModel.IdTaxaGrupoBeneficio.HasValue)
			{
				historico = _uowSciex.QueryStackSciex.Auditoria.ListarPaginadoGrafo(q => new AuditoriaVM()
				{
					CpfCnpjResponsavel = q.CpfCnpjResponsavel,
					DataHoraAcao = q.DataHoraAcao,
					DescricaoAcao = q.DescricaoAcao,
					IdAuditoria = q.IdAuditoria,
					IdAuditoriaAplicacao = q.IdAuditoriaAplicacao,
					IdReferencia = q.IdReferencia,
					NomeResponsavel = q.NomeResponsavel,
					Justificativa = q.Justificativa,
					TipoAcao = q.TipoAcao
				}, q => q.IdReferencia == viewModel.IdTaxaGrupoBeneficio
				&&
					q.IdAuditoriaAplicacao == idAplicacao
				,viewModel
				);

				foreach (var item in historico.Items)
				{
					item.DataHoraAcaoString = item.DataHoraAcao != DateTime.MinValue
													? $@"{item.DataHoraAcao.ToShortDateString()} às {item.DataHoraAcao.ToShortTimeString()}"
													: "-";
				}



			}

			return historico;
		}
	}
}