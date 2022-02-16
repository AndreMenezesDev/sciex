using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ConsultarProtocoloEnvioGridController : ApiController
    {
		private readonly IConsultarProtocoloEnvioBll _consultarProtocoloEnvioBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="consultarPliBll"></param>
		public ConsultarProtocoloEnvioGridController(IConsultarProtocoloEnvioBll consultarProtocoloEnvioBll)
		{
			_consultarProtocoloEnvioBll = consultarProtocoloEnvioBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<EstruturaPropriaPLIVM> Get([FromUri]EstruturaPropriaPLIVM pagedFilter)
		{
			return _consultarProtocoloEnvioBll.ListarPaginado(pagedFilter);
		}
	}
}
