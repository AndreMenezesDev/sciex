using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PEInsumoController : ApiController
	{
		private readonly IPEInsumoBll _bll;

		public PEInsumoController(IPEInsumoBll bll)
		{
			_bll = bll;
		}
		
		public PEInsumoVM Get(int id)
		{
			return _bll.SelecionarPEInsumo(id);
		}

		//public IEnumerable<LEInsumoVM> Get([FromUri]LEInsumoVM vm)
		//{
		//	return _bll.Listar(vm);
		//}

		public bool Put([FromBody] PEInsumoVM vm)
		{
			return _bll.AtualizarInsumo(vm);
		}

		public void Delete(int id)
		{
			_bll.Deletar(id);
		}
	}
}