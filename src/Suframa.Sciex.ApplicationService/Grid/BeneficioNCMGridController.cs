using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class BeneficioNCMGridController : ApiController
	{

		private readonly IManterBeneficioNCMBll _manterBeneficioNCMBll;

		public BeneficioNCMGridController(IManterBeneficioNCMBll manterBeneficioBll)
		{
			_manterBeneficioNCMBll = manterBeneficioBll;

		}

		public PagedItems<TaxaNCMBeneficioVM> Get([FromUri]TaxaNCMBeneficioVM pagedFilter)
		{
			return _manterBeneficioNCMBll.ListarPaginado(pagedFilter);
		}

		public TaxaGrupoBeneficioVM Get(int id)
		{
			return _manterBeneficioNCMBll.Selecionar(id);
		}

		public void Delete(int id)
		{
			_manterBeneficioNCMBll.Deletar(id);
		}

		public int Put([FromBody]TaxaNCMBeneficioVM grupoBeneficio)
		{
			return _manterBeneficioNCMBll.Salvar(grupoBeneficio);
		}

	}
}