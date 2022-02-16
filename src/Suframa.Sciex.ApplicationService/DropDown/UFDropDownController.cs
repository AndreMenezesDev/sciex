using Suframa.Sciex.BusinessLogic;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown UF</summary>
	public class UFDropDownController : ApiController
	{
		private readonly IUFBll _ufBll;
		private readonly IRegimeTributarioMercadoriaBll _regimeTributarioMercadoriaBll;

		/// <summary>Uf injetar regras de negócio</summary>
		/// <param name="ufBll"></param>
		/// <param name="regimeTributarioMercadoriaBll"></param>
		public UFDropDownController(IUFBll ufBll, IRegimeTributarioMercadoriaBll regimeTributarioMercadoriaBll)
		{
			_ufBll = ufBll;
			_regimeTributarioMercadoriaBll = regimeTributarioMercadoriaBll;
		}

		/// <summary>Descrição das siglas dos estados</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get()
		{
			return _regimeTributarioMercadoriaBll
				.ListarUF();
		}
	}
}