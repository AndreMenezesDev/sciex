using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	
	public class BeneficioController : ApiController
	{
		private readonly IManterBeneficioBll _manterBeneficioBll;

		public BeneficioController(IManterBeneficioBll manterBeneficioBll)
		{
			_manterBeneficioBll = manterBeneficioBll;
		}
		

		public TaxaGrupoBeneficioVM Put([FromBody]TaxaGrupoBeneficioVM grupoBeneficio)
		{
			_manterBeneficioBll.Salvar(grupoBeneficio);
			return grupoBeneficio;
		}

		public TaxaGrupoBeneficioVM Get(int id)
		{
			return _manterBeneficioBll.Selecionar(id);
		}




	}
}