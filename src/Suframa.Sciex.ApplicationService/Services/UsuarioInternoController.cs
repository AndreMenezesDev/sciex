using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Serviço UsuarioInterno</summary>
	public class UsuarioInternoController : ApiController
	{
		private readonly IUsuarioBll _usuarioInternoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="usuarioInternoBll"></param>
		public UsuarioInternoController(IUsuarioBll usuarioInternoBll)
		{
			_usuarioInternoBll = usuarioInternoBll;
		}

		/// <summary>Método get</summary>
		/// <param name="usuarioInternoVM"></param>
		/// <returns></returns>
		public UsuarioInternoVM Get([FromUri]UsuarioInternoVM usuarioInternoVM)
		{
			return _usuarioInternoBll.Selecionar(usuarioInternoVM);
		}
	}
}