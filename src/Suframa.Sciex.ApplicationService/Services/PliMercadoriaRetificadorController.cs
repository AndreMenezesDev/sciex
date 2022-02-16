using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApPliMercadoriacationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PliMercadoriaRetificadorController : ApiController
	{
		private readonly IPliMercadoriaBll _PliMercadoriaBll;

		public PliMercadoriaRetificadorController(IPliMercadoriaBll PliMercadoriaBll)
		{
			_PliMercadoriaBll = PliMercadoriaBll;
		}
		
		public PliMercadoriaVM Get(int id)
		{
			return null;
		}

		public IEnumerable<PliMercadoriaVM> Get([FromUri]PliMercadoriaVM PliMercadoriaVM)
		{
			return null;
		}

		public PliMercadoriaVM Put([FromBody]PliMercadoriaVM PliMercadoriaVM)
		{
			PliMercadoriaVM = _PliMercadoriaBll.AtualizarNCM(PliMercadoriaVM);
			return PliMercadoriaVM;
		}

	}
}