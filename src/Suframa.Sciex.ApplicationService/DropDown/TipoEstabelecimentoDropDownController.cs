using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown tipo estabelecimento</summary>
	public class TipoEstabelecimentoDropDownController : ApiController
	{
		/// <summary>Obtem ID e descrição dos tipos de estabelecimentos</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get()
		{
			var tipoEstabelecimento = new List<object>
			{
				new { key = 1, value = "Matriz" },
				new { key = 2, value = "Filial" }
			};

			return tipoEstabelecimento;
		}
	}
}