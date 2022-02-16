using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	/// <summary>injetar regras de negócio</summary>
	public class ConsultarDIEntradaArquivoGridController : ApiController
	{
		private readonly IConsultarEntradaDIBll _consultarDIEntrada;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="consultarDIEntrada"></param>
		public ConsultarDIEntradaArquivoGridController(IConsultarEntradaDIBll consultarDIEntrada)
		{
			_consultarDIEntrada = consultarDIEntrada;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<DIArquivoEntradaVM> Get([FromUri] ParametrosDIEntradaVM pagedFilter)
		{
			return _consultarDIEntrada.ListarPaginado(pagedFilter);
		}

	}
}