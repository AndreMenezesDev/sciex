/*using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class AliHistoricoController : ApiController
	{
		private readonly IAliHistoricoBll _aliHistoricoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="aliHistoricoBll"></param>
		public AliHistoricoController(IAliHistoricoBll aliHistoricoBll)
		{
			_aliHistoricoBll = aliHistoricoBll;
		}

		///// <summary>Seleciona uma lista de Aladi</summary>
		///// <param name="aliHistoricoVM"></param>
		///// <returns></returns>
		//public IEnumerable<AliHistoricoVM> Get([FromUri]AliHistoricoVM aliHistoricoVM)
		//{
		//	return _aliHistoricoBll.Listar(aliHistoricoVM);
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pagedFilter"></param>
		/// <returns></returns>
		public PagedItems<AliHistoricoVM> Get([FromUri]AliHistoricoVM pagedFilter)
		{
			return _aliHistoricoBll.ListarPaginado(pagedFilter);
		}

	}
}*/