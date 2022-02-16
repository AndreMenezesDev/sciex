using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;
using System.Collections.Generic;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class EmpresaPDIGridController : ApiController
	{

		private readonly IManterBeneficioNCMBll _manterBeneficioNCMBll;

		public EmpresaPDIGridController(IManterBeneficioNCMBll manterBeneficioBll)
		{
			_manterBeneficioNCMBll = manterBeneficioBll;
		}

		public PagedItems<TaxaEmpresaAtuacaoVM> Get([FromUri]TaxaEmpresaAtuacaoVM parametros)
		{
			return _manterBeneficioNCMBll.ListarEmpresaPDI(parametros);
		}

	}
}