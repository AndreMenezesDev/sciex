using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ErroProcessamentoLeGridController : ApiController
    {
		private readonly IErroProcessamentoBll _erroProcessamentoBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="aladiBll"></param>
		public ErroProcessamentoLeGridController(IErroProcessamentoBll erroProcessamentoBll)
		{
			_erroProcessamentoBll = erroProcessamentoBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public DadosErroProcessamentoVM Get([FromUri] EstruturaPropriaLEEntityVM pagedFilter)
		{
			return _erroProcessamentoBll.ListarErrosLePaginado(pagedFilter);
		}
	}
}
