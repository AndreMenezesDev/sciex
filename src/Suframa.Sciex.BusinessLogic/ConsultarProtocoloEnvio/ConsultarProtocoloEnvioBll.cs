using FluentValidation;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
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
	public class ConsultarProtocoloEnvioBll : IConsultarProtocoloEnvioBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPss;
		private readonly IPliHistoricoBll _pliHistoricoBll;
		private readonly IPliBll _pliBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;


		private string CNPJ { get; set; }

		public ConsultarProtocoloEnvioBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPss,
			IPliHistoricoBll pliHistoricoBll, IPliBll pliBll, IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			_uowSciex = uowSciex;
			_usuarioPss = usuarioPss;
			_pliHistoricoBll = pliHistoricoBll;
			_pliBll = pliBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
			this.CNPJ = _usuarioInformacoesBll.ObterCNPJ().CnpjCpfUnformat();
		}


		public PagedItems<EstruturaPropriaPLIVM> ListarPaginado(EstruturaPropriaPLIVM pagedFilter)
		{
			try
			{

				var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
				var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

				var usuarioNaoEmpresa = _usuarioPss.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat().Length != 14;

				var ConsultarProtocoloEnvio = _uowSciex.QueryStackSciex.EstruturaPropriaPLI.ListarPaginado<EstruturaPropriaPLIVM>(o =>
					(
						//(
						//	pagedFilter.IdEstruturaPropria == -1 || o.IdEstruturaPropria == pagedFilter.IdEstruturaPropria
						//) 
						//&&
						//(
						//	pagedFilter.DataInicio == null || (o.DataEnvio >= dataInicio && o.DataEnvio <= dataFim)
						//)
						//&&
						//(
						//	(pagedFilter.IdEstruturaPropria != -1 || pagedFilter.DataInicio != null) &&
						//	o.CNPJImportador == this.CNPJ
						//)
						//&&
						//(
						//	pagedFilter.NumeroProtocolo == null || o.NumeroProtocolo == pagedFilter.NumeroProtocolo
						//)

						(
							usuarioNaoEmpresa || o.CNPJImportador == this.CNPJ
						)
						&&
						(
							pagedFilter.TipoArquivo == 0 || o.TipoArquivo == pagedFilter.TipoArquivo
						)
						&&
						(
							dataInicio == DateTime.MinValue || (o.DataEnvio >= dataInicio && o.DataEnvio <= dataFim)
						)
						&&
						(
							pagedFilter.NumeroProtocolo == null || o.NumeroProtocolo == pagedFilter.NumeroProtocolo
						)
					),
					pagedFilter);

				return ConsultarProtocoloEnvio;
			}
			catch (Exception ex)
			{
				//ChamaErro("Sistema ConsultarProtocoloEnvio: Nenhum registro encontrado.");

			}

			return new PagedItems<EstruturaPropriaPLIVM>();

		}

		public EstruturaPropriaPLIVM Selecionar(long? idEstruturaPropria)
		{
			var EstruturaPropriaPLIVM = new EstruturaPropriaPLIVM();

			if (!idEstruturaPropria.HasValue) { return EstruturaPropriaPLIVM; }

			var EstruturaPropriaPLI = _uowSciex.QueryStackSciex.EstruturaPropriaPLI.Selecionar(x =>
				x.IdEstruturaPropria == idEstruturaPropria);

			EstruturaPropriaPLIVM = AutoMapper.Mapper.Map<EstruturaPropriaPLIVM>(EstruturaPropriaPLI);

			return EstruturaPropriaPLIVM;
		}

	}
}