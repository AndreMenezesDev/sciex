using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System;

namespace Suframa.Sciex.BusinessLogic
{
	public class ConsultarEntradaDIBll : IConsultarEntradaDIBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public ConsultarEntradaDIBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public PagedItems<DIArquivoEntradaVM> ListarPaginado(ParametrosDIEntradaVM pagedFilter)
		{
			try
			{
				var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
				var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

				PagedItems<DIArquivoEntradaVM> ConsultarDIEntrada = null;
				if (dataInicio != DateTime.MinValue && dataFim != DateTime.MinValue)
				{
					ConsultarDIEntrada = _uowSciex.QueryStackSciex.DiArquivoEntrada.ListarPaginado<DIArquivoEntradaVM>(o =>
					(
						(o.DataHoraRecepcao >= dataInicio && o.DataHoraRecepcao <= dataFim)
						&&
						(
							pagedFilter.Identificador == 0 ||
							(o.Id == pagedFilter.Identificador)
						)

					)
					,
					pagedFilter);

				}
				else
				{
					ConsultarDIEntrada = _uowSciex.QueryStackSciex.DiArquivoEntrada.ListarPaginado<DIArquivoEntradaVM>(o =>
					(
						pagedFilter.Identificador == 0 ||
						(o.Id == pagedFilter.Identificador)

					)
					,
					pagedFilter);
				}
				

				return ConsultarDIEntrada;
			}
			catch (Exception ex)
			{

			}

			return new PagedItems<DIArquivoEntradaVM>();

		}

	}
}
