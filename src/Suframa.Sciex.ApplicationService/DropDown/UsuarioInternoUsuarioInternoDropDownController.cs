using Suframa.Sciex.BusinessLogic;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Serviço de dropdown de usuário interno de acordo com o usuário interno logado</summary>
	public class UsuarioInternoUsuarioInternoDropDownController : ApiController
	{
		private readonly IUsuarioBll _usuarioBll;

		/// <summary>
		/// Construtor para injetar as dependências
		/// </summary>
		/// <param name="usuarioBll"></param>
		public UsuarioInternoUsuarioInternoDropDownController(IUsuarioBll usuarioBll)
		{
			_usuarioBll = usuarioBll;
		}

		/// <summary>
		/// Lista de acordo com o usuário interno logado
		/// </summary>
		/// <returns></returns>
		public IEnumerable<object> Get()
		{
			return _usuarioBll.ListarParaUsuarioInterno()
				.OrderBy(o => o.IdUsuarioInterno)
				.Select(s => new { key = s.IdUsuarioInterno, value = s.Nome });
		}
	}
}