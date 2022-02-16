using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PEDetalheInsumoController : ApiController
	{
		private readonly IPEDetalheInsumoBll _bll;

		public PEDetalheInsumoController(IPEDetalheInsumoBll bll)
		{
			_bll = bll;
		}

		public int Put([FromBody] SalvarDetalheVM vm)
		{
			return _bll.AtualizarDetalhe(vm);
		}

		public int Post([FromBody] SalvarDetalheVM vm)
		{
			return _bll.SalvarNovoDetalhe(vm);
		}

		public bool Delete(int id)
		{
			return _bll.Deletar(id);
		}
	}
}