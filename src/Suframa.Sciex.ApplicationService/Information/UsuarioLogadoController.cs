using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Information
{
	/// <summary>
	/// Serviço de informações de usuário logado
	/// </summary>
	public class UsuarioLogadoController : ApiController
	{

		private readonly IUsuarioPssBll _usuarioPssBll;
		/// <summary>
		/// Instancia o objeto de usuário logado pelo token
		/// </summary>
		public UsuarioLogadoController(IUsuarioPssBll usuarioPssBll)
		{
			_usuarioPssBll = usuarioPssBll;
		}

		/// <summary>
		/// Obter informações do usuário logado
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public object Get()
		{
			return _usuarioPssBll.ObterUsuarioLogado();
		}

		[AllowAnonymous]
		public object Get(bool cnpj)
		{
			return _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado;
		}
	}
}