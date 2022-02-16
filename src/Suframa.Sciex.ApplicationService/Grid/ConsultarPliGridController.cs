using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ConsultarPliGridController : ApiController
    {
		private readonly IConsultarPliBll _consultarPliBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="consultarPliBll"></param>
		public ConsultarPliGridController(IConsultarPliBll consultarPliBll)
		{
			_consultarPliBll = consultarPliBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<PliVM> Get([FromUri]PliVM pagedFilter)
		{
			return _consultarPliBll.ListarPaginado(pagedFilter);
		}
	}
}
