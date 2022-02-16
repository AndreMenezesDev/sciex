using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ErroProcessamentoProtocoloEnvioGridController : ApiController
    {
		private readonly IErroProcessamentoProtocoloEnvioBll _erroProcessamentoProtocoloEnvioBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="aladiBll"></param>
		public ErroProcessamentoProtocoloEnvioGridController(IErroProcessamentoProtocoloEnvioBll erroProcessamentoProtocoloEnvio)
		{
			_erroProcessamentoProtocoloEnvioBll = erroProcessamentoProtocoloEnvio;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<ErroProcessamentoVM> Get([FromUri]ErroProcessamentoVM pagedFilter)
		{
			return _erroProcessamentoProtocoloEnvioBll.ListarPaginado(pagedFilter);
		}
	}
}
