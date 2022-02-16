using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class ViewDetalheMercadoriaDropDownController : ApiController
	{
		private readonly IViewDetalheMercadoriaBll _viewDetalheMercadoriaBll;

		/// <summary>Município injetar regras de negócio</summary>
		/// <param name="viewDetalheMercadoriaBll"></param>
		public ViewDetalheMercadoriaDropDownController(IViewDetalheMercadoriaBll viewDetalheMercadoriaBll)
		{
			_viewDetalheMercadoriaBll = viewDetalheMercadoriaBll;
		}

		/// <summary>Obter ID e Descrição dos municípios filtrados</summary>
		/// <returns></returns>	
		public IEnumerable<object> Get([FromUri] ViewDetalheMercadoriaVM viewDetalheMercadoriaVM)
		{
			return _viewDetalheMercadoriaBll.ListarChave(viewDetalheMercadoriaVM);
		}
	}
}