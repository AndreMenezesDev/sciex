using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.BusinessLogic;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Cadsuf.ApplicationService.DropDown
{
	/// <summary>Dropdown dos paises</summary>
	public class PaisDropDownController : ApiController
	{
		private readonly IPaisBll _paisBll;

		/// <summary>Países injetar regras de negócio</summary>
		/// <param name="paisBll"></param>
		public PaisDropDownController(IPaisBll paisBll)
		{
			_paisBll = paisBll;
		}

		/// <summary>Obter ID e Descrição dos Paises</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] PaisVM pais)
		{
			return _paisBll.ListarPaises(pais);
		}
	}
}