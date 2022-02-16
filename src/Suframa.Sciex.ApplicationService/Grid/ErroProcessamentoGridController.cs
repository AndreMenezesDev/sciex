using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ErroProcessamentoGridController : ApiController
    {
		private readonly IErroProcessamentoBll _erroProcessamentoBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="aladiBll"></param>
		public ErroProcessamentoGridController(IErroProcessamentoBll erroProcessamentoBll)
		{
			_erroProcessamentoBll = erroProcessamentoBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<ErroProcessamentoVM> Get([FromUri]ErroProcessamentoVM pagedFilter)
		{
			return _erroProcessamentoBll.ListarPaginado(pagedFilter);
		}
	}
}
