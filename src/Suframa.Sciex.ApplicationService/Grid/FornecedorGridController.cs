using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	/// <summary>Grid de Regime Tributário</summary>
	public class FornecedorGridController : ApiController
	{
		private readonly IFornecedorBll _fornecedorBll;
		private readonly IPaisBll _paisBll;

		/// <summary>Regime Tributário injetar regras de negócio</summary>
		/// <param name="FornecedorBll"></param>
		public FornecedorGridController(IFornecedorBll FornecedorBll, IPaisBll paisBll)
		{
			_fornecedorBll = FornecedorBll;
			_paisBll = paisBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<FornecedorVM> Get([FromUri]FornecedorVM pagedFilter)
		{
			var fornecedor = _fornecedorBll.ListarPaginado(pagedFilter);
			if (fornecedor != null)
			{
				foreach (var item in fornecedor.Items)
				{
					item.DescricaoPais =  _paisBll.ListarDescricaoPais(item.CodigoPais);
				}
			
			}
			return fornecedor;
		}
	}
}