using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class CorrigirPEDetalheInsumoController : ApiController
	{
		private readonly IPEDetalheInsumoBll _bll;

		public CorrigirPEDetalheInsumoController(IPEDetalheInsumoBll bll)
		{
			_bll = bll;
		}

		public ResultadoCorrigirDetalheInsumoVM Put([FromBody] SalvarDetalheVM vm)
		{
			return _bll.CorrigirDetalhe(vm);
		}

		public ResultadoMensagemProcessamentoVM Post([FromBody] PEDetalheInsumoVM vm)
		{
			return _bll.InativarDetalheInsumo(vm);
		}
	}
}