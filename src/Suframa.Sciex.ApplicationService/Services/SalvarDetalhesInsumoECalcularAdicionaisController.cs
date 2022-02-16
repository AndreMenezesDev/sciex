using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class SalvarDetalhesInsumoECalcularAdicionaisController : ApiController
	{
		private readonly IProcessoInsumoBll _bll;

		public SalvarDetalhesInsumoECalcularAdicionaisController(IProcessoInsumoBll bll)
		{
			_bll = bll;
		}

		public bool Put([FromBody] SalvarDetalhePRCInsumoVM objeto)
		{
			return _bll.SalvarNovoDetalheAdicional(objeto);
		}


	}
}