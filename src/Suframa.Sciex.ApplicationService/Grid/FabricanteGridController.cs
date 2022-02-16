using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class FabricanteGridController : ApiController
	{
		private readonly IFabricanteBll _fabricanteBll;
		private readonly IPaisBll _paisBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="fabricanteBll"></param>
		public FabricanteGridController(IFabricanteBll fabricanteBll , IPaisBll paisBll)
		{
			_fabricanteBll = fabricanteBll;
			_paisBll = paisBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<FabricanteVM> Get([FromUri]FabricanteVM pagedFilter)
		{

			var fabricante = _fabricanteBll.ListarPaginado(pagedFilter);
			if (fabricante != null)
			{
				foreach (var item in fabricante.Items)
				{
					item.DescricaoPais = _paisBll.ListarDescricaoPais(item.CodigoPais);
				}

			}
			return fabricante;
		}
	}
}
