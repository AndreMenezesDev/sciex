using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Serviço Usuario Interno Logado</summary>
	public class UsuarioInternoLogadoController : ApiController
	{
		private readonly IUsuarioBll _usuarioInternoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="usuarioInternoBll"></param>
		public UsuarioInternoLogadoController(IUsuarioBll usuarioInternoBll)
		{
			_usuarioInternoBll = usuarioInternoBll;
		}

		/// <summary>Método get</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public UsuarioInternoVM Get()
		{
			var usuario = _usuarioInternoBll.SelecionarUsuarioLogado();

			return usuario;
		}


		/// <summary>Salva o UsuarioLogado</summary>
		/// <param name="UsuarioLogado"></param>
		/// <returns></returns>
		public UsuarioInternoVM Put([FromBody]UsuarioInternoVM usuarioInternoVM)
		{
			_usuarioInternoBll.Salvar(usuarioInternoVM);
			return usuarioInternoVM;
		}
	}
}