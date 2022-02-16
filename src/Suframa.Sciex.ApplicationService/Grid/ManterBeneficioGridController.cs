using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class ManterBeneficioGridController : ApiController
	{

		private readonly IManterBeneficioBll _manterBeneficioBll;

		public ManterBeneficioGridController(IManterBeneficioBll manterBeneficioBll)
		{
			_manterBeneficioBll = manterBeneficioBll;

		}

		public PagedItems<TaxaGrupoBeneficioVM> Get([FromUri]TaxaGrupoBeneficioVM pagedFilter)
		{
			return _manterBeneficioBll.ListarPaginado(pagedFilter);
		}

	}
}