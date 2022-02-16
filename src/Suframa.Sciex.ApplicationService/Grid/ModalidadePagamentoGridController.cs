using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ModalidadePagamentoGridController : ApiController
    {
		private readonly IModalidadePagamentoBll _modalidadePagamentoBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="modalidadePagamentoBll"></param>
		public ModalidadePagamentoGridController(IModalidadePagamentoBll modalidadePagamentoBll)
		{
			_modalidadePagamentoBll = modalidadePagamentoBll;
		}

		/// <summary>Obter dados para o grid de modalidadePagamento paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de modalidadePagamento</param>
		/// <returns></returns>
		public PagedItems<ModalidadePagamentoVM> Get([FromUri]ModalidadePagamentoVM pagedFilter)
		{
			return _modalidadePagamentoBll.ListarPaginado(pagedFilter);
		}
	}
}