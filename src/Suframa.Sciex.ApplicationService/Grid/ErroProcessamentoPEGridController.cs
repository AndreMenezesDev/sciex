using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ErroProcessamentoPEGridController : ApiController
    {
		private readonly IErroProcessamentoBll _erroProcessamentoBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="aladiBll"></param>
		public ErroProcessamentoPEGridController(IErroProcessamentoBll erroProcessamentoBll)
		{
			_erroProcessamentoBll = erroProcessamentoBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public DadosErroProcessamentoPEVM Get([FromUri] EstruturaPropriaPEVM pagedFilter)
		{
			return _erroProcessamentoBll.ListarErrosPEPaginado(pagedFilter);
		}
	}
}
