using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	/// <summary>Grid de Regime Tributário</summary>
	public class ParidadeCambialGridController : ApiController
	{
		private readonly IParidadeCambialBll _paridadeCambialBll;

		/// <summary>Paridade Cambial injetar regras de negócio</summary>
		/// <param name="paridadeCambialBll"></param>
		public ParidadeCambialGridController(IParidadeCambialBll paridadeCambialBll)
		{
			_paridadeCambialBll = paridadeCambialBll;
		}

		/// <summary>Obter dados para o grid de Paridade Cambial paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de Paridade Cambial</param>
		/// <returns></returns>
		public PagedItems<ParidadeCambialDto> Get([FromUri]ParidadeCambialPagedFilterVM pagedFilter)
		{
			var paged = _paridadeCambialBll.ListarPaginado(pagedFilter);
			return paged;
		}
	}
}