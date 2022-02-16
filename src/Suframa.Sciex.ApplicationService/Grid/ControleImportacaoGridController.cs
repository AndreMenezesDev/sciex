using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class ControleImportacaoGridController : ApiController
	{
		private readonly IControleImportacaoBll _controleImportacaoBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="controleImportacaoBll"></param>
		public ControleImportacaoGridController(IControleImportacaoBll controleImportacaoBll)
		{
			_controleImportacaoBll = controleImportacaoBll;
		}

		/// <summary>Obter dados para o grid de controleImportacao paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de controleImportacao</param>
		/// <returns></returns>
		public PagedItems<ControleImportacaoVM> Get([FromUri]ControleImportacaoVM pagedFilter)
		{
			return _controleImportacaoBll.ListarPaginado(pagedFilter);
		}
	}
}