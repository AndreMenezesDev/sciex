using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class LEAnaliseInsumoGridController : ApiController
    {
		private readonly ILEInsumoBll _leInsumoBll;

		public LEAnaliseInsumoGridController(ILEInsumoBll leInsumoBll)
		{
			_leInsumoBll = leInsumoBll;
		}

		public LEAnaliseInsumoVM Get(int id)
		{
			return _leInsumoBll.SelecionarInsumoAtualEAnterior(id);
		}
	}
}
