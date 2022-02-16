using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class LEInsumoController : ApiController
	{
		private readonly ILEInsumoBll _leInsumoBll;

		public LEInsumoController(ILEInsumoBll leInsumoBll)
		{
			_leInsumoBll = leInsumoBll;
		}
		
		public LEInsumoVM Get(int id)
		{
			return _leInsumoBll.Selecionar(id);
		}

		public IEnumerable<LEInsumoVM> Get([FromUri]LEInsumoVM vm)
		{
			return _leInsumoBll.Listar(vm);
		}

		public LEInsumoVM Put([FromBody]LEInsumoVM vm)
		{
			vm = _leInsumoBll.Salvar(vm);
			return vm;
		}

		public void Delete(int id)
		{
			_leInsumoBll.Deletar(id);
		}
	}
}