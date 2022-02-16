using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	/// <summary>Grid de Regime Tributário</summary>
	public class RegimeTributarioGridController : ApiController
	{
		private readonly IRegimeTributarioBll _regimeTributarioBll;

		/// <summary>Regime Tributário injetar regras de negócio</summary>
		/// <param name="regimeTributarioBll"></param>
		public RegimeTributarioGridController(IRegimeTributarioBll regimeTributarioBll)
		{
			_regimeTributarioBll = regimeTributarioBll;
		}

		/// <summary>Obter dados para o grid de natureza jurídica paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de natureza jurídica</param>
		/// <returns></returns>
		public PagedItems<RegimeTributarioVM> Get([FromUri]RegimeTributarioVM pagedFilter)
		{
			return _regimeTributarioBll.ListarPaginado(pagedFilter);
		}
	}
}