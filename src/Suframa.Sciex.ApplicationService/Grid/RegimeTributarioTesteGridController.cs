using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	/// <summary>Grid de Regime Tributário</summary>
	public class RegimeTributarioTesteGridController : ApiController
	{
		private readonly IRegimeTributarioTesteBll _regimeTributarioTesteBll;

		/// <summary>Regime Tributário injetar regras de negócio</summary>
		/// <param name="regimeTributarioTesteBll"></param>
		public RegimeTributarioTesteGridController(IRegimeTributarioTesteBll regimeTributarioTesteBll)
		{
			_regimeTributarioTesteBll = regimeTributarioTesteBll;
		}

		/// <summary>Obter dados para o grid de natureza jurídica paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de natureza jurídica</param>
		/// <returns></returns>
		public PagedItems<RegimeTributarioTesteVM> Get([FromUri]RegimeTributarioTestePagedFilterVM pagedFilter)
		{
			var paged = _regimeTributarioTesteBll.ListarPaginado(pagedFilter);
			return paged;
		}
	}
}