using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ErroProcessamentoAliGridController : ApiController
    {
		private readonly IErroProcessamentoAliBll _erroProcessamentoAliBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="aladiBll"></param>
		public ErroProcessamentoAliGridController(IErroProcessamentoAliBll erroProcessamentoAliBll)
		{
			_erroProcessamentoAliBll = erroProcessamentoAliBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<ErroProcessamentoVM> Get([FromUri]ErroProcessamentoVM pagedFilter)
		{
			return _erroProcessamentoAliBll.ListarPaginado(pagedFilter);
		}
	}
}
