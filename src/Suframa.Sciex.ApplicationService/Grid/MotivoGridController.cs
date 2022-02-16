using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class MotivoGridController : ApiController
    {
		private readonly IMotivoBll _motivoBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="motivoBll"></param>
		public MotivoGridController(IMotivoBll motivoBll)
		{
			_motivoBll = motivoBll;
		}

		/// <summary>Obter dados para o grid de motivo paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de motivo</param>
		/// <returns></returns>
		public PagedItems<MotivoVM> Get([FromUri]MotivoVM pagedFilter)
		{
			return _motivoBll.ListarPaginado(pagedFilter);
		}
	}
}