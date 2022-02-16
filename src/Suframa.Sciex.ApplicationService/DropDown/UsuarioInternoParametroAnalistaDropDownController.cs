using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Serviço UsuarioInternoParametroAnalistaDropDownController</summary>
	public class UsuarioInternoParametroAnalistaDropDownController : ApiController
	{
		private readonly IUsuarioBll _usuarioInternoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="usuarioInternoBll"></param>
		public UsuarioInternoParametroAnalistaDropDownController(IUsuarioBll usuarioInternoBll)
		{
			_usuarioInternoBll = usuarioInternoBll;
		}

		/// <summary>Método get</summary>
		/// <param name="usuarioInternoVM"></param>
		/// <returns></returns>
		public object Get([FromUri]UsuarioInternoVM usuarioInternoVM)
		{
			return _usuarioInternoBll
				.ListarParaParametroAnalista(usuarioInternoVM)
				.OrderBy(o => o.Nome)
				.Select(s => new
				{
					key = s.IdUsuarioInterno,
					value = s.Nome,
					description = s.Nome
				});
		}
	}
}