using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class MonitoramentoSiscomexGridController : ApiController
    {
		private readonly IAliArquivoEnvioBll _aliArquivoEnvioBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="aliArquivoEnvio"></param>
		public MonitoramentoSiscomexGridController(IAliArquivoEnvioBll aliArquivoEnvioBll)
		{
			_aliArquivoEnvioBll = aliArquivoEnvioBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aliarquivoenvio</param>
		/// <returns></returns>
		public PagedItems<AliArquivoEnvioVM> Get([FromUri]AliArquivoEnvioVM pagedFilter)
		{
			return _aliArquivoEnvioBll.ListarPaginado(pagedFilter);
		}
	}
}
