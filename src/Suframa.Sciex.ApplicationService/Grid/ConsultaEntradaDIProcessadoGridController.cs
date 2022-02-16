using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ConsultaEntradaDIProcessadoGridController : ApiController
	{
		private readonly IConsultarEntradaDIProcessadoBll _consultarEntradaDIProcessada;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="consultarEntradaDIProcessada"></param>
		public ConsultaEntradaDIProcessadoGridController(IConsultarEntradaDIProcessadoBll consultarEntradaDIProcessada)
		{
			_consultarEntradaDIProcessada = consultarEntradaDIProcessada;
		}

		/// <summary>Consultar a Entrada DI</summary>
		/// <param name="pagedFilter"></param>
		/// <returns></returns>
		public PagedItems<DIEntradaVM> Get([FromUri] DIEntradaVM pagedFilter)
		{
			return _consultarEntradaDIProcessada.ListarPaginado(pagedFilter);
		}

	}
}